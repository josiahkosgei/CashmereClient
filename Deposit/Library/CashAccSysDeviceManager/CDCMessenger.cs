using CashAccSysDeviceManager.MessageClasses;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using DeviceManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager
{
  public class CDCMessenger
  {
    private string _host = "CashAccSys3";
    private int _port = 20201;
    private string _macAddress = "74:D0:2B:72:11:37";
    private int _clientID = 1122;
    private string _clientType = "UI";
    private int _sequenceNumber = 1;
    private int _controllerSequenceNumber;
    private DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send);
    private DispatcherTimer statusTimer = new DispatcherTimer(DispatcherPriority.Send);
    private EventDrivenTCPClient client;
    public bool isLoginSuccess;
    private string lastIncompleteMessage = "";

    protected CashmereLogger Log { get; set; }

    private SortedList<int, string> OutGoingMessageQueue { get; set; } = new SortedList<int, string>();

    public CDCMessenger(
      string host,
      int port,
      string macAddress,
      int clientID,
      string clientType,
      int messagSendInterval)
    {
      Log = new CashmereLogger(Assembly.GetAssembly(typeof (CashAccSysDeviceManager)).GetName().Version.ToString(), "DeviceMessengerLog", null);
      Log.Debug(GetType().Name, "Constructor", "Initialisation", "Creating CDCMessenger", Array.Empty<object>());
      _host = host;
      _port = port;
      _macAddress = macAddress;
      _clientID = clientID;
      _clientType = clientType;
      dispTimer.Interval = TimeSpan.FromSeconds(messagSendInterval);
      dispTimer.Tick += new EventHandler(dispTimer_Tick);
      dispTimer.IsEnabled = true;
      statusTimer.Interval = TimeSpan.FromSeconds(2.0);
      statusTimer.Tick += new EventHandler(statusTimer_Tick);
      statusTimer.IsEnabled = true;
      client = new EventDrivenTCPClient(Log, _host, _port);
      client.XMLDataReceived += new EventHandler<string>(Client_DataReceived);
      client.ConnectionStatusChanged += new EventDrivenTCPClient.delConnectionStatusChanged(Client_ConnectionStatusChanged);
    }

    private void Client_ConnectionStatusChanged(
      EventDrivenTCPClient sender,
      EventDrivenTCPClient.ConnectionStatus status)
    {
      if (status == EventDrivenTCPClient.ConnectionStatus.Connected)
        ResetControllerSequenceNumber();
      OnTCPConnectionEvent(this, new StringResult()
      {
        data = status.ToString("G")
      });
    }

    private void ResetControllerSequenceNumber()
    {
      Log.Info(GetType().Name, "Client_ConnectionStatusChanged", "EventHandler", "Reset _controllerSequenceNumber on connection established", Array.Empty<object>());
      _controllerSequenceNumber = 0;
    }

    private void statusTimer_Tick(object sender, EventArgs e)
    {
      if (OutGoingMessageQueue.Count > 0)
        return;
      if (!isLoginSuccess)
        Connect();
      else
        GetStatus();
    }

    private void dispTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (OutGoingMessageQueue.Count <= 0)
          return;
        string message = OutGoingMessageQueue.FirstOrDefault<KeyValuePair<int, string>>().Value;
        if (message == null)
          return;
        client.SendMessage(message);
        if (message.Contains("StatusReq"))
          Log.Debug(GetType().Name, "ReSendMessage", "Sending", "TX: {0}", new object[1]
          {
             message
          });
        else
          Log.Info(GetType().Name, "ReSendMessage", "Sending", "TX: {0}", new object[1]
          {
             message
          });
      }
      catch (Exception ex)
      {
      }
    }

    public void SendMessage(int seqno, string message)
    {
      message += "\r";
      int key = seqno;
      try
      {
        int sequenceNumber;
        for (; OutGoingMessageQueue.ContainsKey(key); key = sequenceNumber)
        {
          sequenceNumber = _sequenceNumber;
          Log.Debug(GetType().Name, nameof (SendMessage), "Sending", "change message and increment sequence number from {0} to {1}", new object[2]
          {
             key,
             sequenceNumber
          });
          message = message.Replace(string.Format("SeqNo=\"{0}\"", key), string.Format("SeqNo=\"{0}\"", sequenceNumber));
        }
        if (message.Contains("StatusReq"))
          Log.Debug(GetType().Name, nameof (SendMessage), "Sending", "TX: {0}", new object[1]
          {
             message
          });
        else
          Log.Info(GetType().Name, nameof (SendMessage), "Sending", "TX: {0}", new object[1]
          {
             message
          });
        client.SendMessage(message);
        OutGoingMessageQueue.Add(key, message);
        ++_sequenceNumber;
      }
      catch (Exception ex)
      {
      }
    }

    public string SerializeToXML<T>(T message, XmlSerializer xmlSerializer)
    {
      string empty = string.Empty;
      try
      {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = false;
        settings.OmitXmlDeclaration = true;
        settings.NewLineChars = string.Empty;
        settings.NewLineHandling = NewLineHandling.None;
        using (StringWriter output = new StringWriter())
        {
          using (XmlWriter xmlWriter = XmlWriter.Create(output, settings))
          {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            xmlSerializer.Serialize(xmlWriter, message, namespaces);
            empty = output.ToString();
            xmlWriter.Close();
          }
          output.Close();
        }
      }
      catch (Exception ex)
      {
      }
      return empty;
    }

    public T DeserializeFromXML<T>(string message, XmlSerializer xmlSerializer)
    {
      try
      {
        using (StringReader stringReader = new StringReader(message))
          return (T) xmlSerializer.Deserialize(stringReader);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public event EventHandler<AuthoriseResponse> ConnectionEvent;

    private void OnConnectionEvent(object sender, AuthoriseResponse e)
    {
      if (OutGoingMessageQueue.Count > 0)
      {
        SortedList<int, string> goingMessageQueue = OutGoingMessageQueue;
        int seqNo = _sequenceNumber;
        int leastIndex = OutGoingMessageQueue.FirstOrDefault<KeyValuePair<int, string>>().Key;
        OutGoingMessageQueue = new SortedList<int, string>(OutGoingMessageQueue.Where<KeyValuePair<int, string>>(x => !x.Value.Contains("MsgID=\"102\" MsgName=\"StatusReq\"")).Select<KeyValuePair<int, string>, KeyValuePair<int, string>>(message => new KeyValuePair<int, string>(message.Key - leastIndex + seqNo + 11, message.Value.Replace(string.Format("SeqNo=\"{0}\"", message.Key), string.Format("SeqNo=\"{0}\"", message.Key - leastIndex + seqNo + 11)))).ToDictionary<KeyValuePair<int, string>, int, string>(newmessage => newmessage.Key, newmessage => newmessage.Value));
        SendMessage(seqNo + 10, SerializeToXML<StatusRequest>(new StatusRequest(seqNo + 10), StatusRequest.Serializer));
        _sequenceNumber = _sequenceNumber + OutGoingMessageQueue.Count + 11;
      }
      if (ConnectionEvent == null)
        return;
      ConnectionEvent(this, e);
    }

    public event EventHandler<StringResult> TCPConnectionEvent;

    private void OnTCPConnectionEvent(object sender, StringResult e)
    {
      if (TCPConnectionEvent == null)
        return;
      TCPConnectionEvent(this, e);
    }

    public event EventHandler<DropStatus> DropStatusResultEvent;

    private void OnDropStatusResultEvent(object sender, DropStatus e)
    {
      if (DropStatusResultEvent == null)
        return;
      DropStatusResultEvent(this, e);
    }

    public event EventHandler<StatusReport> StatusReportEvent;

    private void OnStatusReportEvent(object sender, StatusReport e)
    {
      if (StatusReportEvent == null)
        return;
      StatusReportEvent(this, e);
    }

    public event EventHandler<TransactionStatusResponse> TransactionStatusEvent;

    private void OnTransactionStatusEvent(object sender, TransactionStatusResponse e)
    {
      if (TransactionStatusEvent == null)
        return;
      TransactionStatusEvent(this, e);
    }

    public event EventHandler<DropResult> DropResultEvent;

    private void OnDropResultEvent(object sender, DropResult e)
    {
      if (DropResultEvent == null)
        return;
      DropResultEvent(this, e);
    }

    public event EventHandler<CITResult> CITResultEvent;

    private void OnCITResultEvent(object sender, CITResult e)
    {
      if (CITResultEvent == null)
        return;
      CITResultEvent(this, e);
    }

    public event EventHandler<string> ComLogEvent;

    private void OnComLogEvent(object sender, string e)
    {
      if (ComLogEvent == null)
        return;
      ComLogEvent(this, e);
    }

    private List<string> ProcessReceivedMessages(string xmlData) => xmlData.Split(new string[2]
    {
      "\r\n\r\n",
      "\n\n"
    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

    private void Client_DataReceived(object sender, string data)
    {
      if (data.Contains("=\"StatusReport"))
        Log.Debug(GetType().Name, nameof (Client_DataReceived), "Receiving", "Rx: {0}", new object[1]
        {
           data
        });
      else
        Log.Info(GetType().Name, nameof (Client_DataReceived), "Receiving", "Rx: {0}", new object[1]
        {
           data
        });
      List<string> stringList = ProcessReceivedMessages(lastIncompleteMessage + data);
      lastIncompleteMessage = "";
      Console.WriteLine(string.Format("Found {0} messages", stringList.Count));
      for (int index = 0; index < stringList.Count; ++index)
      {
        Console.WriteLine(string.Format("[{0}] Processing message {1} of {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), index + 1, stringList.Count));
        string str = stringList[index];
        Match match = Regex.Match(str, "(\"\\w+\")+");
        MessageType messageType = (MessageType) int.Parse(match.Value.Replace('"', ' '));
        int num = int.Parse(match.NextMatch().NextMatch().Value.Replace('"', ' '));
        try
        {
          Log.Info(GetType().Name, nameof (Client_DataReceived), "EventHandler", "Message received. messageSeqNo={0} _controllerSequenceNumber={1}", new object[2]
          {
             num,
             _controllerSequenceNumber
          });
          if (messageType == MessageType.ACK)
          {
            OutGoingMessageQueue.Remove(DeserializeFromXML<ACK>(str, ACK.Serializer).SequenceNumber);
          }
          else
          {
            if (_controllerSequenceNumber == 199999 && num < 100010)
              ResetControllerSequenceNumber();
            if (num <= _controllerSequenceNumber)
            {
              Log.Warning(GetType().Name, nameof (Client_DataReceived), "EventHandler", "Old massage found messageSeqNo={0} _controllerSequenceNumber={1}", new object[2]
              {
                 num,
                 _controllerSequenceNumber
              });
            }
            else
            {
              _controllerSequenceNumber = num;
              switch (messageType)
              {
                case MessageType.ReqErr:
                  if (DeserializeFromXML<RequestError>(str, RequestError.Serializer).Body.Error == "Not Authorised")
                  {
                    Connect();
                    continue;
                  }
                  continue;
                case MessageType.AuthoriseResp:
                  AuthoriseResponse e = DeserializeFromXML<AuthoriseResponse>(str, AuthoriseResponse.Serializer);
                  isLoginSuccess = !(e.Body.Result != "ACCEPTED");
                  OnConnectionEvent(this, e);
                  return;
                default:
                  processResponse(str, messageType);
                  continue;
              }
            }
          }
        }
        catch (InvalidOperationException ex)
        {
          Console.WriteLine("Error Receiving data: {0}", ex.Message);
          if (index == stringList.Count - 1)
            lastIncompleteMessage = str;
        }
      }
    }

    public void processResponse(string strData, MessageType messageType)
    {
      switch (messageType)
      {
        case MessageType.StatusReport:
          OnStatusReportEvent(this, DeserializeFromXML<StatusReport>(strData, StatusReport.Serializer));
          break;
        case MessageType.DropStatus:
          OnDropStatusResultEvent(this, DeserializeFromXML<DropStatus>(strData, DropStatus.Serializer));
          break;
        case MessageType.DropResult:
          OnDropResultEvent(this, DeserializeFromXML<DropResult>(strData, DropResult.Serializer));
          break;
        case MessageType.BagRemovalReport:
          BagRemovalReport bagRemovalReport = DeserializeFromXML<BagRemovalReport>(strData, BagRemovalReport.Serializer);
          List<DenominationItem> denominationItemList = new List<DenominationItem>();
          foreach (BagRemovalReportNoteCount noteCount in bagRemovalReport.Body.Summary.NoteCounts)
            denominationItemList.Add(new DenominationItem()
            {
              count = noteCount.Count,
              Currency = noteCount.Currency,
              type = DenominationItemType.NOTE,
              denominationValue = noteCount.Denomination
            });
          CITResult e = new CITResult();
          e.level = ErrorLevel.SUCCESS;
          e.resultCode = 0;
          e.data = new CITResultBody()
          {
            BagNumber = bagRemovalReport.Body.Summary.Information.BagNumber,
            Currency = bagRemovalReport.Body.Summary.Information.Currency,
            DateTime = bagRemovalReport.Body.Summary.Information.DateTime,
            DeviceSerialNumber = bagRemovalReport.Body.Summary.Information.DeviceSerialNumber,
            Name = bagRemovalReport.Body.Summary.Information.Name,
            TotalValue = bagRemovalReport.Body.Summary.Information.TotalValue,
            TransactionCount = bagRemovalReport.Body.Summary.Information.TransactionCount,
            denomination = new Denomination()
            {
              DenominationItems = denominationItemList
            }
          };
          OnCITResultEvent(this, e);
          break;
        case MessageType.TransactionStatusResponse:
          OnTransactionStatusEvent(this, DeserializeFromXML<TransactionStatusResponse>(strData, TransactionStatusResponse.Serializer));
          break;
      }
    }

    internal void ShowDeviceController()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<ShowController>(new ShowController(sequenceNumber), ShowController.Serializer));
    }

    public void Connect()
    {
      string xml = SerializeToXML<AuthoriseRequest>(new AuthoriseRequest(_sequenceNumber, _clientID, _macAddress, _clientType), AuthoriseRequest.Serializer);
      ++_sequenceNumber;
      client.SendMessage(xml);
      Console.WriteLine(">OUT> " + xml);
    }

    internal void ResetDevice() => SendMessage(_sequenceNumber, SerializeToXML<CancelCurrentProcess>(new CancelCurrentProcess(_sequenceNumber), CancelCurrentProcess.Serializer));

    internal void GetStatus()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<StatusRequest>(new StatusRequest(sequenceNumber), StatusRequest.Serializer));
    }

    internal void ClearOutgoingMessageQueue() => OutGoingMessageQueue.Clear();

    internal void SetCurrency(string currency) => SendMessage(_sequenceNumber, SerializeToXML<SelectCurrency>(new SelectCurrency(_sequenceNumber, currency), SelectCurrency.Serializer));

    internal void CashInStart(
      string currency,
      string userID,
      string sessionID,
      string transactionID,
      DropMode dropMode = DropMode.DROP_NOTES)
    {
      Currency = currency;
      UserID = userID;
      SessionID = sessionID;
      TransactionID = transactionID;
      DropMode = dropMode;
    }

    public string Currency { get; set; }

    public string UserID { get; set; }

    public string SessionID { get; set; }

    public string TransactionID { get; set; }

    public DropMode DropMode { get; set; }

    internal void Count(string DropID, long transactionValue = 0)
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<DropRequest>(new DropRequest(sequenceNumber, Currency, UserID, dropType: DropMode.ToCashAccSysString(), reference: TransactionID, inputSubNumber: DropID, inputNumber: SessionID), DropRequest.Serializer));
    }

    internal void EndCount()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<DropEnd>(new DropEnd(sequenceNumber, "DONE"), DropEnd.Serializer));
    }

    internal void EscrowDrop()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<DropEnd>(new DropEnd(sequenceNumber, "ESCROW_DROP"), DropEnd.Serializer));
    }

    internal void EscrowReject()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<DropEnd>(new DropEnd(sequenceNumber, "ESCROW_REJECT"), DropEnd.Serializer));
    }

    internal void RequestPause()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<DropPause>(new DropPause(sequenceNumber), DropPause.Serializer));
    }

    internal void RequestManualDrop() => throw new NotImplementedException();

    public void TransactionEnd()
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<EndTransaction>(new EndTransaction(sequenceNumber), EndTransaction.Serializer));
    }

    internal void EndCIT(string bagNumber)
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<OpenBagRequest>(new OpenBagRequest(sequenceNumber, bagNumber), OpenBagRequest.Serializer));
    }

    internal void StartCIT(string sealNumber)
    {
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<CloseBagRequest>(new CloseBagRequest(sequenceNumber, sealNumber, CloseBagRequestAction.SUMMARY_REPORT), CloseBagRequest.Serializer));
    }

    public void GetTransactionStatus()
    {
      IEnumerable<bool> source = OutGoingMessageQueue.Select<KeyValuePair<int, string>, bool>(x =>
      {
          string str = x.Value;
          return str != null && str.TrimStart().StartsWith("<CCP MsgID=\"302\"");
      });
      int num1;
      if (source == null)
      {
        num1 = 0;
      }
      else
      {
        int? count = source.ToList<bool>()?.Count;
        int num2 = 0;
        num1 = count.GetValueOrDefault() <= num2 & count.HasValue ? 1 : 0;
      }
      if (num1 == 0)
        return;
      int sequenceNumber = _sequenceNumber;
      SendMessage(sequenceNumber, SerializeToXML<TransactionStatusRequest>(new TransactionStatusRequest(sequenceNumber), TransactionStatusRequest.Serializer));
    }
  }
}
