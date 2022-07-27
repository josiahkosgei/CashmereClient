
// Line


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "Line")]
    public class Line
    {
        [XmlAttribute(AttributeName = "bold")]
        public int Bold { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
