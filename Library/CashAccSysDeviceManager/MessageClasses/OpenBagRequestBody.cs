
// OpenBagRequestBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class OpenBagRequestBody
  {
    [XmlElement(ElementName = "BagNumber")]
    public string BagNumber { get; set; }
  }
}
