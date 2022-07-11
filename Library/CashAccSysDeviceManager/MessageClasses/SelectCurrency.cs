
// SelectCurrency


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

    internal new static XmlSerializer Serializer => _serializer;
  }
}
