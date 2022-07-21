
//DictionaryExtensions


using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashmere.Library.Standard.Utilities
{
    public static class DictionaryExtensions
    {
        public static string FromDictionaryToJson(this Dictionary<string, string> dictionary) => "{" + string.Join(",", dictionary.Select(kvp => string.Format("\"{0}\":\"{1}\"", kvp.Key, kvp.Value))) + "}";

        public static Dictionary<string, string> FromJsonToDictionary(this string json) => json.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split(',').Select(t => t.Split(':')).ToDictionary(item => item[0], item => item[1]);
    }
}
