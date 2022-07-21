
// CancelCurrentProcess


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class CancelCurrentProcess : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(CancelCurrentProcess));

        [XmlElement(ElementName = "body")]
        public string Body { get; set; }

        public CancelCurrentProcess()
        {
        }

        public CancelCurrentProcess(int seqno)
        {
            MessageID = 12;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
            Body = "";
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
