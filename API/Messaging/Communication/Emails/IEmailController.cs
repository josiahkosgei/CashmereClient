
using System.Threading.Tasks;

namespace Cashmere.API.Messaging.Communication.Emails
{
  public interface IEmailController
  {
    Task<EmailResponse> SendEmailAsync(EmailRequest request);
  }
}
