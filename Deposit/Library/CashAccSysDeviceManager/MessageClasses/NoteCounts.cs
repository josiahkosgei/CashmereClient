//NoteCounts
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "NoteCounts")]
  public class NoteCounts
  {
    [XmlElement(ElementName = "NoteCount")]
    public List<MessageClasses.NoteCount> NoteCount { get; set; }
  }
}
