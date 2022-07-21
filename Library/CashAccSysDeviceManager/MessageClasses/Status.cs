
// Status


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "status")]
    public class Status
    {
        [XmlElement(ElementName = "ControlState")]
        public string ControllerState { get; set; }

        [XmlElement(ElementName = "Acceptors")]
        public Acceptors Acceptors { get; set; }

        [XmlElement(ElementName = "Dispensers")]
        public Dispensers Dispensers { get; set; }

        [XmlElement(ElementName = "Bag")]
        public Bag Bag { get; set; }

        [XmlElement(ElementName = "Sensors")]
        public Sensors Sensors { get; set; }

        [XmlElement(ElementName = "Escrow")]
        public Escrow Escrow { get; set; }

        [XmlElement(ElementName = "Transaction")]
        public Transaction Transaction { get; set; }

        [XmlElement(ElementName = "DateTime")]
        public string DateTime { get; set; }
    }
}
