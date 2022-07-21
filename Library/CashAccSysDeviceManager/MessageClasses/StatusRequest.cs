
// StatusRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class StatusRequest : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(StatusRequest));

        [XmlElement(ElementName = "body")]
        public string Body { get; set; }

        public StatusRequest()
        {
        }

        public StatusRequest(int seqno)
        {
            MessageID = 102;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
            Body = "";
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
