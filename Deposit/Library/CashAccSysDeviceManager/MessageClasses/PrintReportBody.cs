
// PrintReportBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "body")]
    public class PrintReportBody
    {
        [XmlElement(ElementName = "ReportType")]
        public string ReportType { get; set; }

        [XmlElement(ElementName = "UserID")]
        public string UserID { get; set; }
    }
}
