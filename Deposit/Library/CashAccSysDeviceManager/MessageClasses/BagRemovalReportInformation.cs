//BagRemovalReportInformation
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Information")]
  public class BagRemovalReportInformation
  {
    [XmlElement(ElementName = "BagNumber")]
    public string BagNumber { get; set; }

    [XmlElement(ElementName = "Currency")]
    public string Currency { get; set; }

    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }

    [XmlElement(ElementName = "DateTime")]
    public string DateTime { get; set; }

    [XmlElement(ElementName = "TransactionCount")]
    public int TransactionCount { get; set; }

    [XmlElement(ElementName = "TotalValue")]
    public long TotalValue { get; set; }

    [XmlElement(ElementName = "DeviceSerialNumber")]
    public string DeviceSerialNumber { get; set; }
  }
}
