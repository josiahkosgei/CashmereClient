
// Type: Cashmere.Library.Standard.Security.APIHashing


using System;
using System.Security.Cryptography;
using System.Text;

namespace Cashmere.Library.Standard.Security
{
  public static class APIHashing
  {
    public const string AuthHeaderName = "hmacAuth";

    public static string GetAuthHeader(
      Guid APPId,
      string requestURI,
      string requestHttpMethod,
      byte[] secret,
      string content)
    {
      try
      {
        string str1 = string.Empty;
        string str2 = Convert.ToUInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
        string str3 = Guid.NewGuid().ToString("N");
        if (!string.IsNullOrWhiteSpace(content))
          str1 = CashmereHashing.SHA256WithEncode(content, Encoding.Unicode);
        string s = string.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestURI, str2, str3, str1);
        byte[] key = secret;
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        using HMACSHA256 hmacshA256 = new HMACSHA256(key);
        string base64String = Convert.ToBase64String(hmacshA256.ComputeHash(bytes));
        return string.Format("{0}:{1}:{2}:{3}", APPId, base64String, str3, str2);
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
