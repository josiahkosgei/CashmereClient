
//Licensing.SymmetricKey


using System;
using System.Xml.Serialization;

namespace CashmereUtil.Licensing
{
    [XmlRoot("SK")]
    public struct SymmetricKey
    {
        [XmlElement("K")]
        public byte[] Key;
        [XmlElement("I")]
        public byte[] IV;
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(SymmetricKey));

        public static XmlSerializer Serializer => _serializer;
    }
}
