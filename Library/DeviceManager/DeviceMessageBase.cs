using System;
using System.Xml.Serialization;

namespace DeviceManager
{
    [Serializable]
    public abstract class DeviceMessageBase
    {
        [XmlIgnore]
        public int MessageID { get; set; }

        [XmlIgnore]
        public string MessageName { get; set; }

        [XmlIgnore]
        public int SequenceNumber { get; set; }
    }
}
