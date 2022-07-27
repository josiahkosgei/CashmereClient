using Newtonsoft.Json;

namespace Cashmere.API.Messaging
{
    public abstract class APIRequestBase : APIMessageBase
    {
        [JsonProperty(Required = Required.Always)]
        public string Language { get; set; } = "en-gb";
    }
}
