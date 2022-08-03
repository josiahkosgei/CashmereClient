//Body
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class Body
  {
    [XmlElement(ElementName = "TransactionStatus")]
    public string TransactionStatus { get; set; }

    [XmlElement(ElementName = "TransactionType")]
    public string TransactionType { get; set; }

    [XmlElement(ElementName = "Result")]
    public string Result { get; set; }

    [XmlElement(ElementName = "InputNumber")]
    public string InputNumber { get; set; }

    [XmlElement(ElementName = "InputSubNumber")]
    public string InputSubNumber { get; set; }

    [XmlElement(ElementName = "OutputNumber")]
    public string OutputNumber { get; set; }

    [XmlElement(ElementName = "OutputSubNumber")]
    public string OutputSubNumber { get; set; }

    [XmlElement(ElementName = "Reference")]
    public string Reference { get; set; }

    [XmlElement(ElementName = "ShiftReference")]
    public string ShiftReference { get; set; }

    [XmlElement(ElementName = "RequestedDropAmount")]
    public long RequestedDropAmount { get; set; }

    [XmlElement(ElementName = "TotalDroppedAmount")]
    public long TotalDroppedAmount { get; set; }

    [XmlElement(ElementName = "NumberOfDrops")]
    public int NumberOfDrops { get; set; }

    [XmlElement(ElementName = "TotalDroppedNotes")]
    public TotalDroppedNotes TotalDroppedNotes { get; set; }

    [XmlElement(ElementName = "LastDroppedAmount")]
    public long LastDroppedAmount { get; set; }

    [XmlElement(ElementName = "LastDroppedNotes")]
    public LastDroppedNotes LastDroppedNotes { get; set; }

    [XmlElement(ElementName = "RequestedDispenseAmount")]
    public long RequestedDispenseAmount { get; set; }

    [XmlElement(ElementName = "DispensedAmount")]
    public long DispensedAmount { get; set; }

    [XmlElement(ElementName = "RequestedDispenseNotes")]
    public RequestedDispenseNotes RequestedDispenseNotes { get; set; }

    [XmlElement(ElementName = "DispensedNotes")]
    public DispensedNotes DispensedNotes { get; set; }
  }
}
