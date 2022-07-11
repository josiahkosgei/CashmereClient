
namespace Cashmere.API.Messaging.Communication.Emails
{
  public class EmailConfiguration : IEmailConfiguration
  {
    public string SmtpServer { get; set; }

    public int SmtpPort { get; set; }

    public string SmtpUsername { get; set; }

    public string SmtpPassword { get; set; }

    public string PopServer { get; set; }

    public int PopPort { get; set; }

    public string PopUsername { get; set; }

    public string PopPassword { get; set; }

    public string FromAddress { get; set; }

    public string FromAddressName { get; set; }

    public int SendInterval { get; set; }

    public int SendRetryLimit { get; set; }

    public int Timeout { get; set; }

    public bool UseSSL { get; set; }

    public bool UseAuth { get; set; }
  }
}
