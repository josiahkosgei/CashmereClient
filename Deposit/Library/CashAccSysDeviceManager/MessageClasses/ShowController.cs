//ShowController
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class ShowController : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (ShowController));

    [XmlElement(ElementName = "body")]
    public string Body { get; set; } = "";

    public ShowController()
    {
    }

    public ShowController(int seqno)
    {
      MessageID = 105;
      MessageName = Enum.GetName(typeof (MessageType), MessageID);
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => ShowController._serializer;
  }
}
