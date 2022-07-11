﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// The system password policy
    /// </summary>
    [Table("PasswordPolicy")]
    public partial class PasswordPolicy
    {
        public PasswordPolicy()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("min_length")]
        public int MinLength { get; set; }
        [Column("min_lowercase")]
        public int MinLowercase { get; set; }
        [Column("min_digits")]
        public int MinDigits { get; set; }
        [Column("min_uppercase")]
        public int MinUppercase { get; set; }
        [Column("min_special")]
        public int MinSpecial { get; set; }
        [Required]
        [Column("allowed_special")]
        [StringLength(100)]
        public string AllowedSpecial { get; set; }
        [Column("expiry_days")]
        public int ExpiryDays { get; set; }
        [Column("history_size")]
        public int HistorySize { get; set; }
        [Required]
        [Column("use_history")]
        public bool? UseHistory { get; set; }

        [InverseProperty(nameof(ApplicationUserChangePassword.PasswordPolicyNavigation))]
        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
    }
}
