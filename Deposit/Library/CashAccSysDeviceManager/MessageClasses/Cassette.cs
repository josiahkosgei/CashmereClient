//Cassette
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Cassette")]
  public class Cassette
  {
    [XmlAttribute(AttributeName = "No")]
    public string No { get; set; }

    [XmlAttribute(AttributeName = "Denom")]
    public string Denom { get; set; }

    [XmlAttribute(AttributeName = "Level")]
    public string Level { get; set; }

    [XmlAttribute(AttributeName = "Capacity")]
    public string Capacity { get; set; }
  }
}
