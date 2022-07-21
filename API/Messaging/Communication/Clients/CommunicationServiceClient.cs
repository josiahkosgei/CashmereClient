using Cashmere.API.Messaging.APIClients;
using Cashmere.API.Messaging.Communication.Emails;
using Cashmere.API.Messaging.Communication.SMSes;
using Cashmere.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Cashmere.API.Messaging.Communication.Clients
{
    public class CommunicationServiceClient : APIClient, IEmailController, ISMSController
    {
        public CommunicationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashmereAPILogger(nameof(CommunicationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<EmailResponse> SendEmailAsync(EmailRequest request) => await SendAsync<EmailResponse>("api/Email/SendEmail", request);

        public async Task<SMSResponse> SendSMSAsync(SMSRequest request) => await SendAsync<SMSResponse>("api/SMS/SendSMS", request);
    }
}
