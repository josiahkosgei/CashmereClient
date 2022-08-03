//CloseBagRequestBody
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class CloseBagRequestBody
  {
    [XmlElement(ElementName = "ContainerNumber")]
    public string ContainerNumber { get; set; }

    [XmlElement(ElementName = "Action")]
    public string Action { get; set; }
  }
}
