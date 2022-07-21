using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Cashmere.API.Messaging
{
    public abstract class APIResponseBase : APIMessageBase
    {
        [DataMember]
        public string ServerErrorCode { get; set; }

        [DataMember]
        public string ServerErrorMessage { get; set; }

        [DataMember]
        public string PublicErrorCode { get; set; }

        [DataMember]
        public string PublicErrorMessage { get; set; }

        [JsonProperty(Required = Required.Always)]
        [DataMember(IsRequired = true)]
        public bool IsSuccess { get; set; }

        [JsonProperty(Required = Required.Always)]
        [DataMember(IsRequired = true)]
        public string RequestID { get; set; } = string.Empty;
    }
}
