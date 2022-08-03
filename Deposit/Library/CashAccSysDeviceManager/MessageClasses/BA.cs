//BA
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "BA")]
  public class BA
  {
    [XmlAttribute(AttributeName = "Type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "Status")]
    public string Status { get; set; }

    [XmlAttribute(AttributeName = "Currency")]
    public string Currency { get; set; }
  }
}
