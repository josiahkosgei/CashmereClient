using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Utilities;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Linq;

namespace DeviceManager
{
  public class EventDrivenTCPClient : IDisposable
  {
    private const int DEFAULTTIMEOUT = 5000;
    private const int RECONNECTINTERVAL = 2000;
    private string _host = "";
    private EventDrivenTCPClient.ConnectionStatus _ConStat;
    private TcpClient _client;
    private byte[] dataBuffer = new byte[3000];
    private bool _AutoReconnect = true;
    private int _Port;
    private Encoding _encode = Encoding.Default;
    private object _SyncLock = new object();
    private System.Timers.Timer tmrReceiveTimeout = new System.Timers.Timer();
    private System.Timers.Timer tmrSendTimeout = new System.Timers.Timer();
    private System.Timers.Timer tmrConnectTimeout = new System.Timers.Timer();

    protected CashmereLogger Log { get; set; }

    public object SyncLock => this._SyncLock;

    public Encoding DataEncoding
    {
      get => this._encode;
      set => this._encode = value;
    }

    public EventDrivenTCPClient.ConnectionStatus ConnectionState
    {
      get => this._ConStat;
      private set
      {
        if (this._ConStat != value)
          Console.WriteLine("Changing Connection State from {0} to {1}", (object) Enum.GetName(typeof (EventDrivenTCPClient.ConnectionStatus), (object) this._ConStat), (object) Enum.GetName(typeof (EventDrivenTCPClient.ConnectionStatus), (object) value));
        bool flag = value != this._ConStat;
        this._ConStat = value;
        if (!(this.ConnectionStatusChanged != null & flag))
          return;
      //  this.ConnectionStatusChanged.BeginInvoke(this, this._ConStat, new AsyncCallback(this.cbChangeConnectionStateComplete), (object) this);

      }
    }

    public bool AutoReconnect
    {
      get => this._AutoReconnect;
      set => this._AutoReconnect = value;
    }

    public int ReconnectInterval { get; set; }

    public string Host => this._host;

    public int Port => this._Port;

    public int ReceiveTimeout
    {
      get => (int) this.tmrReceiveTimeout.Interval;
      set => this.tmrReceiveTimeout.Interval = (double) value;
    }

    public int SendTimeout
    {
      get => (int) this.tmrSendTimeout.Interval;
      set => this.tmrSendTimeout.Interval = (double) value;
    }

    public int ConnectTimeout
    {
      get => (int) this.tmrConnectTimeout.Interval;
      set => this.tmrConnectTimeout.Interval = (double) value;
    }

    public event EventDrivenTCPClient.delConnectionStatusChanged ConnectionStatusChanged;

    public EventDrivenTCPClient(CashmereLogger log, string host, int port, bool autoreconnect = true)
    {
      this.Log = log;
      this._host = host;
      this._Port = port;
      this._AutoReconnect = autoreconnect;
      this._client = new TcpClient(AddressFamily.InterNetwork);
      this._client.NoDelay = true;
      this.ReceiveTimeout = 5000;
      this.SendTimeout = 5000;
      this.ConnectTimeout = 5000;
      this.ReconnectInterval = 2000;
      this.tmrReceiveTimeout.AutoReset = false;
      this.tmrReceiveTimeout.Elapsed += new ElapsedEventHandler(this.tmrReceiveTimeout_Elapsed);
      this.tmrConnectTimeout.AutoReset = false;
      this.tmrConnectTimeout.Elapsed += new ElapsedEventHandler(this.tmrConnectTimeout_Elapsed);
      this.tmrSendTimeout.AutoReset = false;
      this.tmrSendTimeout.Elapsed += new ElapsedEventHandler(this.tmrSendTimeout_Elapsed);
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.NeverConnected;
    }

    private void tmrSendTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.SendFail_Timeout;
      this.DisconnectByHost();
    }

    private void tmrReceiveTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.ReceiveFail_Timeout;
      this.DisconnectByHost();
    }

    private void tmrConnectTimeout_Elapsed(object sender, ElapsedEventArgs e)
    {
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.ConnectFail_Timeout;
      this.DisconnectByHost();
    }

    private void DisconnectByHost()
    {
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.DisconnectedByHost;
      this.tmrReceiveTimeout.Stop();
      if (!this.AutoReconnect)
        return;
      this.Reconnect();
    }

    private void Reconnect()
    {
      if (this.ConnectionState == EventDrivenTCPClient.ConnectionStatus.Connected)
        return;
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.AutoReconnecting;
      try
      {
        this._client.Client.BeginDisconnect(true, new AsyncCallback(this.cbDisconnectByHostComplete), (object) this._client.Client);
      }
      catch
      {
      }
    }

    public void Connect()
    {
      if (this.ConnectionState == EventDrivenTCPClient.ConnectionStatus.Connected)
        return;
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.Connecting;
      this.tmrConnectTimeout.Start();
      try
      {
        this._client.BeginConnect(this._host, this._Port, new AsyncCallback(this.cbConnect), (object) this._client.Client);
      }
      catch (Exception ex)
      {
      }
    }

    public void Disconnect()
    {
      if (this.ConnectionState != EventDrivenTCPClient.ConnectionStatus.Connected)
        return;
      this._client.Client.BeginDisconnect(true, new AsyncCallback(this.cbDisconnectComplete), (object) this._client.Client);
    }

    public void Send(string data)
    {
      if (this.ConnectionState != EventDrivenTCPClient.ConnectionStatus.Connected)
      {
        this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.SendFail_NotConnected;
      }
      else
      {
        byte[] bytes = this._encode.GetBytes(data);
        SocketError errorCode = SocketError.Success;
        this.tmrSendTimeout.Start();
        this._client.Client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, out errorCode, new AsyncCallback(this.cbSendComplete), (object) this._client.Client);
        if (errorCode == SocketError.Success)
          return;
        new Action(this.DisconnectByHost)();
      }
    }

    public void Send(byte[] data)
    {
      if (this.ConnectionState != EventDrivenTCPClient.ConnectionStatus.Connected)
        throw new InvalidOperationException("Cannot send data, socket is not connected");
      SocketError errorCode = SocketError.Success;
      this._client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, out errorCode, new AsyncCallback(this.cbSendComplete), (object) this._client.Client);
      if (errorCode == SocketError.Success)
        return;
      new Action(this.DisconnectByHost)();
    }

    public void Dispose()
    {
      this._client.Close();
      this._client.Client.Dispose();
    }

    private void cbConnectComplete()
    {
      if (this._client.Connected)
      {
        this.tmrConnectTimeout.Stop();
        this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.Connected;
        try
        {
          Task.Run((Action) (() => this.StartListening()));
        }
        catch (Exception ex)
        {
          this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.Error;
        }
      }
      else
        this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.Error;
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
        Console.WriteLine("Error running 'r.EndDisconnect(result);' in  cbDisconnectByHostComplete(IAsyncResult result): {0}", (object) (ex.Message + Environment.NewLine + ex.InnerException?.Message));
      }
      if (!this.AutoReconnect)
        return;
      new Action(this.Connect)();
    }

    private void cbDisconnectComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is Socket asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      asyncState.EndDisconnect(result);
      this.ConnectionState = EventDrivenTCPClient.ConnectionStatus.DisconnectedByUser;
    }

    private void cbConnect(IAsyncResult result)
    {
      Socket asyncState = result.AsyncState as Socket;
      if (result == null)
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      if (!asyncState.Connected)
      {
        if (!this.AutoReconnect)
          return;
        Thread.Sleep(this.ReconnectInterval);
        new Action(this.Connect)();
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
        new Action(this.cbConnectComplete)();
      }
    }

    private void cbSendComplete(IAsyncResult result)
    {
      if (!(result.AsyncState is Socket asyncState))
        throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
      SocketError errorCode = SocketError.Success;
      asyncState.EndSend(result, out errorCode);
      if (errorCode != SocketError.Success)
      {
        new Action(this.DisconnectByHost)();
      }
      else
      {
        lock (this.SyncLock)
          this.tmrSendTimeout.Stop();
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
      Console.WriteLine(this.GetType().Name + ".SendMessage(): " + message);
      byte[] bytes = Encoding.ASCII.GetBytes(message);
      try
      {
        this.Send(bytes);
        if (message.Contains("StatusReq"))
          this.Log.Debug(this.GetType().Name, nameof (SendMessage), "SendToContoller", "TX: {0}", new object[1]
          {
            (object) message
          });
        else
          this.Log.Info(this.GetType().Name, nameof (SendMessage), "SendToContoller", "TX: {0}", new object[1]
          {
            (object) message
          });
      }
      catch (InvalidOperationException ex)
      {
        Console.WriteLine("Sending Error: " + ex.Message);
        this.Connect();
      }
    }

    private void StartListening()
    {
      XmlReaderSettings settings = new XmlReaderSettings()
      {
        ConformanceLevel = ConformanceLevel.Fragment,
        IgnoreComments = true,
        IgnoreProcessingInstructions = true,
        IgnoreWhitespace = true
      };
      XmlReader xmlReader;
      using (xmlReader = XmlReader.Create((Stream) this._client.GetStream(), settings))
      {
        while (true)
        {
          try
          {
            int content = (int) xmlReader.MoveToContent();
            if (!(xmlReader.Name == "CCP"))
              break;
            XDocument xdocument = XDocument.Load(xmlReader.ReadOuterXml().ToStream());
            string args = xdocument.ToString();
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]|TCPXMLListener|Data Received|" + args);
            if (xdocument.Root.Attribute((XName) "MsgID").Value != "10")
              this.SendMessage("<CCP MsgID=\"10\" MsgName=\"ACK\" SeqNo=\"" + xdocument.Root.Attribute((XName) "SeqNo").Value + "\"><body /></CCP>");
            this.XMLDataReceived.AsyncSafeInvoke((object) this, args);
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
      EventDrivenTCPClient.ConnectionStatus status);

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
