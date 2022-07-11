using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities;

public class ApplicationUserLoginDetail
{
    public ApplicationUserLoginDetail()
    {
        ApplicationUsers = new HashSet<ApplicationUser>();
        UserLocks = new HashSet<UserLock>();
        WebPortalLogins = new HashSet<WebPortalLogin>();
    }

    [Key]
    public Guid Oid { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? LastPasswordDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public Guid? LastLoginLogEntryId { get; set; }
    public int? FailedLoginCount { get; set; }
    [StringLength(100)]
    public string OTP { get; set; }
    public DateTime? OTPExpire { get; set; }
    [StringLength(100)]
    public string OTPEnabled { get; set; }
    [StringLength(128)]
    public string ResetEmailCode { get; set; }
    public DateTime? ResetEmailExpire { get; set; }
    public bool? ResetEmailEnabled { get; set; }
    public int? OptimisticLockField { get; set; }
    public int? GCRecord { get; set; }

    [ForeignKey(nameof(LastLoginLogEntryId))]
    public virtual WebPortalLogin LastLoginLogEntry { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    public virtual ICollection<UserLock> UserLocks { get; set; }
    public virtual ICollection<WebPortalLogin> WebPortalLogins { get; set; }
}
