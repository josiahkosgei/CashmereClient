
namespace Cashmere.API.Messaging.Communication.Emails
{
  public interface IEmailConfiguration
  {
    string SmtpServer { get; set; }

    int SmtpPort { get; set; }

    string SmtpUsername { get; set; }

    string SmtpPassword { get; set; }

    string FromAddress { get; set; }

    string FromAddressName { get; set; }

    int SendInterval { get; set; }

    int SendRetryLimit { get; set; }

    int Timeout { get; set; }

    bool UseSSL { get; set; }

    string PopServer { get; set; }

    int PopPort { get; set; }

    string PopUsername { get; set; }

    string PopPassword { get; set; }

    bool UseAuth { get; set; }
  }
}
