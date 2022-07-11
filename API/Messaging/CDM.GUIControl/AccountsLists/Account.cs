
using Newtonsoft.Json;

namespace Cashmere.API.Messaging.CDM.GUIControl.AccountsLists
{
  public class Account
  {
    public byte[] Icon { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string account_number { get; set; }

    public string account_name { get; set; }

    public string currency { get; set; }

    public bool enabed { get; set; } = true;

    public override string ToString() => account_number + "|" + account_name + "|" + currency;

    public override bool Equals(object obj) => obj is Account account && account_number.Equals(account.account_number);

    public override int GetHashCode() => account_number.GetHashCode();
  }
}
