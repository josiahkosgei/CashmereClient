//DropStatusBody
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class DropStatusBody
  {
    [XmlElement(ElementName = "Status")]
    public string Status { get; set; }

    [XmlElement(ElementName = "TotalValue")]
    public long TotalValue { get; set; }

    [XmlElement(ElementName = "TotalCount")]
    public int TotalCount { get; set; }

    [XmlElement(ElementName = "NoteCounts")]
    public NoteCounts NoteCounts { get; set; }

    [XmlElement(ElementName = "OutstandingBalance")]
    public long OutstandingBalance { get; set; }
  }
}
