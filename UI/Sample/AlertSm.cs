using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("AlertSMS", Schema = "bak")]
    public partial class AlertSm
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
        [Column("message")]
        public string Message { get; set; }
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
