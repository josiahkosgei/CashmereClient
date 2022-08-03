using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "RequestedDispenseNotes")]
  public class RequestedDispenseNotes
  {
    [XmlElement(ElementName = "NoteCount")]
    public List<MessageClasses.NoteCount> NoteCount { get; set; }
  }
}
