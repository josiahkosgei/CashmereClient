
// BagRemovalReportBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class BagRemovalReportBody
  {
    [XmlElement(ElementName = "Result")]
    public string Result { get; set; }

    [XmlElement(ElementName = "ReportID")]
    public int ReportID { get; set; }

    [XmlElement(ElementName = "Summary")]
    public Summary Summary { get; set; }
  }
}
