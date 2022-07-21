
// StatusReport


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class StatusReport : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(StatusReport));

        [XmlElement(ElementName = "status")]
        public Status Status { get; set; }

        public StatusReport()
        {
        }

        public StatusReport(int seqno)
        {
            MessageID = 103;
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
