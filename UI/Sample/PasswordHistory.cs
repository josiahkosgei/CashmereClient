using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PasswordHistory")]
    [Index(nameof(User), Name = "iUser_PasswordHistory")]
    public partial class PasswordHistory
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogDate { get; set; }
        public Guid? User { get; set; }
        [StringLength(71)]
        public string Password { get; set; }

        [ForeignKey(nameof(User))]
        [InverseProperty(nameof(ApplicationUser.PasswordHistories))]
        public virtual ApplicationUser UserNavigation { get; set; }
    }
}
