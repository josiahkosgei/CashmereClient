﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("DeviceLock")]
    // [Index("DeviceId", Name = "idevice_DeviceLock")]
    // [Index("LockingUser", Name = "ilocking_user_DeviceLock")]
    public partial class DeviceLock
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("lock_date")]
        public DateTime LockDate { get; set; }
        [Column("locked")]
        public bool Locked { get; set; }
        [Column("locking_user")]
        public Guid? LockingUser { get; set; }
        [Column("web_locking_user")]
        [StringLength(50)]
        public string? WebLockingUser { get; set; }
        [Column("locked_by_device")]
        public bool LockedByDevice { get; set; }

        [ForeignKey("DeviceId")]
        //[InverseProperty("DeviceLocks")]
        public virtual Device Device { get; set; } = null!;
        [ForeignKey("LockingUser")]
        //[InverseProperty("DeviceLocks")]
        public virtual ApplicationUser? LockingUserNavigation { get; set; }
    }
}