//StatusRequest
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class StatusRequest : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (StatusRequest));

    [XmlElement(ElementName = "body")]
    public string Body { get; set; }

    public StatusRequest()
    {
    }

    public StatusRequest(int seqno)
    {
      MessageID = 102;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
      Body = "";
    }

    internal new static XmlSerializer Serializer => StatusRequest._serializer;
  }
}
