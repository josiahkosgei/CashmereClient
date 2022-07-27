using Cashmere.API.Messaging.APIClients;
using Cashmere.API.Messaging.Integration.ServerPing;
using Cashmere.API.Messaging.Integration.Transactions;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;

namespace Cashmere.API.Messaging.Integration.Controllers;

public class FinacleIntegrationServiceClient : APIClient, IMonitoringController, ITransactionController, IAccountsController
{
    public FinacleIntegrationServiceClient(
        string apiBaseAddress,
        Guid AppID,
        byte[] appKey,
        IConfiguration configuration)
        : base(new CashmereAPILogger(nameof(FinacleIntegrationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
    {
    }

    public async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(AccountNumberValidationRequest request)
    {
        return await SendAsync<AccountNumberValidationResponse>("api/CashIn/ValidateAccountNumber", request);
    }

    public async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
        ReferenceAccountNumberValidationRequest request)
    {
        return await SendAsync<ReferenceAccountNumberValidationResponse>("api/CashIn/ValidateReferenceAccountNumber", request);
    }

    public async Task<IntegrationServerPingResponse> ServerPingAsync(
        IntegrationServerPingRequest request)
    {
        return await SendAsync<IntegrationServerPingResponse>("healthchecks-api", request);
    }

    public async Task<PostTransactionResponse> PostTransactionAsync(
        PostTransactionRequest request)
    {
        return await SendAsync<PostTransactionResponse>("api/CashIn/PostTransaction", request);
    }

    public async Task<PostCITTransactionResponse> PostCITTransactionAsync(
        PostCITTransactionRequest request)
    {
        return await SendAsync<PostCITTransactionResponse>("api/CashIn/PostCITTransaction", request);
    }
}