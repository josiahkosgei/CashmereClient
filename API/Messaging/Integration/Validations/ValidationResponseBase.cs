// Validations.ValidationResponseBase


using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Cashmere.API.Messaging.Integration.Validations
{
    public abstract class ValidationResponseBase : APIResponseBase
    {
        [JsonProperty(Required = Required.Always)]
        [DataMember]
        public string RequestedAccountNumber { get; set; }

        [DataMember]
        public string RequestedCurrency { get; set; }

        [JsonProperty(Required = Required.Always)]
        public bool CanTransact { get; set; }

        [DataMember]
        public string AccountCurrency { get; set; }

        [DataMember]
        public string AccountName { get; set; }
    }
}
