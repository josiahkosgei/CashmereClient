
// TransactionStatusRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class TransactionStatusRequest : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(TransactionStatusRequest));

        [XmlElement(ElementName = "body")]
        public string Body { get; set; } = "";

        public TransactionStatusRequest()
        {
        }

        public TransactionStatusRequest(int seqno)
        {
            MessageID = 302;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
