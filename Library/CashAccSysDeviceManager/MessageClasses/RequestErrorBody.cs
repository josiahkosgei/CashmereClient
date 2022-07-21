
// RequestErrorBody


using System.Xml.Serialization;

namespace CashAccSysDeviceManager.MessageClasses
{
    [XmlRoot(ElementName = "body")]
    public class RequestErrorBody
    {
        [XmlElement(ElementName = "ReqMsgName")]
        public string ReqMsgName { get; set; }

        [XmlElement(ElementName = "ReqMsgID")]
        public int ReqMsgID { get; set; }

        [XmlElement(ElementName = "ReqSeqNo")]
        public int ReqSeqNo { get; set; }

        [XmlElement(ElementName = "ErrorCode")]
        public int ErrorCode { get; set; }

        [XmlElement(ElementName = "Error")]
        public string Error { get; set; }
    }
}
