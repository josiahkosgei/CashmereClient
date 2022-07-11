// Validations.ValidationRequestBase


using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Cashmere.API.Messaging.Integration.Validations
{
  public abstract class ValidationRequestBase : APIDeviceRequestBase
  {
    [JsonProperty(Required = Required.Always)]
    [DataMember(IsRequired = true)]
    public string AccountNumber { get; set; }

    [DataMember]
    public string Currency { get; set; }

    public int TransactionType { get; set; }

    public string CoreBankingString { get; set; }
  }
}
