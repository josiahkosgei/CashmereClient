
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Cashmere.Finacle.Integration.CQRS.Helpers
{
    public static class Extensions
    {
        public static string AsJson(this object o) => JsonSerializer.Serialize(o);
         public static string ToJson(this object o) => JsonConvert.SerializeObject(o);
        public static string AppendAsURL(this string baseURL, params string[] segments)
        {            
                return string.Join("/", new[] { baseURL.TrimEnd('/') }
                    .Concat(segments?.Select(s => s?.Trim('/'))));
        }
    }
}
