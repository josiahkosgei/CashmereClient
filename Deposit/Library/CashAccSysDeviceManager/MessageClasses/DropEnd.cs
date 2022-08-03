//DropEnd
using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class DropEnd : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (DropEnd));

    [XmlElement(ElementName = "body")]
    public DropEndBody Body { get; set; }

    public DropEnd()
    {
    }

    public DropEnd(int seqno, string escrowAction, string message = "")
    {
      MessageID = 123;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = new DropEndBody()
      {
        Escrow = escrowAction,
        Message = message
      };
    }

    internal new static XmlSerializer Serializer => DropEnd._serializer;
  }
}
