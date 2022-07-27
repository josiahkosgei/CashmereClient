
// BagRemovalReportNoteCount


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "NoteCount")]
    public class BagRemovalReportNoteCount
    {
        [XmlAttribute(AttributeName = "Currency")]
        public string Currency { get; set; }

        [XmlAttribute(AttributeName = "Denomination")]
        public int Denomination { get; set; }

        [XmlAttribute(AttributeName = "Count")]
        public int Count { get; set; }
    }
}
