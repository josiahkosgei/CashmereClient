
// Sensors


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Sensors")]
  public class Sensors
  {
    [XmlAttribute(AttributeName = "Type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "Status")]
    public string Status { get; set; }

    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }

    [XmlAttribute(AttributeName = "Door")]
    public string Door { get; set; }

    [XmlAttribute(AttributeName = "Bag")]
    public string Bag { get; set; }
  }
}
