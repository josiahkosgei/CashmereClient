
// AuthoriseRequestBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "body")]
    public class AuthoriseRequestBody
    {
        [XmlElement(ElementName = "ClientID")]
        public int ClientID { get; set; }

        [XmlElement(ElementName = "MACAddress")]
        public string MACAddress { get; set; }

        [XmlElement(ElementName = "ClientType")]
        public string ClientType { get; set; }
    }
}
