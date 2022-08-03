
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BSAccountDetailsServiceReference;
using BSAccountFundsTransferServiceReference;
using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Cashmere.Finacle.Integration.CQRS.Helpers;
using MediatR;
using Microsoft.Extensions.Options;
using Cashmere.Finacle.Integration.Models;
using FundsTransferRequestHeaderType = BSAccountFundsTransferServiceReference.RequestHeaderType;
namespace Cashmere.Finacle.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BankingController> _logger;
        private readonly SOAServerConfiguration _soaServerConfiguration;

        public BankingController(ILogger<BankingController> logger, IMediator mediator, IOptionsMonitor<SOAServerConfiguration> optionsMonitor)
        {
            _logger = logger;
            _soaServerConfiguration = optionsMonitor.CurrentValue;
            _mediator = mediator;
        }
        [Route("[action]")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> AccountValidate(string accountNumber)
        {
            try
            {
                _logger.LogInformation($"ValidateAccount Request: {accountNumber}");
                var remoteAddress = new EndpointAddress(_soaServerConfiguration.AccountValidationConfiguration.ServerURI);
                BSAccountDetailsServiceReference.RequestHeaderType RequestHeader = new();
                BSAccountDetailsServiceReference.RequestHeaderType requestHeaderType1 = RequestHeader;
                Guid guid = Guid.NewGuid();
                string str1 = guid.ToString();
                requestHeaderType1.CorrelationID = str1;
                BSAccountDetailsServiceReference.RequestHeaderType requestHeaderType2 = RequestHeader;
                guid = Guid.NewGuid();
                string str2 = guid.ToString();
                requestHeaderType2.MessageID = str2;
                RequestHeader.CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                RequestHeader.CreationTimestampSpecified = true;
                RequestHeader.Credentials = new BSAccountDetailsServiceReference.CredentialsType()
                {
                    SystemCode = "000",
                    BankID = "01"
                };
                AccountDetailsRequestType AccountDetailsRequest = new()
                {
                    AccountNumber = accountNumber
                };
                BasicHttpBinding basicHttpBinding = new();
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                basicHttpBinding.Name = "IssuingSoapBinding";
                basicHttpBinding.AllowCookies = false;
                basicHttpBinding.BypassProxyOnLocal = true;
                basicHttpBinding.UseDefaultWebProxy = false;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                BSGetAccountDetailsClient accountDetailsClient = new(basicHttpBinding, remoteAddress);
                accountDetailsClient.Endpoint.Binding = basicHttpBinding;
                ClientCredentials clientCredentials = new();
                clientCredentials.UserName.UserName = _soaServerConfiguration.AccountValidationConfiguration.Username;
                clientCredentials.UserName.Password = _soaServerConfiguration.AccountValidationConfiguration.Password;
                accountDetailsClient.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
                accountDetailsClient.ChannelFactory.Endpoint.EndpointBehaviors.Add(clientCredentials);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (new OperationContextScope(accountDetailsClient.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                    {
                        Headers = {
              [HttpRequestHeader.Accept] = "text/xml",
              [HttpRequestHeader.AcceptCharset] = "utf-8",
              [HttpRequestHeader.Authorization] = ("Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(accountDetailsClient.ClientCredentials.UserName.UserName + ":" + accountDetailsClient.ClientCredentials.UserName.Password)))
            }
                    };
                    AccountDetailsResponseType AccountDetailsResponse;
                    var accountDetails = await accountDetailsClient.GetAccountDetailsAsync(RequestHeader, AccountDetailsRequest);

                    _logger.LogInformation($"ValidateAccount Response: {accountDetails.ToJson()}");
                    return Ok(new
                    {
                        message = "Request successfully processed ",
                        responseHeader = accountDetails.ResponseHeader,
                        accountDetailsResponse = accountDetails.AccountDetailsResponse
                    });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"ValidateAccount Error: {ex.Message}");
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> FundsTransfer([FromBody] FundsTransferRequest _ftReq)
        {
            try
            {
                var remoteAddress = new EndpointAddress(_soaServerConfiguration.PostConfiguration.ServerURI);
                _logger.LogInformation($"FundsTransfer Request: {_ftReq.ToJson()}");
                BSAccountFundsTransferServiceReference.RequestHeaderType requestHeaderType = new()
                {
                    CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    CorrelationID = Guid.NewGuid().ToString(),
                    MessageID = Guid.NewGuid().ToString(),
                    Credentials = new BSAccountFundsTransferServiceReference.CredentialsType()
                    {
                        SystemCode = _ftReq.SystemCode_Cred
                    },
                };
                FundsTransferTypeTransactionItem[] typeTransactionItemArray = new FundsTransferTypeTransactionItem[2]
                {
                      new FundsTransferTypeTransactionItem()
                      {
                        TransactionReference = _ftReq.DebitRequest.TransactionReference,
                        TransactionItemKey = _ftReq.DebitRequest.TransactionItemKey,
                        AccountNumber = _ftReq.DebitRequest.AccountNumber,
                        DebitCreditFlag = "D",
                        TransactionAmount = _ftReq.DebitRequest.TransactionAmount,
                        TransactionCurrency = _ftReq.DebitRequest.TransactionCurrency,
                        Narrative = _ftReq.DebitRequest.Narrative,
                        TransactionCode =_soaServerConfiguration.PostConfiguration.TransactionCode
                      },
                      new FundsTransferTypeTransactionItem()
                      {
                        TransactionReference = _ftReq.CreditRequest.TransactionReference,
                        TransactionItemKey = _ftReq.CreditRequest.TransactionItemKey,
                        AccountNumber = _ftReq.CreditRequest.AccountNumber,
                        DebitCreditFlag = "C",
                        TransactionAmount = _ftReq.CreditRequest.TransactionAmount,
                        TransactionCurrency = _ftReq.CreditRequest.TransactionCurrency,
                        Narrative = _ftReq.CreditRequest.Narrative,
                        TransactionCode =_soaServerConfiguration.PostConfiguration.TransactionCode
                      }
                };
                FundsTransferType fundsTransferType = new()
                {
                    SystemCode = _ftReq.SystemCode_FT,
                    MessageReference = _ftReq.MessageReference,
                    TransactionDatetime = DateTime.Now.ToString(),
                    TransactionType = _ftReq.TransactionType,
                    TransactionSubType = _ftReq.TransactionSubType,
                    TransactionItem = typeTransactionItemArray
                };

                BSAccountClient bsAccountClient = new();

                BasicHttpBinding basicHttpBinding = new();
                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
                basicHttpBinding.Name = "IssuingSoapBinding";
                basicHttpBinding.AllowCookies = false;
                basicHttpBinding.BypassProxyOnLocal = true;
                basicHttpBinding.UseDefaultWebProxy = false;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                basicHttpBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                bsAccountClient.Endpoint.Binding = basicHttpBinding;
                bsAccountClient.Endpoint.Address = remoteAddress;

                _logger.LogInformation($"----------------------------------------------------------------");
                _logger.LogInformation($"FundsTransfer fundsTransferType: {fundsTransferType.ToJson()}");

                PostRequest request = new()
                {
                    FundsTransfer = fundsTransferType,
                    RequestHeader = requestHeaderType
                };
                ClientCredentials clientCredentials = new();
                clientCredentials.UserName.UserName = _soaServerConfiguration.AccountValidationConfiguration.Username;
                clientCredentials.UserName.Password = _soaServerConfiguration.AccountValidationConfiguration.Password;

                bsAccountClient.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
                bsAccountClient.ChannelFactory.Endpoint.EndpointBehaviors.Add(clientCredentials);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (new OperationContextScope(bsAccountClient.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = new HttpRequestMessageProperty()
                    {
                        Headers = {
                              [HttpRequestHeader.Accept] = "text/xml",
                              [HttpRequestHeader.AcceptCharset] = "utf-8",
                              [HttpRequestHeader.Authorization] = ("Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(bsAccountClient.ClientCredentials.UserName.UserName + ":" + bsAccountClient.ClientCredentials.UserName.Password)))
                            }
                    };
                    var result = bsAccountClient.PostAsync(request).Result;
                    _logger.LogInformation($"----------------------------------------------------------------");
                    _logger.LogInformation($"FundsTransfer Response: {result.ToJson()}");
                    _logger.LogInformation($"----------------------------------------------------------------");
                    return Ok(new
                    {

                        responseHeader = result.ResponseHeader,
                        fundsTransfer = result.FundsTransfer,
                        message = "Transaction Success"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"FundsTransfer Error: {ex.Message}");
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> FundsTransfer_Raw()
        {
            BankingController bankingController = this;
            try
            {
                string str = CoopFinancle.ExecutePOST();
                return bankingController.Ok(new
                {
                    message = "Transaction Success",
                    resHeader = str
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"FundsTransfer_Raw Error: {ex.Message}");
                return bankingController.BadRequest(new
                {
                    message = ex
                });
            }
        }
    }
}
