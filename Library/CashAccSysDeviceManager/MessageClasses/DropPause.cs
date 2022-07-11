
// DropPause


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class DropPause : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (DropPause));

    [XmlElement(ElementName = "body")]
    public DropPauseBody Body { get; set; }

    public DropPause()
    {
    }

    public DropPause(int seqno)
    {
      MessageID = 122;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = new DropPauseBody() { Message = "" };
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
