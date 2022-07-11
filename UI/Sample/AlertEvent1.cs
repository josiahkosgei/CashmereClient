using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// An event that has raised an alert. Various messages can be sent based on the alert raised e.g. SMS EMail etc
    /// </summary>
    [Table("AlertEvent")]
    [Index(nameof(AlertEventId), Name = "ialert_event_id_AlertEvent")]
    [Index(nameof(AlertTypeId), Name = "ialert_type_id_AlertEvent")]
    [Index(nameof(DeviceId), Name = "idevice_id_AlertEvent")]
    public partial class AlertEvent1
    {
        public AlertEvent1()
        {
            AlertEmail1s = new HashSet<AlertEmail1>();
            AlertSm1s = new HashSet<AlertSm1>();
            InverseAlertEvent = new HashSet<AlertEvent1>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The Device that raised the alert
        /// </summary>
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// The exact moment the alert was raised
        /// </summary>
        [Column("created")]
        public DateTime Created { get; set; }

        /// <summary>
        /// the type of alert
        /// </summary>
        [Column("alert_type_id")]
        public int AlertTypeId { get; set; }

        /// <summary>
        /// When was the alert detected, in case it is different from the created date. e.g. may indicate the event occured some other time, possibly before it was created in the db
        /// </summary>
        [Column("date_detected")]
        public DateTime DateDetected { get; set; }

        /// <summary>
        /// If tied to another Alert, this is when the the paired Alert was resolved e.g. a door close alert may resolve a previous door open alert
        /// </summary>
        [Column("date_resolved")]
        public DateTime? DateResolved { get; set; }

        /// <summary>
        /// whether the Alert in qustion has been resolved or is still open
        /// </summary>
        [Column("is_resolved")]
        public bool IsResolved { get; set; }

        /// <summary>
        /// has this alert been processed and messages created accordingly
        /// </summary>
        [Column("is_processed")]
        public bool IsProcessed { get; set; }

        /// <summary>
        /// if this alert is paired with a previous alert, it is linked here
        /// </summary>
        [Column("alert_event_id")]
        public Guid? AlertEventId { get; set; }

        /// <summary>
        /// is this alert currently being processed, used for concurrency control
        /// </summary>
        [Column("is_processing")]
        public bool IsProcessing { get; set; }
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }

        [ForeignKey(nameof(AlertEventId))]
        [InverseProperty(nameof(AlertEvent1.InverseAlertEvent))]
        public virtual AlertEvent1 AlertEvent { get; set; }
        [ForeignKey(nameof(AlertTypeId))]
        [InverseProperty(nameof(AlertMessageType.AlertEvent1s))]
        public virtual AlertMessageType AlertType { get; set; }
        [InverseProperty(nameof(AlertEmail1.AlertEvent))]
        public virtual ICollection<AlertEmail1> AlertEmail1s { get; set; }
        [InverseProperty(nameof(AlertSm1.AlertEvent))]
        public virtual ICollection<AlertSm1> AlertSm1s { get; set; }
        [InverseProperty(nameof(AlertEvent1.AlertEvent))]
        public virtual ICollection<AlertEvent1> InverseAlertEvent { get; set; }
    }
}
