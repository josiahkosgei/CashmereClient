
// AuthoriseResponse


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class AuthoriseResponse : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (AuthoriseResponse));

    [XmlElement(ElementName = "body")]
    public AuthoriseResponseBody Body { get; set; }

    public AuthoriseResponse()
    {
    }

    public AuthoriseResponse(int seqno)
    {
      MessageID = 101;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
