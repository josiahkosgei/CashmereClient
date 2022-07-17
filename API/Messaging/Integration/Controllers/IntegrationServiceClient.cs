// IntegrationServiceClient


using Cashmere.API.Messaging.APIClients;
using Cashmere.API.Messaging.Integration.ServerPing;
using Cashmere.API.Messaging.Integration.Transactions;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;

namespace Cashmere.API.Messaging.Integration.Controllers
{
    public class IntegrationServiceClient : APIClient, IMonitoringController, ITransactionController, IAccountsController
    {
        public IntegrationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashmereAPILogger(nameof(IntegrationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(AccountNumberValidationRequest request) => await SendAsync<AccountNumberValidationResponse>("api/Accounts/ValidateAccountNumber", request);

        public async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          ReferenceAccountNumberValidationRequest request) => await SendAsync<ReferenceAccountNumberValidationResponse>("api/Accounts/ValidateReferenceAccountNumber", request);

        public async Task<IntegrationServerPingResponse> ServerPingAsync(
          IntegrationServerPingRequest request) => await SendAsync<IntegrationServerPingResponse>("api/Monitoring/ServerPing", request);

        public async Task<PostTransactionResponse> PostTransactionAsync(
          PostTransactionRequest request) => await SendAsync<PostTransactionResponse>("api/Transactions/PostTransaction", request);

        public async Task<PostCITTransactionResponse> PostCITTransactionAsync(
          PostCITTransactionRequest request) => await SendAsync<PostCITTransactionResponse>("api/Transactions/PostCITTransaction", request);
    }
}
