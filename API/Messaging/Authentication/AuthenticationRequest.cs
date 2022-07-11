using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Authentication
{
  public class AuthenticationRequest : APIRequestBase
  {
    [JsonIgnore]
    private object ToStringWithHiddenLock = new object();

    [JsonProperty(Required = Required.Always)]
    public string Username { get; set; }

    public bool ShouldSerializePassword() => SerializeHidden;

    [JsonProperty(Required = Required.Always)]
    public string Password { get; set; }

    [JsonProperty(Required = Required.Always)]
    public bool IsADUser { get; set; }

    public override string ToStringWithHidden()
    {
      lock (ToStringWithHiddenLock)
      {
        SerializeHidden = true;
        string str = JsonConvert.SerializeObject(this);
        SerializeHidden = false;
        return str;
      }
    }

    [JsonIgnore]
    private bool SerializeHidden { get; set; }
  }
}
