//Acceptors
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Acceptors")]
  public class Acceptors
  {
    [XmlElement(ElementName = "BA")]
    public List<MessageClasses.BA> BA { get; set; }

    [XmlElement(ElementName = "CA")]
    public List<MessageClasses.CA> CA { get; set; }
  }
}
