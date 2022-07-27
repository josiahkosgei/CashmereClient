
// RequestError


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class RequestError : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(RequestError));

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

        internal new static XmlSerializer Serializer => _serializer;
    }
}
