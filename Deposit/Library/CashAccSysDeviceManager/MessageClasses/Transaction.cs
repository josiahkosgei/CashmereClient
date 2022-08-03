//Transaction
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Transaction")]
  public class Transaction
  {
    [XmlElement(ElementName = "Type")]
    public string Type { get; set; }

    [XmlElement(ElementName = "Status")]
    public string Status { get; set; }
  }
}
