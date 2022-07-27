
// DropResult


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class DropResult : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(DropResult));

        [XmlElement(ElementName = "body")]
        public DropResultBody Body { get; set; }

        public DropResult()
        {
        }

        public DropResult(int seqno)
        {
            MessageID = 124;
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
