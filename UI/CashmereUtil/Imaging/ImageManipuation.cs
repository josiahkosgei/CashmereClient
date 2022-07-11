
//Imaging.ImageManipuation


using Cashmere.Library.Standard.Utilities;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace CashmereUtil.Imaging
{
  public class ImageManipuation
  {
    public static BitmapImage GetBitmapFromFile(string path)
    {
      BitmapImage bitmapImage = new BitmapImage(path.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory + "\\Resources").Replace("{ResourceDir}", "pack://application:,,,").ToURI());
      bitmapImage.Freeze();
      return bitmapImage;
    }

    public static BitmapImage GetBitmapFromBytes(byte[] blob)
    {
      if (blob == null)
        return (BitmapImage) null;
      MemoryStream memoryStream1 = new MemoryStream();
      memoryStream1.Write(blob, 0, blob.Length);
      memoryStream1.Position = 0L;
      Image image = Image.FromStream(memoryStream1);
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      MemoryStream memoryStream2 = new MemoryStream();
      MemoryStream memoryStream3 = memoryStream2;
      ImageFormat png = ImageFormat.Png;
      image.Save(memoryStream3, png);
      memoryStream2.Seek(0L, SeekOrigin.Begin);
      bitmapImage.StreamSource = (Stream) memoryStream2;
      bitmapImage.EndInit();
      bitmapImage.Freeze();
      return bitmapImage;
    }
  }
}
