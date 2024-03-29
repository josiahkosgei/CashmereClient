﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("DevicePrinter")]
    [Index("DeviceId", Name = "idevice_id_DevicePrinter")]
    public partial class DevicePrinter
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        [Required]
        [Column("is_infront")]
        public bool? IsInfront { get; set; }
        [Column("port")]
        [StringLength(5)]
        public string Port { get; set; } = null!;
        [Column("make")]
        [StringLength(50)]
        public string? Make { get; set; }
        [Column("model")]
        [StringLength(50)]
        public string? Model { get; set; }
        [Column("serial")]
        [StringLength(50)]
        public string? Serial { get; set; }

        [ForeignKey("DeviceId")]
        [InverseProperty("DevicePrinters")]
        public virtual Device Device { get; set; } = null!;
    }
}