using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// AlertSmses
    /// </summary>
    [Table("AlertSMS")]
    [Index(nameof(AlertEventId), Name = "ialert_event_id_AlertSMS")]
    public partial class AlertSm1
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Datetime when the SMS alert message was created by the system
        /// </summary>
        [Column("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// the number from which the SMS originates
        /// </summary>
        [Column("from")]
        [StringLength(100)]
        public string From { get; set; }

        /// <summary>
        /// Pipe delimited list of phone numbers to receive SMSes
        /// </summary>
        [Column("to")]
        public string To { get; set; }

        /// <summary>
        /// the SMS text message to deliver
        /// </summary>
        [Column("message")]
        public string Message { get; set; }

        /// <summary>
        /// whether or not the SMS message was processed
        /// </summary>
        [Column("sent")]
        public bool Sent { get; set; }

        /// <summary>
        /// the datetime when the SMS message was processed
        /// </summary>
        [Column("send_date")]
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// the associated AlertEvent for this SMS message
        /// </summary>
        [Column("alert_event_id")]
        public Guid AlertEventId { get; set; }

        /// <summary>
        /// was there a fatal rror during processing?
        /// </summary>
        [Column("send_error")]
        public bool SendError { get; set; }

        /// <summary>
        /// error mssage returned by the system while processing the SMS message
        /// </summary>
        [Column("send_error_message")]
        [StringLength(255)]
        public string SendErrorMessage { get; set; }

        [ForeignKey(nameof(AlertEventId))]
        [InverseProperty(nameof(AlertEvent1.AlertSm1s))]
        public virtual AlertEvent1 AlertEvent { get; set; }
    }
}
