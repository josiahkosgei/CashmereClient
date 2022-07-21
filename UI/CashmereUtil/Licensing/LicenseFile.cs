
//Licensing.LicenseFile


using System;
using System.Xml.Serialization;

namespace CashmereUtil.Licensing
{
    public struct LicenseFile
    {
        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(LicenseFile));

        [XmlElement("A")]
        public string SymmetricKeyCypherText { get; set; }

        [XmlElement("B")]
        public string CDMLicenseCypherText { get; set; }

        public static XmlSerializer Serializer => _serializer;
    }
}
