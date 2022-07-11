
// AuthoriseRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class AuthoriseRequest : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (AuthoriseRequest));

    [XmlElement(ElementName = "body")]
    public AuthoriseRequestBody Body { get; set; }

    public AuthoriseRequest()
    {
    }

    public AuthoriseRequest(int seqno, int clientID, string macAddress, string clientType)
    {
      MessageID = 100;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = new AuthoriseRequestBody()
      {
        ClientID = clientID,
        MACAddress = macAddress,
        ClientType = clientType
      };
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
