//RequestError
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class RequestError : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (RequestError));

    [XmlElement(ElementName = "body")]
    public RequestErrorBody Body { get; set; }

    public RequestError()
    {
    }

    public RequestError(int seqno)
    {
      MessageID = 11;
      SequenceNumber = seqno;
    }

    internal new static XmlSerializer Serializer => RequestError._serializer;
  }
}
