// Transactions.PostTransactionData


using Newtonsoft.Json;
using System;

namespace Cashmere.API.Messaging.Integration.Transactions
{
  public class PostTransactionData
  {
    [JsonProperty(Required = Required.Always)]
    public Guid TransactionID { get; set; }

    [JsonProperty(Required = Required.Always)]
    public PostBankAccount DebitAccount { get; set; }

    [JsonProperty(Required = Required.Always)]
    public PostBankAccount CreditAccount { get; set; }

    [JsonProperty(Required = Required.Always)]
    public Decimal Amount { get; set; }

    [JsonProperty(Required = Required.Always)]
    public Guid DeviceID { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string DeviceNumber { get; set; }

    [JsonProperty(Required = Required.Always)]
    public DateTime DateTime { get; set; }

    public string Narration { get; set; }
  }
}
