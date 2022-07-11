using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores the general application log that the GUI and other local systems write to
    /// </summary>
    [Table("ApplicationLog")]
    [Index(nameof(DeviceId), Name = "idevice_id_ApplicationLog")]
    [Index(nameof(SessionId), Name = "isession_id_ApplicationLog")]
    public partial class ApplicationLog1
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The session this log entry belongs to
        /// </summary>
        [Column("session_id")]
        public Guid? SessionId { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Datetime the system deems for the log entry.
        /// </summary>
        [Column("log_date")]
        public DateTime LogDate { get; set; }

        /// <summary>
        /// The name of the log event
        /// </summary>
        [Required]
        [Column("event_name")]
        [StringLength(50)]
        public string EventName { get; set; }

        /// <summary>
        /// the details of the log message
        /// </summary>
        [Required]
        [Column("event_detail")]
        [StringLength(250)]
        public string EventDetail { get; set; }

        /// <summary>
        /// the type of the log event used for grouping and sorting
        /// </summary>
        [Required]
        [Column("event_type")]
        [StringLength(50)]
        public string EventType { get; set; }

        /// <summary>
        /// Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc
        /// </summary>
        [Required]
        [Column("component")]
        [StringLength(50)]
        public string Component { get; set; }

        /// <summary>
        /// the LogLevel
        /// </summary>
        [Column("log_level")]
        public int LogLevel { get; set; }
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
