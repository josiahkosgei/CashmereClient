
using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Integration.Transactions
{
  public class PostBankAccount
  {
    [JsonProperty(Required = Required.Always)]
    public string AccountNumber { get; set; }

    public string AccountName { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Currency { get; set; }
  }
}
