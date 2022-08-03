//StatusReport
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class StatusReport : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (StatusReport));

    [XmlElement(ElementName = "status")]
    public Status Status { get; set; }

    public StatusReport()
    {
    }

    public StatusReport(int seqno)
    {
      MessageID = 103;
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => StatusReport._serializer;
  }
}
