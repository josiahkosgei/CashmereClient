//PrintReportBody
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

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
