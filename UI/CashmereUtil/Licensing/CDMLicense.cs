
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace CashmereUtil.Licensing
{
  public class CDMLicense
  {
    [NonSerialized]
    private static XmlSerializer _serializer = new XmlSerializer(typeof (CDMLicense));

    public string CustomerName { get; set; }

    public string CustomerAddress { get; set; }

    [XmlIgnore]
    public BitmapSource Logo { get; set; }

    [XmlElement("Logo")]
    public byte[] ImageBuffer
    {
      get
      {
        byte[] numArray = null;
        if (Logo != null)
        {
            using MemoryStream memoryStream = new MemoryStream();
            PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(Logo));
            pngBitmapEncoder.Save((Stream) memoryStream);
            numArray = memoryStream.ToArray();
        }
        return numArray;
      }
      set
      {
        if (value == null)
        {
          Logo = (BitmapSource) null;
        }
        else
        {
            using MemoryStream memoryStream = new MemoryStream(value);
            Logo = (BitmapSource) BitmapDecoder.Create((Stream) memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames[0];
        }
      }
    }

    public string MachineName { get; set; }

    public string ActivationKey { get; set; }

    public string LicenseType { get; set; }

    public DateTime ExpiryDate { get; set; }

    public bool IsFullLicense { get; set; }

    public List<LicenseFeatureItem> Features { get; set; }

    public static XmlSerializer Serializer => _serializer;

    public bool Grant(LicenseFeatureItem keyValuePair) => IsFullLicense ? IsFullLicense : Features.Contains(keyValuePair);
  }
}
