
// Dispensers


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "Dispensers")]
    public class Dispensers
    {
        [XmlElement(ElementName = "ND")]
        public List<ND> ND { get; set; }
    }
}
