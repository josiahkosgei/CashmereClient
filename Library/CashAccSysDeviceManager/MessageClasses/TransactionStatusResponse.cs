
// TransactionStatusResponse


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class TransactionStatusResponse : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (TransactionStatusResponse));

    [XmlElement(ElementName = "body")]
    public Body Body { get; set; }

    public TransactionStatusResponse()
    {
    }

    public TransactionStatusResponse(int seqno)
    {
      MessageID = 303;
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
