using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class ApplicationUserChangePassword
    {
        [Key]
        public Guid Oid { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
        [StringLength(100)]
        public string OldPassword { get; set; }
        [StringLength(100)]
        public string NewPassword { get; set; }
        [StringLength(100)]
        public string ConfirmPassword { get; set; }
        public Guid? UserId { get; set; }
        public Guid? PasswordPolicyId { get; set; }

        [ForeignKey(nameof(PasswordPolicyId))]
        public virtual PasswordPolicy PasswordPolicy { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }
    }
}
