// Transactions.PostTransactionRequestBase


using Newtonsoft.Json;

namespace Cashmere.API.Messaging.Integration.Transactions
{
    public class PostTransactionRequestBase : APIDeviceRequestBase
    {
        [JsonProperty(Required = Required.Always)]
        public PostTransactionData Transaction { get; set; }
    }
}
