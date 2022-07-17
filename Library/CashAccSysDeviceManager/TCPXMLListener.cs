
//TCPXMLListener


using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace CashAccSysDeviceManager
{
  public class TCPXMLListener
  {
      private TcpClient _client;
    private StringBuilder Message;
    private XDocument doc;
    private XmlReader reader;

    public string Host { get; }

    public int Port { get; }

    public TCPXMLListener(string host, int port)
    {
      Host = host;
      Port = port;
      _client = new TcpClient(host, port);
      Task.Run(() => StartListening());
    }

    public void SendMessage(string message)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(message);
      try
      {
        Send(bytes);
        Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]|" + GetType().Name + "|SendMessage|Sent|" + message + "|0");
      }
      catch (InvalidOperationException ex)
      {
        Console.WriteLine("Sending Error: " + ex.Message);
      }
    }

    public void Send(byte[] data)
    {
      SocketError errorCode = SocketError.Success;
      _client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, out errorCode, null, _client.Client);
    }

    private void StartListening()
    {
      XmlReaderSettings settings = new XmlReaderSettings()
      {
        ConformanceLevel = ConformanceLevel.Fragment
      };
      using (reader = XmlReader.Create(_client.GetStream(), settings))
      {
        while (true)
        {
          do
          {
            int content = (int) reader.MoveToContent();
          }
          while (string.IsNullOrWhiteSpace(reader.Name));
          try
          {
            doc = XDocument.Load(reader.ReadSubtree());
            Console.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]|TCPXMLListener|Data Received|" + doc.ToString());
            if (doc.Root.Attribute("MsgID").Value != "10")
              SendMessage("<?xml version=\"1.0\" encoding=\"UTF-8\"?><CCP MsgID=\"10\" MsgName=\"ACK\" SeqNo=\"" + doc.Root.Attribute("SeqNo").Value + "\"><body /></CCP>");
            DataReceived.AsyncSafeInvoke(this, doc.ToString());
            reader.ReadEndElement();
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    public event EventHandler<string> DataReceived;
  }
}
