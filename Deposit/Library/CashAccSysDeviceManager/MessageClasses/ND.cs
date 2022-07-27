
// ND


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "ND")]
    public class ND
    {
        [XmlElement(ElementName = "Cassette")]
        public List<Cassette> Cassette { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "DeviceID")]
        public string DeviceID { get; set; }

        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "CassetteState")]
        public string CassetteState { get; set; }

        [XmlAttribute(AttributeName = "DispenseCurrency")]
        public string DispenseCurrency { get; set; }
    }
}
