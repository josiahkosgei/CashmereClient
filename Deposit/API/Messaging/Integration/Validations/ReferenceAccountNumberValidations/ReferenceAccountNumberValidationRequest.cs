// Validations.ReferenceAccountNumberValidations.ReferenceAccountNumberValidationRequest


using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations
{
    public class ReferenceAccountNumberValidationRequest : ValidationRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public string ReferenceAccountNumber { get; set; }
    }
}
