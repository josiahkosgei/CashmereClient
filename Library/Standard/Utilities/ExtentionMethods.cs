
//ExtentionMethods


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Cashmere.Library.Standard.Utilities
{
    public class ExtentionMethods
    {
        public static string GetDefaultMacAddress()
        {
            Dictionary<string, long> source = new Dictionary<string, long>();
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                    source[networkInterface.GetPhysicalAddress().ToString()] = networkInterface.GetIPStatistics().BytesSent + networkInterface.GetIPStatistics().BytesReceived;
            }
            string str = Regex.Replace(source.FirstOrDefault(x => x.Value > 0L).Key, ".{2}", "$0-");
            return str.Substring(0, str.Length - 1);
        }

        public static string SerializeToXml(object value)
        {
            StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            new XmlSerializer(value.GetType()).Serialize(stringWriter, value);
            return stringWriter.ToString();
        }
    }
}
