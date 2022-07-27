
// ShowController


using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "CCP")]
    public class ShowController : CashAccSysMessageBase
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(ShowController));

        [XmlElement(ElementName = "body")]
        public string Body { get; set; } = "";

        public ShowController()
        {
        }

        public ShowController(int seqno)
        {
            MessageID = 105;
            MessageName = Enum.GetName(typeof(MessageType), MessageID);
            SequenceNumber = seqno;
        }

        internal new static XmlSerializer Serializer => _serializer;
    }
}
