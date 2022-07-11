
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertEmail
    {
         [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Datetime when the email message was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Email address of the sender
        /// </summary>
        [StringLength(100)]
        public string From { get; set; }

        /// <summary>
        /// Fills the &quot;To:&quot; heading of the email
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Subject of the email
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        /// <summary>
        /// Pipe delimited list of filenames for files to attach when sending. Files must be accessible from the server
        /// </summary>
        public string Attachments { get; set; }

        /// <summary>
        /// The HTML formatted message
        /// </summary>
        public string HtmlMessage { get; set; }

        /// <summary>
        /// The raw ANSI text version of the email for clients that do not support HTML emails e.g. mobile phones etc
        /// </summary>
        public string RawTextMessage { get; set; }

        /// <summary>
        /// Whether or not the email message has been processed by the server
        /// </summary>
        public bool Sent { get; set; }

        /// <summary>
        /// Datetime when the email message was processed by the server
        /// </summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// Corresponding Alert that is tied to this email message
        /// </summary>
        public Guid AlertEventId { get; set; }

        /// <summary>
        /// Was there a fatal error during processing this email message
        /// </summary>
        public bool SendError { get; set; }

        /// <summary>
        /// Error message returned by the server when email sending failed
        /// </summary>
        [StringLength(255)]
        public string SendErrorMessage { get; set; }

        [ForeignKey(nameof(AlertEventId))]
        public virtual AlertEvent AlertEvent { get; set; }
    }
}
