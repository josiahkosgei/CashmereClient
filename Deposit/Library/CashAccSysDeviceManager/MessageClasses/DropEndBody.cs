//DropEndBody
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class DropEndBody
  {
    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }

    [XmlElement(ElementName = "Escrow")]
    public string Escrow { get; set; }
  }
}
