
// OpenBagRequest


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class OpenBagRequest : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(OpenBagRequest));

        [XmlElement(ElementName = "body")]
        public OpenBagRequestBody Body { get; set; }

        public OpenBagRequest()
        {
        }

        public OpenBagRequest(int seqno, string bagNumber)
        {
            MessageID = 134;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
            Body = new OpenBagRequestBody()
            {
                BagNumber = bagNumber
            };
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
