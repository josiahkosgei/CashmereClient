
// DropStatus


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class DropStatus : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(DropStatus));

        [XmlElement(ElementName = "body")]
        public DropStatusBody Body { get; set; }

        public DropStatus()
        {
        }

        public DropStatus(int seqno)
        {
            MessageID = 121;
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
