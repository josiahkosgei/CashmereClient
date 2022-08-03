//LastDroppedNotes
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "LastDroppedNotes")]
  public class LastDroppedNotes
  {
    [XmlElement(ElementName = "NoteCount")]
    public List<MessageClasses.NoteCount> NoteCount { get; set; }
  }
}
