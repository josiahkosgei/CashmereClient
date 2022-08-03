//Escrow
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Escrow")]
  public class Escrow
  {
    [XmlAttribute(AttributeName = "Type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "Status")]
    public string Status { get; set; }

    [XmlAttribute(AttributeName = "Position")]
    public string Position { get; set; }
  }
}
