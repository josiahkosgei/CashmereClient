
// Bag


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "Bag")]
    public class Bag
    {
        [XmlAttribute(AttributeName = "Number")]
        public string Number { get; set; }

        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "NoteLevel")]
        public int NoteLevel { get; set; }

        [XmlAttribute(AttributeName = "NoteCapacity")]
        public long NoteCapacity { get; set; }

        [XmlAttribute(AttributeName = "ValueLevel")]
        public long ValueLevel { get; set; }

        [XmlAttribute(AttributeName = "ValueCapacity")]
        public long ValueCapacity { get; set; }

        [XmlAttribute(AttributeName = "PercentFull")]
        public int PercentFull { get; set; }
    }
}
