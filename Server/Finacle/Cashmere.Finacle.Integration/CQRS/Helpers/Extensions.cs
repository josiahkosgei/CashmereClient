
using System.Text;
using System.Text.Json;
namespace Cashmere.Finacle.Integration.CQRS.Helpers
{
    public static class Extensions
    {
        public static string AsJson(this object o) => JsonSerializer.Serialize(o);
        public static string AppendAsURL(this string baseURL, params string[] segments)
        {            
                return string.Join("/", new[] { baseURL.TrimEnd('/') }
                    .Concat(segments?.Select(s => s?.Trim('/'))));
        }
    }
}
