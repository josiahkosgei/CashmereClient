
// BagRemovalReport


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class BagRemovalReport : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (BagRemovalReport));

    [XmlElement(ElementName = "body")]
    public BagRemovalReportBody Body { get; set; }

    internal new static XmlSerializer Serializer => _serializer;
  }
}
