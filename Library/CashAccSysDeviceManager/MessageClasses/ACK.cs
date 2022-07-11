
// ACK


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class ACK : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (ACK), new XmlRootAttribute()
    {
      ElementName = "CCP",
      IsNullable = true
    });

    [XmlElement(ElementName = "body")]
    public string Body { get; set; }

    public ACK()
    {
    }

    public ACK(int seqno)
    {
      MessageID = 10;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = "";
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
