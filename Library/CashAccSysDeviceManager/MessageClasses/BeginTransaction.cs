
// BeginTransaction


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class BeginTransaction : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(BeginTransaction));

        [XmlElement(ElementName = "body")]
        public string Body { get; set; }

        public BeginTransaction(int seqno)
        {
            MessageID = 300;
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
