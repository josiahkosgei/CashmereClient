
// PrintText


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class PrintText : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (PrintText));

    [XmlElement(ElementName = "body")]
    public PrintTextBody Body { get; set; }

    public PrintText(int seqno)
    {
      MessageID = 106;
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
