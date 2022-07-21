
using Newtonsoft.Json;

namespace Cashmere.API.Messaging.CDM.GUIControl.AccountsLists
{
    public class Account
    {
        public byte[] Icon { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string Currency { get; set; }

        public bool Enabled { get; set; } = true;

        public override string ToString() => AccountNumber + "|" + AccountName + "|" + Currency;

        public override bool Equals(object obj) => obj is Account account && AccountNumber.Equals(account.AccountNumber);

        public override int GetHashCode() => AccountNumber.GetHashCode();
    }
}
