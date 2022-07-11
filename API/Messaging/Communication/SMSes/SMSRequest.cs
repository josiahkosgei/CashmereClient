using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Communication.SMSes
{
  public class SMSRequest : APIRequestBase
  {
    [JsonProperty(Required = Required.Always)]
    public SMSMessage SMSMessage { get; set; }
  }
}
