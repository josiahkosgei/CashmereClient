//DropResultBody
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class DropResultBody
  {
    [XmlElement(ElementName = "SoftwareVersion")]
    public string SoftwareVersion { get; set; }

    [XmlElement(ElementName = "DeviceProductName")]
    public string DeviceProductName { get; set; }

    [XmlElement(ElementName = "DeviceSerialNumber")]
    public string DeviceSerialNumber { get; set; }

    [XmlElement(ElementName = "LocationName")]
    public string LocationName { get; set; }

    [XmlElement(ElementName = "InputNumber")]
    public string InputNumber { get; set; }

    [XmlElement(ElementName = "InputSubNumber")]
    public string InputSubNumber { get; set; }

    [XmlElement(ElementName = "OutputNumber")]
    public string OutputNumber { get; set; }

    [XmlElement(ElementName = "OutputSubNumber")]
    public string OutputSubNumber { get; set; }

    [XmlElement(ElementName = "TrxCode")]
    public int TrxCode { get; set; }

    [XmlElement(ElementName = "BagNumber")]
    public string BagNumber { get; set; }

    [XmlElement(ElementName = "Rejected")]
    public int Rejected { get; set; }

    [XmlElement(ElementName = "NoteJam")]
    public int NoteJam { get; set; }

    [XmlElement(ElementName = "DropMode")]
    public string DropMode { get; set; }

    [XmlElement(ElementName = "TranAmount")]
    public long TranAmount { get; set; }

    [XmlElement(ElementName = "TranCycle")]
    public int TranCycle { get; set; }

    [XmlElement(ElementName = "ContainerCycle")]
    public int ContainerCycle { get; set; }

    [XmlElement(ElementName = "ContainerNumber")]
    public string ContainerNumber { get; set; }

    [XmlElement(ElementName = "TotalNumberOfNotes")]
    public int TotalNumberOfNotes { get; set; }

    [XmlElement(ElementName = "NoteCounts")]
    public NoteCounts NoteCounts { get; set; }

    [XmlElement(ElementName = "Reference")]
    public string Reference { get; set; }

    [XmlElement(ElementName = "ShiftReference")]
    public string ShiftReference { get; set; }

    [XmlElement(ElementName = "DateTime")]
    public string DateTime { get; set; }
  }
}
