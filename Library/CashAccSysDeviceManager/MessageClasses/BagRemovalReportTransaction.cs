
// BagRemovalReportTransaction


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "Transaction")]
    public class BagRemovalReportTransaction
    {
        [XmlElement(ElementName = "DateTime")]
        public string DateTime { get; set; }

        [XmlElement(ElementName = "InputNumber")]
        public string InputNumber { get; set; }

        [XmlElement(ElementName = "RefNo")]
        public string RefNo { get; set; }

        [XmlElement(ElementName = "Amount")]
        public long Amount { get; set; }
    }
}
