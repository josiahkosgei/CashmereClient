
//ByteArrayExtentions


using System;
using System.IO;

namespace Cashmere.Library.Standard.Utilities
{
  public static class ByteArrayExtentions
  {
    public static Stream ToMemoryStream(this byte[] b) => new MemoryStream(b);

    public static string ToBase64String(this byte[] b) => Convert.ToBase64String(b);
  }
}
