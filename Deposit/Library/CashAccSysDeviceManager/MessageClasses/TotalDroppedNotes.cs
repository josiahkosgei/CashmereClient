
// TotalDroppedNotes


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "TotalDroppedNotes")]
    public class TotalDroppedNotes
    {
        [XmlElement(ElementName = "NoteCount")]
        public List<NoteCount> NoteCount { get; set; }
    }
}
