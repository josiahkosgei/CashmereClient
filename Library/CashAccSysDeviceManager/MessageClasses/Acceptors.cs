
// Acceptors


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Acceptors")]
  public class Acceptors
  {
    [XmlElement(ElementName = "BA")]
    public List<BA> BA { get; set; }

    [XmlElement(ElementName = "CA")]
    public List<CA> CA { get; set; }
  }
}
