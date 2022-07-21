
using System.Collections.Generic;

namespace Cashmere.API.Messaging.Communication.Emails
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }

        public List<EmailAddress> ToAddresses { get; set; }

        public List<EmailAddress> FromAddresses { get; set; }

        public string Subject { get; set; }

        public string HTMLContent { get; set; }

        public string RawContent { get; set; }

        public List<EmailAttachment> Attachments { get; set; }
    }
}
