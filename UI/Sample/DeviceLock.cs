using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Record device locking and unlocking activity
    /// </summary>
    [Table("DeviceLock")]
    [Index(nameof(DeviceId), Name = "idevice_DeviceLock")]
    [Index(nameof(LockingUser), Name = "ilocking_user_DeviceLock")]
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
        public string WebLockingUser { get; set; }
        [Column("locked_by_device")]
        public bool LockedByDevice { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("DeviceLocks")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(LockingUser))]
        [InverseProperty(nameof(ApplicationUser.DeviceLocks))]
        public virtual ApplicationUser LockingUserNavigation { get; set; }
    }
}
