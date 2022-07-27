using Cashmere.API.Messaging.CDM.GUIControl.AccountsLists;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Cashmere.API.Messaging.APIClients;

namespace Cashmere.API.Messaging.CDM.GUIControl.Clients
{
    public class GUIControlServiceClient : APIClient, IGUIControlClient
    {
        public GUIControlServiceClient(
          string apiBaseAddress,
          Guid appID,
          byte[] appKey,
          IConfiguration configuration)
          : base((ICashmereAPILogger)new CashmereAPILogger(nameof(GUIControlServiceClient), configuration), apiBaseAddress, appID, appKey, configuration)
        {
        }

        public async Task<AccountsListResponse> GetAccountsListAsync(AccountsListRequest request) => await SendAsync<AccountsListResponse>("api/AccountsList/GetAccountsList", request);

        public async Task<AccountsListResponse> SearchAccountAsync(
          AccountsListRequest request) => await SendAsync<AccountsListResponse>("api/AccountsList/SearchAccountsList", request);
    }
}
