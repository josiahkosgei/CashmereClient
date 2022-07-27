
using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Communication.Emails
{
    public class EmailRequest : APIRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public EmailMessage Message { get; set; }
    }
}
