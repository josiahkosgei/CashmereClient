
// CloseBagRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class CloseBagRequest : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (CloseBagRequest));

    [XmlElement(ElementName = "body")]
    public CloseBagRequestBody Body { get; set; }

    public CloseBagRequest()
    {
    }

    public CloseBagRequest(int seqno, string containerNumber, CloseBagRequestAction action = CloseBagRequestAction.DETAIL_REPORT)
    {
      MessageID = 135;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = new CloseBagRequestBody()
      {
        ContainerNumber = containerNumber,
        Action = Enum.GetName(typeof (CloseBagRequestAction), action)
      };
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
