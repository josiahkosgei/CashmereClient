using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// contains a crash report
    /// </summary>
    [Table("CrashEvent", Schema = "exp")]
    [Index(nameof(DeviceId), Name = "idevice_id_exp_CrashEvent_7BCB8F5E")]
    public partial class CrashEvent1
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
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
