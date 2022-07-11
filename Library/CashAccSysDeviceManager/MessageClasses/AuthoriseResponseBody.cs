
// AuthoriseResponseBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class AuthoriseResponseBody
  {
    [XmlElement(ElementName = "ClientID")]
    public int ClientID { get; set; }

    [XmlElement(ElementName = "Result")]
    public string Result { get; set; }

    [XmlElement(ElementName = "FailCode")]
    public string FailCode { get; set; }

    [XmlElement(ElementName = "SoftwareVersion")]
    public string SoftwareVersion { get; set; }
  }
}
