
// DispensedNotes


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "DispensedNotes")]
  public class DispensedNotes
  {
    [XmlElement(ElementName = "NoteCount")]
    public List<NoteCount> NoteCount { get; set; }
  }
}
