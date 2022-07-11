using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("UserLock")]
    [Index(nameof(ApplicationUserLoginDetail), Name = "iApplicationUserLoginDetail_UserLock")]
    [Index(nameof(InitiatingUser), Name = "iInitiatingUser_UserLock")]
    public partial class UserLock
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public int? LockType { get; set; }
        public bool? WebPortalInitiated { get; set; }
        public Guid? InitiatingUser { get; set; }

        [ForeignKey(nameof(ApplicationUserLoginDetail))]
        [InverseProperty("UserLocks")]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetailNavigation { get; set; }
        [ForeignKey(nameof(InitiatingUser))]
        [InverseProperty(nameof(ApplicationUser.UserLocks))]
        public virtual ApplicationUser InitiatingUserNavigation { get; set; }
    }
}
