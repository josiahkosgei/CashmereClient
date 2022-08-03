//RequestErrorBody
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "body")]
  public class RequestErrorBody
  {
    [XmlElement(ElementName = "ReqMsgName")]
    public string ReqMsgName { get; set; }

    [XmlElement(ElementName = "ReqMsgID")]
    public int ReqMsgID { get; set; }

    [XmlElement(ElementName = "ReqSeqNo")]
    public int ReqSeqNo { get; set; }

    [XmlElement(ElementName = "ErrorCode")]
    public int ErrorCode { get; set; }

    [XmlElement(ElementName = "Error")]
    public string Error { get; set; }
  }
}
