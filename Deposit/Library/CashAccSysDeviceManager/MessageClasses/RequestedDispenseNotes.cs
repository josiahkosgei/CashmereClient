
// RequestedDispenseNotes


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "RequestedDispenseNotes")]
    public class RequestedDispenseNotes
    {
        [XmlElement(ElementName = "NoteCount")]
        public List<NoteCount> NoteCount { get; set; }
    }
}
