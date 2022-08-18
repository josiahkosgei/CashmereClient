
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
using System.Xml.Serialization;
using System.Xml;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Dynamic;

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
                var MessageReference = Guid.NewGuid().ToString().ToLower();
                var TransactionReference = GetRandomTransactionReference();
                FundsTransferRequestHeaderType requestHeaderType = new FundsTransferRequestHeaderType
                {
                    CreationTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    CorrelationID = Guid.NewGuid().ToString().ToLower(),
                    MessageID = Guid.NewGuid().ToString().ToLower(),
                    Credentials = new BSAccountFundsTransferServiceReference.CredentialsType()
                    {
                        SystemCode = _ftReq.SystemCode_Cred,
                        BankID = _ftReq.BankID
                    }
                };
                FundsTransferTypeTransactionItem[] typeTransactionItemArray = new FundsTransferTypeTransactionItem[2]
                {
                          new FundsTransferTypeTransactionItem()
                          {
                            TransactionReference = $"{TransactionReference}",
                            TransactionItemKey ="1",
                            AccountNumber = _ftReq.AccountNumber_Dr,
                            DebitCreditFlag = "D",
                            TransactionAmount = _ftReq.TransactionAmount_Dr,
                            TransactionCurrency = _ftReq.TransactionCurrency_Dr,
                            Narrative = _ftReq.Narrative_Dr,
                            TransactionCode="A2A"
                          },
                          new FundsTransferTypeTransactionItem()
                          {
                            TransactionReference = $"{TransactionReference}",
                            TransactionItemKey ="2",
                            AccountNumber = _ftReq.AccountNumber_Cr,
                            DebitCreditFlag = "C",
                            TransactionAmount = _ftReq.TransactionAmount_Cr,
                            TransactionCurrency = _ftReq.TransactionCurrency_Cr,
                            Narrative = _ftReq.Narrative_Cr,
                            TransactionCode="A2A"
                          }
                };
                FundsTransferType fundsTransferType = new FundsTransferType
                {
                    SystemCode = _ftReq.SystemCode_FT,
                    MessageReference = MessageReference,
                    TransactionType = _ftReq.TransactionType,
                    TransactionSubType = _ftReq.TransactionSubType,
                    TransactionDatetime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    TransactionDatetimeSpecified = true,
                    ValueDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                    ValueDateSpecified = true,
                    TransactionItems = typeTransactionItemArray
                };

                var soapRequest = @$"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
   <Header>
      <wstxns1:RequestHeader xmlns:wstxns1=""urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader"">
         <wstxns2:CreationTimestamp xmlns:wstxns2=""urn://co-opbank.co.ke/CommonServices/Data/Common"">{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns2:CreationTimestamp>
         <wstxns3:CorrelationID xmlns:wstxns3=""urn://co-opbank.co.ke/CommonServices/Data/Common"">9f0ceafe-1924-464c-9dad-a30af625b2ff</wstxns3:CorrelationID>
         <wstxns1:MessageID>{MessageReference}</wstxns1:MessageID>
        <wstxns1:Credentials>
            <wstxns1:SystemCode>{_ftReq.SystemCode_Cred}</wstxns1:SystemCode>
            <wstxns1:BankID>{_ftReq.BankID}</wstxns1:BankID>
        </wstxns1:Credentials>
      </wstxns1:RequestHeader>
   </Header>
   <Body>
      <wstxns4:FundsTransfer xmlns:wstxns4=""urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0"">
         <wstxns4:MessageReference>{TransactionReference}</wstxns4:MessageReference>
         <wstxns4:SystemCode>{fundsTransferType.SystemCode}</wstxns4:SystemCode>
         <wstxns4:TransactionDatetime>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns4:TransactionDatetime>
         <wstxns4:ValueDate>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns4:ValueDate>
         <wstxns4:TransactionType>T</wstxns4:TransactionType>
         <wstxns4:TransactionSubType>BI</wstxns4:TransactionSubType>
         <wstxns4:TransactionItems>
            <wstxns4:TransactionItem>
               <wstxns4:TransactionReference>{TransactionReference}</wstxns4:TransactionReference>
               <wstxns4:TransactionItemKey>{TransactionReference}_1</wstxns4:TransactionItemKey>
               <wstxns4:AccountNumber>{_ftReq.AccountNumber_Dr}</wstxns4:AccountNumber>
               <wstxns4:DebitCreditFlag>D</wstxns4:DebitCreditFlag>
               <wstxns4:TransactionAmount>{_ftReq.TransactionAmount_Dr}</wstxns4:TransactionAmount>
               <wstxns4:TransactionCurrency>{_ftReq.TransactionCurrency_Dr}</wstxns4:TransactionCurrency>
               <wstxns4:Narrative>{TransactionReference}|T|{_ftReq.AccountNumber_Dr}|7</wstxns4:Narrative>
               <wstxns4:TransactionCode>A2A</wstxns4:TransactionCode>
            </wstxns4:TransactionItem>
            <wstxns4:TransactionItem>
               <wstxns4:TransactionReference>{TransactionReference}</wstxns4:TransactionReference>
               <wstxns4:TransactionItemKey>{TransactionReference}_2</wstxns4:TransactionItemKey>
               <wstxns4:AccountNumber>{_ftReq.AccountNumber_Cr}</wstxns4:AccountNumber>
               <wstxns4:DebitCreditFlag>C</wstxns4:DebitCreditFlag>
               <wstxns4:TransactionAmount>{_ftReq.TransactionAmount_Cr}</wstxns4:TransactionAmount>
               <wstxns4:TransactionCurrency>{_ftReq.TransactionCurrency_Cr}</wstxns4:TransactionCurrency>
               <wstxns4:Narrative>{TransactionReference}|T|{_ftReq.AccountNumber_Cr}|7</wstxns4:Narrative>
               <wstxns4:TransactionCode>A2A</wstxns4:TransactionCode>
            </wstxns4:TransactionItem>
         </wstxns4:TransactionItems>
         <wstxns4:TransactionCharges>
            <wstxns4:Charge>
               <wstxns4:EventType/>
               <wstxns4:EventId/>
            </wstxns4:Charge>
         </wstxns4:TransactionCharges>
      </wstxns4:FundsTransfer>
   </Body>
</Envelope>";
                string str = "";
                Guid.NewGuid().ToString();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(soapRequest);
                HttpWebRequest webRequest = CreateWebRequest();
                using (Stream requestStream = webRequest.GetRequestStream())
                    xmlDocument.Save(requestStream);
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        str = streamReader.ReadToEnd();

                    }
                }
                _logger.LogInformation($"WebResponse: {str}");

                XDocument doc = XDocument.Parse(str);
                string jsonText = JsonConvert.SerializeXNode(doc);
                dynamic envelope = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
                return Ok(new
                {
                    message = "Transaction Success",
                    envelope
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"FundsTransfer Error StackTrace: {ex}");
                return BadRequest(new
                {
                    message = ex
                });
            }
        }

        [Route("[action]")]
        [AllowAnonymous]
        [HttpPost]
        private async Task<IActionResult> FundsTransfer_RawXml()
        {
            BankingController bankingController = this;
            try
            {

                var MessageReference = Guid.NewGuid().ToString().ToLower();
                var soapRequest = @$"<Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
   <Header>
      <wstxns1:RequestHeader xmlns:wstxns1=""urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader"">
         <wstxns2:CreationTimestamp xmlns:wstxns2=""urn://co-opbank.co.ke/CommonServices/Data/Common"">{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns2:CreationTimestamp>
         <wstxns3:CorrelationID xmlns:wstxns3=""urn://co-opbank.co.ke/CommonServices/Data/Common"">9f0ceafe-1924-464c-9dad-a30af625b2ff</wstxns3:CorrelationID>
         <wstxns1:MessageID>{MessageReference}</wstxns1:MessageID>
         <wstxns1:Credentials>
            <wstxns1:SystemCode>070</wstxns1:SystemCode>
            <wstxns1:BankID>01</wstxns1:BankID>
         </wstxns1:Credentials>
      </wstxns1:RequestHeader>
   </Header>
   <Body>
      <wstxns4:FundsTransfer xmlns:wstxns4=""urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0"">
         <wstxns4:MessageReference>213138544034</wstxns4:MessageReference>
         <wstxns4:SystemCode>070</wstxns4:SystemCode>
         <wstxns4:TransactionDatetime>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns4:TransactionDatetime>
         <wstxns4:ValueDate>{DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff")}</wstxns4:ValueDate>
         <wstxns4:TransactionType>T</wstxns4:TransactionType>
         <wstxns4:TransactionSubType>BI</wstxns4:TransactionSubType>
         <wstxns4:TransactionItems>
            <wstxns4:TransactionItem>
               <wstxns4:TransactionReference>213138544034</wstxns4:TransactionReference>
               <wstxns4:TransactionItemKey>213138544035_1</wstxns4:TransactionItemKey>
               <wstxns4:AccountNumber>01108188032100</wstxns4:AccountNumber>
               <wstxns4:DebitCreditFlag>D</wstxns4:DebitCreditFlag>
               <wstxns4:TransactionAmount>100</wstxns4:TransactionAmount>
               <wstxns4:TransactionCurrency>KES</wstxns4:TransactionCurrency>
               <wstxns4:Narrative>213138544035|T|01010070021064|7</wstxns4:Narrative>
               <wstxns4:TransactionCode>A2A</wstxns4:TransactionCode>
            </wstxns4:TransactionItem>
            <wstxns4:TransactionItem>
               <wstxns4:TransactionReference>213138544034</wstxns4:TransactionReference>
               <wstxns4:TransactionItemKey>213138544035_2</wstxns4:TransactionItemKey>
               <wstxns4:AccountNumber>01148743633300</wstxns4:AccountNumber>
               <wstxns4:DebitCreditFlag>C</wstxns4:DebitCreditFlag>
               <wstxns4:TransactionAmount>100</wstxns4:TransactionAmount>
               <wstxns4:TransactionCurrency>KES</wstxns4:TransactionCurrency>
               <wstxns4:Narrative>213138544035|T|01010070021064|7</wstxns4:Narrative>
               <wstxns4:TransactionCode>A2A</wstxns4:TransactionCode>
            </wstxns4:TransactionItem>
         </wstxns4:TransactionItems>
         <wstxns4:TransactionCharges>
            <wstxns4:Charge>
               <wstxns4:EventType/>
               <wstxns4:EventId/>
            </wstxns4:Charge>
         </wstxns4:TransactionCharges>
      </wstxns4:FundsTransfer>
   </Body>
</Envelope>";
                string str = "";
                Guid.NewGuid().ToString();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(soapRequest);
                HttpWebRequest webRequest = CreateWebRequest();
                using (Stream requestStream = webRequest.GetRequestStream())
                    xmlDocument.Save(requestStream);
                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        str = streamReader.ReadToEnd();
                        _logger.LogInformation($"WebResponse: {str}");

                    }
                }

                XDocument doc = XDocument.Parse(str);
                string jsonText = JsonConvert.SerializeXNode(doc);
                dynamic envelope = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
                return Ok(new
                {
                    message = "Transaction Success",
                    envelope
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"FundsTransfer Error StackTrace: {ex}");
                return bankingController.BadRequest(new
                {
                    message = ex
                });
            }
        }
        private string GetRandomTransactionReference()
        {
            Random random = new Random();
            string s = random.Next(0, 1000000000).ToString("D9");
            return s;
        }
        private HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_soaServerConfiguration.PostConfiguration.ServerURI);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Headers["SOAPAction"] = "\"Post\"";
            webRequest.Headers["Authorization"] = ("Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_soaServerConfiguration.PostConfiguration.Username + ":" + _soaServerConfiguration.PostConfiguration.Password)));
            webRequest.Method = "POST";
            return webRequest;
        }

    }
}
