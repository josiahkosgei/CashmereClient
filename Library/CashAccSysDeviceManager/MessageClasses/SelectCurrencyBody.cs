
// SelectCurrencyBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class SelectCurrencyBody
  {
    [XmlElement(ElementName = "Currency")]
    public string Currency { get; set; }
  }
}
