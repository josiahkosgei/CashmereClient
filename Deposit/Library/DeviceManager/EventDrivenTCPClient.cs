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
        private ConnectionStatus _ConStat;
        private TcpClient _client;
        private byte[] dataBuffer = new byte[3000];
        private bool _AutoReconnect = true;
        private int _Port;
        private Encoding _encode = Encoding.Default;
        private object _SyncLock = new();
        private System.Timers.Timer tmrReceiveTimeout = new();
        private System.Timers.Timer tmrSendTimeout = new();
        private System.Timers.Timer tmrConnectTimeout = new();

        protected CashmereLogger Log { get; set; }

        public object SyncLock => _SyncLock;

        public Encoding DataEncoding
        {
            get => _encode;
            set => _encode = value;
        }

        public ConnectionStatus ConnectionState
        {
            get => _ConStat;
            private set
            {
                if (_ConStat != value)
                    Console.WriteLine("Changing Connection State from {0} to {1}", Enum.GetName(typeof(ConnectionStatus), _ConStat), Enum.GetName(typeof(ConnectionStatus), value));
                bool flag = value != _ConStat;
                _ConStat = value;
                if (!(ConnectionStatusChanged != null & flag))
                    return;

                var workTask = Task.Run(() => ConnectionStatusChanged.Invoke(this, _ConStat));
                var ret = workTask.ContinueWith(x => x).Result;
            }
        }

        public bool AutoReconnect
        {
            get => _AutoReconnect;
            set => _AutoReconnect = value;
        }

        public int ReconnectInterval { get; set; }

        public string Host => _host;

        public int Port => _Port;

        public int ReceiveTimeout
        {
            get => (int)tmrReceiveTimeout.Interval;
            set => tmrReceiveTimeout.Interval = value;
        }

        public int SendTimeout
        {
            get => (int)tmrSendTimeout.Interval;
            set => tmrSendTimeout.Interval = value;
        }

        public int ConnectTimeout
        {
            get => (int)tmrConnectTimeout.Interval;
            set => tmrConnectTimeout.Interval = value;
        }

        public event delConnectionStatusChanged ConnectionStatusChanged;

        public EventDrivenTCPClient(CashmereLogger log, string host, int port, bool autoreconnect = true)
        {
            Log = log;
            _host = host;
            _Port = port;
            _AutoReconnect = autoreconnect;
            _client = new TcpClient(AddressFamily.InterNetwork);
            _client.NoDelay = true;
            ReceiveTimeout = 5000;
            SendTimeout = 5000;
            ConnectTimeout = 5000;
            ReconnectInterval = 2000;
            tmrReceiveTimeout.AutoReset = false;
            tmrReceiveTimeout.Elapsed += new ElapsedEventHandler(tmrReceiveTimeout_Elapsed);
            tmrConnectTimeout.AutoReset = false;
            tmrConnectTimeout.Elapsed += new ElapsedEventHandler(tmrConnectTimeout_Elapsed);
            tmrSendTimeout.AutoReset = false;
            tmrSendTimeout.Elapsed += new ElapsedEventHandler(tmrSendTimeout_Elapsed);
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
                _client.Client.BeginDisconnect(true, new AsyncCallback(cbDisconnectByHostComplete), _client.Client);
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
                _client.BeginConnect(_host, _Port, new AsyncCallback(cbConnect), _client.Client);
            }
            catch (Exception ex)
            {
            }
        }

        public void Disconnect()
        {
            if (ConnectionState != ConnectionStatus.Connected)
                return;
            _client.Client.BeginDisconnect(true, new AsyncCallback(cbDisconnectComplete), _client.Client);
        }

        public void Send(string data)
        {
            if (ConnectionState != ConnectionStatus.Connected)
            {
                ConnectionState = ConnectionStatus.SendFail_NotConnected;
            }
            else
            {
                byte[] bytes = _encode.GetBytes(data);
                SocketError errorCode = SocketError.Success;
                tmrSendTimeout.Start();
                _client.Client.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, out errorCode, new AsyncCallback(cbSendComplete), _client.Client);
                if (errorCode == SocketError.Success)
                    return;
                new Action(DisconnectByHost)();
            }
        }

        public void Send(byte[] data)
        {
            if (ConnectionState != ConnectionStatus.Connected)
                throw new InvalidOperationException("Cannot send data, socket is not connected");
            SocketError errorCode = SocketError.Success;
            _client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, out errorCode, new AsyncCallback(cbSendComplete), _client.Client);
            if (errorCode == SocketError.Success)
                return;
            new Action(DisconnectByHost)();
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
            new Action(Connect)();
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
            Socket asyncState = (Socket)result.AsyncState;
            if (result == null)
                throw new InvalidOperationException("Invalid IAsyncResult - Could not interpret as a socket object");
            if (!asyncState.Connected)
            {
                if (!AutoReconnect)
                    return;
                Thread.Sleep(ReconnectInterval);
                new Action(Connect)();
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
                new Action(cbConnectComplete)();
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
                new Action(DisconnectByHost)();
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
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            try
            {
                Send(bytes);
                if (message.Contains("StatusReq"))
                    Log.Debug(GetType().Name, nameof(SendMessage), "SendToContoller", "TX: {0}", new object[1]
                    {
             message
                    });
                else
                    Log.Info(GetType().Name, nameof(SendMessage), "SendToContoller", "TX: {0}", new object[1]
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
            XmlReaderSettings settings = new()
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
                        int content = (int)xmlReader.MoveToContent();
                        if (!(xmlReader.Name == "CCP"))
                            break;
                        XDocument xdocument = XDocument.Load(xmlReader.ReadOuterXml().ToStream());
                        string args = xdocument.ToString();
                        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]|TCPXMLListener|Data Received|" + args);
                        if (xdocument.Root.Attribute((XName)"MsgID").Value != "10")
                            SendMessage("<CCP MsgID=\"10\" MsgName=\"ACK\" SeqNo=\"" + xdocument.Root.Attribute((XName)"SeqNo").Value + "\"><body /></CCP>");
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
