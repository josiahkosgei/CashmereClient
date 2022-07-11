using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Result of sending an alert email
    /// </summary>
    [Table("AlertEmailResult")]
    public partial class AlertEmailResult
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("alert_email_id")]
        public Guid AlertEmailId { get; set; }
        [Required]
        [Column("status")]
        [StringLength(10)]
        public string Status { get; set; }
        [Column("date_sent")]
        public DateTime? DateSent { get; set; }
        [Column("is_sent")]
        public bool IsSent { get; set; }
        [Required]
        [Column("recipient")]
        [StringLength(50)]
        public string Recipient { get; set; }
        [Column("error")]
        public int Error { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; }
        [Column("is_processed")]
        public bool IsProcessed { get; set; }
        [Column("html_message")]
        public string HtmlMessage { get; set; }
        [Column("raw_text_message")]
        public string RawTextMessage { get; set; }
    }
}
