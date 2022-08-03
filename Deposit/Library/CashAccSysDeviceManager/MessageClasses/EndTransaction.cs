//EndTransaction
using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class EndTransaction : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (EndTransaction));

    [XmlElement(ElementName = "body")]
    public string Body { get; set; } = "";

    public EndTransaction()
    {
    }

    public EndTransaction(int seqno)
    {
      MessageID = 301;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => EndTransaction._serializer;
  }
}
