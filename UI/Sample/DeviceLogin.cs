using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("DeviceLogin")]
    [Index(nameof(User), Name = "iUser_DeviceLogin")]
    public partial class DeviceLogin
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public Guid User { get; set; }
        public bool? Success { get; set; }
        public bool? DepositorEnabled { get; set; }
        public bool? ChangePassword { get; set; }
        [StringLength(200)]
        public string Message { get; set; }
        public bool? ForcedLogout { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("DeviceLogins")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(ApplicationUser.DeviceLogins))]
        public virtual ApplicationUser UserNavigation { get; set; }
    }
}
