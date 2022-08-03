//Summary
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System.Collections.Generic;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "Summary")]
  public class Summary
  {
    [XmlElement(ElementName = "Transaction")]
    public List<BagRemovalReportTransaction> Transaction { get; set; }

    [XmlElement(ElementName = "Information")]
    public BagRemovalReportInformation Information { get; set; }

    [XmlElement(ElementName = "NoteCounts")]
    public List<BagRemovalReportNoteCount> NoteCounts { get; set; }
  }
}
