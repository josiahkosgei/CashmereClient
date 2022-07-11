using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("AlertEvent", Schema = "bak")]
    public partial class AlertEvent
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("alert_type_id")]
        public int AlertTypeId { get; set; }
        [Column("date_detected")]
        public DateTime DateDetected { get; set; }
        [Column("date_resolved")]
        public DateTime? DateResolved { get; set; }
        [Column("is_resolved")]
        public bool IsResolved { get; set; }
        [Column("is_processed")]
        public bool IsProcessed { get; set; }
        [Column("alert_event_id")]
        public Guid? AlertEventId { get; set; }
        [Column("is_processing")]
        public bool IsProcessing { get; set; }
    }
}
