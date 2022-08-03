using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "DispensedNotes")]
  public class DispensedNotes
  {
    [XmlElement(ElementName = "NoteCount")]
    public List<MessageClasses.NoteCount> NoteCount { get; set; }
  }
}
