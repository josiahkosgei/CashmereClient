
//CashAccSysMessageBase


using DeviceManager;
using System;
using System.Xml.Serialization;

namespace CashAccSysDeviceManager
{
  [Serializable]
  public class CashAccSysMessageBase : DeviceMessageBase
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (CashAccSysMessageBase), new XmlRootAttribute()
    {
      ElementName = "CCP",
      IsNullable = true
    });

    [XmlAttribute(AttributeName = "MsgID")]
    public new int MessageID { get; set; }

    [XmlAttribute(AttributeName = "MsgName")]
    public new string MessageName { get; set; }

    [XmlAttribute(AttributeName = "SeqNo")]
    public new int SequenceNumber { get; set; }

    internal static XmlSerializer Serializer => _serializer;
  }
}
