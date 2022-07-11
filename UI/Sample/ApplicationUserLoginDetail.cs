using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationUserLoginDetail")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_ApplicationUserLoginDetail")]
    [Index(nameof(LastLoginLogEntry), Name = "iLastLoginLogEntry_ApplicationUserLoginDetail")]
    [Index(nameof(User), Name = "iUser_ApplicationUserLoginDetail")]
    public partial class ApplicationUserLoginDetail
    {
        public ApplicationUserLoginDetail()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            UserLocks = new HashSet<UserLock>();
            WebPortalLogins = new HashSet<WebPortalLogin>();
        }

        [Key]
        public Guid Oid { get; set; }
        public Guid? User { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastPasswordDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastLoginDate { get; set; }
        public Guid? LastLoginLogEntry { get; set; }
        public int? FailedLoginCount { get; set; }
        [Column("OTP")]
        [StringLength(100)]
        public string Otp { get; set; }
        [Column("OTPExpire", TypeName = "datetime")]
        public DateTime? Otpexpire { get; set; }
        [Column("OTPEnabled")]
        [StringLength(100)]
        public string Otpenabled { get; set; }
        [StringLength(128)]
        public string ResetEmailCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ResetEmailExpire { get; set; }
        public bool? ResetEmailEnabled { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(LastLoginLogEntry))]
        [InverseProperty(nameof(WebPortalLogin.ApplicationUserLoginDetails))]
        public virtual WebPortalLogin LastLoginLogEntryNavigation { get; set; }
        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(ApplicationUser.ApplicationUserLoginDetails))]
        public virtual ApplicationUser UserNavigation { get; set; }
        [InverseProperty(nameof(ApplicationUser.ApplicationUserLoginDetailNavigation))]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        [InverseProperty(nameof(UserLock.ApplicationUserLoginDetailNavigation))]
        public virtual ICollection<UserLock> UserLocks { get; set; }
        [InverseProperty(nameof(WebPortalLogin.ApplicationUserLoginDetailNavigation))]
        public virtual ICollection<WebPortalLogin> WebPortalLogins { get; set; }
    }
}
