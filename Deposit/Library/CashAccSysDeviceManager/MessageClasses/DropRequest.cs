
// DropRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class DropRequest : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(DropRequest));

        [XmlElement(ElementName = "body")]
        public DropRequestBody Body { get; set; }

        public DropRequest()
        {
        }

        public DropRequest(
          int seqno,
          string currency,
          string userID,
          long transactionValue = 0,
          string dropType = "DROP:NOTES",
          string reference = "Rent",
          string inputSubNumber = "Cash Deposit",
          string inputNumber = "Acc NO 100",
          string shiftReference = "New Shift Reference")
        {
            MessageID = 120;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
            Body = new DropRequestBody()
            {
                Currency = currency,
                DropType = dropType,
                InputNumber = inputNumber,
                InputSubNumber = inputSubNumber,
                Reference = reference,
                ShiftReference = shiftReference,
                TransactionValue = transactionValue,
                UserID = userID
            };
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
