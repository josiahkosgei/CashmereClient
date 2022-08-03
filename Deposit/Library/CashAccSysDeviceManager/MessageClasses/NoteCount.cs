//NoteCount
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "NoteCount")]
  public class NoteCount
  {
    [XmlAttribute(AttributeName = "Currency")]
    public string Currency { get; set; }

    [XmlAttribute(AttributeName = "Denomination")]
    public int Denomination { get; set; }

    [XmlAttribute(AttributeName = "Count")]
    public int Count { get; set; }

    [XmlAttribute(AttributeName = "Balance")]
    public long Balance { get; set; }
  }
}
