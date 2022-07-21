
//StringExtensions


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cashmere.Library.Standard.Utilities
{
    public static class StringExtensions
    {
        public static byte[] FromBase64String(this string s) => Convert.FromBase64String(s);

        public static Uri ToURI(this string s) => new(s);

        public static Stream ToStream(this string s) => s.ToStream(Encoding.UTF8);

        public static Stream ToStream(this string s, Encoding encoding) => new MemoryStream(encoding.GetBytes(s ?? ""));

        public static bool isEmail(this string s) => new RegexUtilities().IsValidEmail(s);

        public static string Left(this string s, int count) => s != null && s.Count() > 0 ? new string(s.Take(Math.Max(0, count)).ToArray()) : null;

        public static string Shuffle(this string s)
        {
            List<char> list = s.ToList();
            list.Shuffle();
            return new string(list.ToArray());
        }
    }
}
