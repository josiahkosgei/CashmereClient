using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationUserChangePassword")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_ApplicationUserChangePassword")]
    [Index(nameof(PasswordPolicy), Name = "iPasswordPolicy_ApplicationUserChangePassword")]
    [Index(nameof(User), Name = "iUser_ApplicationUserChangePassword")]
    public partial class ApplicationUserChangePassword
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
        public Guid? User { get; set; }
        public Guid? PasswordPolicy { get; set; }

        [ForeignKey(nameof(PasswordPolicy))]
        [InverseProperty("ApplicationUserChangePasswords")]
        public virtual PasswordPolicy PasswordPolicyNavigation { get; set; }
        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(ApplicationUser.ApplicationUserChangePasswords))]
        public virtual ApplicationUser UserNavigation { get; set; }
    }
}
