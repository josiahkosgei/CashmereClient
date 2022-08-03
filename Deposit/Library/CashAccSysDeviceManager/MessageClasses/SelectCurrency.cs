//SelectCurrency
// Assembly: CashAccSysDeviceManager, Version=6.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1A094D69-2C1B-4665-AB8C-664E4DCABA99
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashmereDeposit\App\UI\6.0\CashAccSysDeviceManager.dll

using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
  [XmlRoot(ElementName = "CCP")]
  public class SelectCurrency : CashAccSysMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (SelectCurrency));

    [XmlElement(ElementName = "body")]
    public SelectCurrencyBody Body { get; set; }

    public SelectCurrency()
    {
    }

    public SelectCurrency(int seqno, string currency)
    {
      if (currency == null)
      {
        Console.WriteLine("Cannot set currency because currency is null.");
      }
      else
      {
        currency = currency.Trim().Substring(0, 3).ToUpper();
        MessageID = 110;
        MessageName = Enum.GetName(typeof (MessageType), MessageID);
        SequenceNumber = seqno;
        Body = new SelectCurrencyBody()
        {
          Currency = currency
        };
      }
    }

    internal new static XmlSerializer Serializer => SelectCurrency._serializer;
  }
}
