
using Cashmere.API.Messaging.CDM.GUIControl.AccountsLists;

namespace Cashmere.API.Messaging.CDM.GUIControl.Clients
{
    public interface IGUIControlClient
    {
        Task<AccountsListResponse> GetAccountsListAsync(
          AccountsListRequest request);

        Task<AccountsListResponse> SearchAccountAsync(
          AccountsListRequest request);
    }
}
