using Cashmere.API.Messaging.APIClients;
using Cashmere.API.Messaging.Integration.ServerPing;
using Cashmere.API.Messaging.Integration.Transactions;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using BSAccountDetailsServiceReference;
using System.ServiceModel.Channels;
using System.ServiceModel;
using BSAccountFundsTransferServiceReference;
using AccountDetailsRequestHeaderType = BSAccountDetailsServiceReference.RequestHeaderType;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;

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
        var remoteAddress = new EndpointAddress("http://192.168.0.180/Account/AccountDetails/Get/3.0");

        var bsGetAccountDetailsClient = new BSGetAccountDetailsClient(new NetTcpBinding(SecurityMode.None), remoteAddress);
        var requestHeaderType = new AccountDetailsRequestHeaderType()
        {
            CorrelationID = new Guid().ToString(),
            CreationTimestamp = DateTime.Now,
            MessageID = new Guid().ToString(),
            Credentials = new BSAccountDetailsServiceReference.CredentialsType
            {
                BankID = "000",
                SystemCode = "0"
            },

        };
        var accountDetailsRequestType = new BSAccountDetailsServiceReference.AccountDetailsRequestType
        {
            AccountNumber = request.AccountNumber
        };
        var response = await bsGetAccountDetailsClient.GetAccountDetailsAsync(requestHeaderType, accountDetailsRequestType);
        return await SendAsync<AccountNumberValidationResponse>("api/Accounts/ValidateAccountNumber", request);
    }

    public async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
        ReferenceAccountNumberValidationRequest request)
    {
        return await SendAsync<ReferenceAccountNumberValidationResponse>("api/Accounts/ValidateReferenceAccountNumber", request);
    }

    public async Task<IntegrationServerPingResponse> ServerPingAsync(
        IntegrationServerPingRequest request)
    {
        return await SendAsync<IntegrationServerPingResponse>("api/Monitoring/ServerPing", request);
    }

    public async Task<PostTransactionResponse> PostTransactionAsync(
        PostTransactionRequest request)
    {
        var remoteAddress = new EndpointAddress("http://192.168.0.180/Account/FundsTransfer/SyncPost/4.0");

        var bsAccountClient = new BSAccountClient(new NetTcpBinding(SecurityMode.None), remoteAddress);
        var re = new PostRequest
        {
            RequestHeader = new FundsTransferRequestHeaderType()
            {
                CorrelationID = new Guid().ToString(),
                CreationTimestamp = DateTime.Now,
                MessageID = new Guid().ToString(),
                Credentials = new BSAccountFundsTransferServiceReference.CredentialsType
                {
                  // BankID = "000",
                    SystemCode = "0"
                },

            },
            FundsTransfer = new FundsTransferType
            {
                SystemCode = "0",
                
            }
        };
        var response = await bsAccountClient.PostAsync(re);
        return await SendAsync<PostTransactionResponse>("api/Transactions/PostTransaction", request);
    }

    public async Task<PostCITTransactionResponse> PostCITTransactionAsync(
        PostCITTransactionRequest request)
    {
        return await SendAsync<PostCITTransactionResponse>("api/Transactions/PostCITTransaction", request);
    }
}