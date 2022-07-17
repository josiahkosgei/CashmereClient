using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Xml;
using System.Xml.Linq;

namespace DeviceManager
{
  public class EventDrivenTCPClient : IDisposable
  {
    private const int DEFAULTTIMEOUT = 5000;
    private const int RECONNECTINTERVAL = 2000;
    private ConnectionStatus _ConStat;
    private TcpClient _client;
    private byte[] dataBuffer = new byte[3000];
    private System.Timers.Timer tmrReceiveTimeout = new();
    private System.Timers.Timer tmrSendTimeout = new();
    private System.Timers.Timer tmrConnectTimeout = new();

    protected CashmereLogger Log { get; set; }

    public object SyncLock { get; } = new();

    public Encoding DataEncoding { get; set; } = Encoding.Default;

    public ConnectionStatus ConnectionState
    {
      get => _ConStat;
      private set
      {
        if (_ConStat != value)
          Console.WriteLine("Changing Connection State from {0} to {1}", Enum.GetName(typeof (ConnectionStatus), _ConStat), Enum.GetName(typeof (ConnectionStatus), value));
        var flag = value != _ConStat;
        _ConStat = value;
        if (!(ConnectionStatusChanged != null & flag))
          return;

        ConnectionStatusChanged?.BeginInvoke(this, _ConStat, cbChangeConnectionStateComplete, this);
      }
    }

    public bool AutoReconnect { get; set; } = true;

    public int ReconnectInterval { get; set; }

    public string Host { get; } = "";

    public int Port { get; } = 0;

    public int ReceiveTimeout
    {
      get => (int) tmrReceiveTimeout.Interval;
      set => tmrReceiveTimeout.Interval = value;
    }

    public int SendTimeout
    {
      get => (int) tmrSendTimeout.Interval;
      set => tmrSendTimeout.Interval = value;
    }

    public int ConnectTimeout
    {
      get => (int) tmrConnectTimeout.Interval;
      set => tmrConnectTimeout.Interval = value;
    }

    public event delConnectionStatusChanged ConnectionStatusChanged;

    public EventDrivenTCPClient(
      CashmereLogger log,
      string host,
      int port,
      AddressFamily addressFamily,
      bool autoreconnect = true)
    {
      Log = log;
      Host = host;
      Port = port;
      AutoReconnect = autoreconnect;
      _client = new TcpClient(addressFamily);
      _client.NoDelay = true;
      ReceiveTimeout = 5000;
      SendTimeout = 5000;
      ConnectTimeout = 5000;
      ReconnectInterval = 2000;
      tmrReceiveTimeout.AutoReset = false;
      tmrReceiveTimeout.Elapsed += tmrReceiveTimeout_Elapsed;
      tmrConnectTimeout.AutoReset = false;
      tmrConnectTimeout.Elapsed += tmrConnectTimeout_Elapsed;
      tmrSendTimeout.AutoReset = false;
      tmrSendTimeout.Elapsed += tmrSendTimeout_Elapsed;
      ConnectionState = ConnectionStatus.NeverConnected;
    }

    private void tmrSendTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      ConnectionState = ConnectionStatus.SendFail_Timeout;
      DisconnectByHost();
    }

    private void tmrReceiveTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      ConnectionState = ConnectionStatus.ReceiveFail_Timeout;
      DisconnectByHost();
    }

    private void tmrConnectTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      ConnectionState = ConnectionStatus.ConnectFail_Timeout;
      DisconnectByHost();
    }

    private void DisconnectByHost()
    {
      ConnectionState = ConnectionStatus.DisconnectedByHost;
      tmrReceiveTimeout.Stop();
      if (!AutoReconnect)
        return;
      Reconnect();
    }

    private void Reconnect()
    {
      if (ConnectionState == ConnectionStatus.Connected)
        return;
      ConnectionState = ConnectionStatus.AutoReconnecting;
      try
      {
        _client.Client.BeginDisconnect(true, cbDisconnectByHostComplete, _client.Client);
      }
      catch
      {
      }
    }

    public void Connect()
    {
      if (ConnectionState == ConnectionStatus.Connected)
        return;
      ConnectionState = ConnectionStatus.Connecting;
      tmrConnectTimeout.Start();
      try
      {
        _client.BeginConnect(Host, Port, cbConnect, _client.Client);
      }
      catch (Exception ex)
      {
      }
    }

    public void Disconnect()
    {
      if (ConnectionState != ConnectionStatus.Connected)
        return;
      _client.Client.BeginDisconnect(true, cbDisconnectComplete, _client.Client);
    }

    public void Send(string data)
    {
      if (ConnectionState != ConnectionStatus.Connected)
      {
        ConnectionState = ConnectionStatus.SendFail_NotConnected;
      }
      else
      {
        var bytes = DataEncoding.GetBytes(data);
        var errorCode = SocketError.Success;
        tmrSendTimeout.Start();
        _client.Client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, out errorCode, cbSendComplete, _client.Client);
        if (errorCode == 0)
          return;
        DisconnectByHost();
      }
    }

    public void Send(byte[] data)
    {
      if (ConnectionState != ConnectionStatus.Connected)
        throw new InvalidOperationException("Cannot send data, socket is not connected");
      var errorCode = SocketError.Success;
      _client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, out errorCode, cbSendComplete, _client.Client);
      if (errorCode == 0)
        return;
      DisconnectByHost();
    }

    public void Dispose()
    {
      _client.Close();
      _client.Client.Dispose();
    }

    private void cbConnectComplete()
    {
      if (_client.Connected)
      {
        tmrConnectTimeout.Stop();
        ConnectionState = ConnectionStatus.Connected;
        try
        {
          Task.Run(() => StartListening());
        }
        catch (Exception ex)
        {
          ConnectionState = ConnectionStatus.Error;
        }
      }
      else
        ConnectionState = ConnectionStatus.Error;
    }

    private void cbDisconnectByHostComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is Socket asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      try
      {
        asyncState.EndDisconnect(result);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error running 'r.EndDisconnect(result);' in  cbDisconnectByHostComplete(IAsyncResult result): {0}", ex.Message + Environment.NewLine + ex.InnerException?.Message);
      }
      if (!AutoReconnect)
        return;
      Connect();
    }

    private void cbDisconnectComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is Socket asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      asyncState.EndDisconnect(result);
      ConnectionState = ConnectionStatus.DisconnectedByUser;
    }

    private void cbConnect(IAsyncResult result)
    {
      var asyncState = result.AsyncState as Socket;
      if (result == null)
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      if (!asyncState.Connected)
      {
        if (!AutoReconnect)
          return;
        Thread.Sleep(ReconnectInterval);
        Connect();
      }
      else
      {
        try
        {
          asyncState.EndConnect(result);
        }
        catch (Exception ex)
        {
          Console.WriteLine("Error encountered while executing Socket.EndConnect: " + ex.Message + ex.InnerException?.Message + ex.InnerException?.InnerException?.Message);
        }
        cbConnectComplete();
      }
    }

    private void cbSendComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is Socket asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      var errorCode = SocketError.Success;
      asyncState.EndSend(result, out errorCode);
      if (errorCode != 0)
      {
        DisconnectByHost();
      }
      else
      {
        lock (SyncLock)
          tmrSendTimeout.Stop();
      }
    }

    private void cbChangeConnectionStateComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is EventDrivenTCPClient asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a EDTC object");
      asyncState.ConnectionStatusChanged.EndInvoke(result);
    }

    public void SendMessage(string message)
    {
      if (!message.EndsWith("\r"))
        message += "\r";
      Console.WriteLine(GetType().Name + ".SendMessage(): " + message);
      var bytes = Encoding.ASCII.GetBytes(message);
      try
      {
        Send(bytes);
        if (message.Contains("StatusReq"))
          Log.Debug(GetType().Name, nameof (SendMessage), "SendToContoller", "TX: {0}", new object[1]
          {
            message
          });
        else
          Log.Info(GetType().Name, nameof (SendMessage), "SendToContoller", "TX: {0}", new object[1]
          {
            message
          });
      }
      catch (InvalidOperationException ex)
      {
        Console.WriteLine("Sending Error: " + ex.Message);
        Connect();
      }
    }

    private void StartListening()
    {
      var settings = new XmlReaderSettings()
      {
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreComments = true,
        IgnoreProcessingInstructions = true,
        IgnoreWhitespace = true
      };
      XmlReader xmlReader;
      using (xmlReader = XmlReader.Create(_client.GetStream(), settings))
      {
        while (true)
        {
          try
          {
            var content = (int) xmlReader.MoveToContent();
            if (!string.IsNullOrWhiteSpace(xmlReader.Name) && xmlReader.Name != "CCP")
            {
              xmlReader.ReadToFollowing("CCP");
              Thread.Sleep(1);
              continue;
            }
            if (!(xmlReader.Name == "CCP"))
              break;
            var xdocument = XDocument.Load(xmlReader.ReadOuterXml().ToStream());
            var args = xdocument.ToString();
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]|TCPXMLListener|Data Received|" + args);
            if (xdocument.Root.Attribute("MsgID").Value != "10")
              SendMessage("<CCP MsgID=\"10\" MsgName=\"ACK\" SeqNo=\"" + xdocument.Root.Attribute("SeqNo").Value + "\"><body /></CCP>");
            XMLDataReceived.AsyncSafeInvoke(this, args);
          }
          catch (Exception ex)
          {
          }
          Thread.Sleep(1);
        }
      }
    }

    public event EventHandler<string> XMLDataReceived;

    public delegate void delDataReceived(EventDrivenTCPClient sender, object data);

    public delegate void delConnectionStatusChanged(
      EventDrivenTCPClient sender,
      ConnectionStatus status);

    public enum ConnectionStatus
    {
      NeverConnected,
      Connecting,
      Connected,
      AutoReconnecting,
      DisconnectedByUser,
      DisconnectedByHost,
      ConnectFail_Timeout,
      ReceiveFail_Timeout,
      SendFail_Timeout,
      SendFail_NotConnected,
      Error,
    }
  }
}
