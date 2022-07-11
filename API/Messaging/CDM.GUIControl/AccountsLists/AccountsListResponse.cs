
using System.Collections.Generic;
using System.Linq;
using Cashmere.API.Messaging.Integration;

namespace Cashmere.API.Messaging.CDM.GUIControl.AccountsLists
{
  public class AccountsListResponse : APIResponseBase
  {
    public List<Account> Accounts { get; set; } = new List<Account>();

    public string RequestedCurrency { get; set; }

    public override string ToString() => string.Format("{0}\tAccountsReturned={1}", base.ToString(), Accounts.Count());
  }
}
