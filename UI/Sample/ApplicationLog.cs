using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationLog", Schema = "bak")]
    public partial class ApplicationLog
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("session_id")]
        public Guid? SessionId { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("log_date")]
        public DateTime LogDate { get; set; }
        [Required]
        [Column("event_name")]
        [StringLength(50)]
        [Unicode(false)]
        public string EventName { get; set; }
        [Required]
        [Column("event_detail", TypeName = "text")]
        public string EventDetail { get; set; }
        [Required]
        [Column("event_type")]
        [StringLength(50)]
        [Unicode(false)]
        public string EventType { get; set; }
        [Required]
        [Column("component")]
        [StringLength(50)]
        [Unicode(false)]
        public string Component { get; set; }
        [Column("log_level")]
        public int LogLevel { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("ApplicationLogs")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(SessionId))]
        [InverseProperty(nameof(DepositorSession.ApplicationLogs))]
        public virtual DepositorSession Session { get; set; }
    }
}
