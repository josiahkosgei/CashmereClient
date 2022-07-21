
// DropPauseBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "body")]
    public class DropPauseBody
    {
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
