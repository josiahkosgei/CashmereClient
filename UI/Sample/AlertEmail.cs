using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("AlertEmail", Schema = "bak")]
    public partial class AlertEmail
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("from")]
        [StringLength(100)]
        public string From { get; set; }
        [Column("to")]
        public string To { get; set; }
        [Required]
        [Column("subject")]
        [StringLength(100)]
        public string Subject { get; set; }
        [Column("attachments")]
        public string Attachments { get; set; }
        [Column("html_message")]
        public string HtmlMessage { get; set; }
        [Column("raw_text_message")]
        public string RawTextMessage { get; set; }
        [Column("sent")]
        public bool Sent { get; set; }
        [Column("send_date")]
        public DateTime? SendDate { get; set; }
        [Column("alert_event_id")]
        public Guid AlertEventId { get; set; }
        [Column("send_error")]
        public bool SendError { get; set; }
        [Column("send_error_message")]
        [StringLength(255)]
        public string SendErrorMessage { get; set; }
    }
}
