
// PrintTextBody


using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class PrintTextBody
  {
    [XmlElement(ElementName = "Line")]
    public List<Line> Line { get; set; }
  }
}
