
// DropRequestBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "body")]
    public class DropRequestBody
    {
        [XmlElement(ElementName = "Currency")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "TransactionValue")]
        public long TransactionValue { get; set; }

        [XmlElement(ElementName = "InputNumber")]
        public string InputNumber { get; set; }

        [XmlElement(ElementName = "InputSubNumber")]
        public string InputSubNumber { get; set; }

        [XmlElement(ElementName = "Reference")]
        public string Reference { get; set; }

        [XmlElement(ElementName = "ShiftReference")]
        public string ShiftReference { get; set; }

        [XmlElement(ElementName = "UserID")]
        public string UserID { get; set; }

        [XmlElement(ElementName = "DropType")]
        public string DropType { get; set; }
    }
}
