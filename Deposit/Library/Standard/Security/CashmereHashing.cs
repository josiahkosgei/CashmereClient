
// Type: Cashmere.Library.Standard.Security.CashmereHashing


using Cashmere.Library.Standard.Utilities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Cashmere.Library.Standard.Security
{
    public static class CashmereHashing
    {
        public static string sha256_hash(string value, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256.Create())
            {
                foreach (byte num in ComputeHash(encoding.GetBytes(value)))
                    stringBuilder.Append(num.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        public static byte[] ComputeHash(byte[] values, HashAlgorithm HashAlgorithm = null)
        {
            if (HashAlgorithm == null)
                HashAlgorithm = SHA256.Create();
            using (HashAlgorithm)
                return HashAlgorithm.ComputeHash(values);
        }

        public static string GenerateHMACSHA256String(string secret, string Json) => Encoding.Unicode.GetString(new HMACSHA256(Convert.FromBase64String(secret)).ComputeHash(Json.ToStream(Encoding.Unicode)));

        public static string GenerateHMACSHA512String(string secret, string Json) => Encoding.Unicode.GetString(new HMACSHA512(Convert.FromBase64String(secret)).ComputeHash(Json.ToStream(Encoding.Unicode)));

        public static string EncodeTo64(string toEncode) => Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));

        public static string EncodeTo64(byte[] toEncode) => Convert.ToBase64String(toEncode);

        public static string SHA256WithEncode(string input, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.Unicode;
            return EncodeTo64(sha256_hash(input, encoding));
        }

        public static string SHA256WithEncode(byte[] input) => EncodeTo64(ComputeHash(input));

        public static byte[] HMACSHA512(byte[] password, byte[] value)
        {
            Encoding unicode = Encoding.Unicode;
            using HMACSHA512 hmacshA512 = new HMACSHA512(password);
            return hmacshA512.ComputeHash(value);
        }
    }
}
