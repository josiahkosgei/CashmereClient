using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("DevicePrinter")]
    [Index(nameof(DeviceId), Name = "idevice_id_DevicePrinter")]
    public partial class DevicePrinter
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Is the printer in the front i.e. customer facing or in the rear i.e. custodian facing
        /// </summary>
        [Required]
        [Column("is_infront")]
        public bool? IsInfront { get; set; }
        [Required]
        [Column("port")]
        [StringLength(5)]
        public string Port { get; set; }
        [Column("make")]
        [StringLength(50)]
        public string Make { get; set; }
        [Column("model")]
        [StringLength(50)]
        public string Model { get; set; }
        [Column("serial")]
        [StringLength(50)]
        public string Serial { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("DevicePrinters")]
        public virtual Device Device { get; set; }
    }
}
