
// PrintReport


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class PrintReport : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (PrintReport));

    [XmlElement(ElementName = "body")]
    public PrintReportBody Body { get; set; }

    public PrintReport(int seqno)
    {
      MessageID = 104;
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
