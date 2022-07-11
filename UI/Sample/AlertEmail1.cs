﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores emails sent by the system
    /// </summary>
    [Table("AlertEmail")]
    [Index(nameof(AlertEventId), Name = "ialert_event_id_AlertEmail")]
    public partial class AlertEmail1
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Datetime when the email message was created
        /// </summary>
        [Column("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// Email address of the sender
        /// </summary>
        [Column("from")]
        [StringLength(100)]
        public string From { get; set; }

        /// <summary>
        /// Fills the &quot;To:&quot; heading of the email
        /// </summary>
        [Column("to")]
        public string To { get; set; }

        /// <summary>
        /// Subject of the email
        /// </summary>
        [Required]
        [Column("subject")]
        [StringLength(100)]
        public string Subject { get; set; }

        /// <summary>
        /// Pipe delimited list of filenames for files to attach when sending. Files must be accessible from the server
        /// </summary>
        [Column("attachments")]
        public string Attachments { get; set; }

        /// <summary>
        /// The HTML formatted message
        /// </summary>
        [Column("html_message")]
        public string HtmlMessage { get; set; }

        /// <summary>
        /// The raw ANSI text version of the email for clients that do not support HTML emails e.g. mobile phones etc
        /// </summary>
        [Column("raw_text_message")]
        public string RawTextMessage { get; set; }

        /// <summary>
        /// Whether or not the email message has been processed by the server
        /// </summary>
        [Column("sent")]
        public bool Sent { get; set; }

        /// <summary>
        /// Datetime when the email message was processed by the server
        /// </summary>
        [Column("send_date")]
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// Corresponding Alert that is tied to this email message
        /// </summary>
        [Column("alert_event_id")]
        public Guid AlertEventId { get; set; }

        /// <summary>
        /// Was there a fatal error during processing this email message
        /// </summary>
        [Column("send_error")]
        public bool SendError { get; set; }

        /// <summary>
        /// Error message returned by the server when email sending failed
        /// </summary>
        [Column("send_error_message")]
        [StringLength(255)]
        public string SendErrorMessage { get; set; }

        [ForeignKey(nameof(AlertEventId))]
        [InverseProperty(nameof(AlertEvent1.AlertEmail1s))]
        public virtual AlertEvent1 AlertEvent { get; set; }
    }
}
