using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("CrashEvent", Schema = "bak")]
    public partial class CrashEvent
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("date_detected")]
        public DateTime DateDetected { get; set; }
        [Required]
        [Column("content")]
        public string Content { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("CrashEvents")]
        public virtual Device Device { get; set; }
    }
}
