
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PasswordPolicy
    {

        public PasswordPolicy()
        {
            ApplicationUserChangePasswords = new HashSet<ApplicationUserChangePassword>();
        }

        [Key]
        public Guid Id { get; set; }
        public int MinLength { get; set; }
        public int MinLowercase { get; set; }
        public int MinDigits { get; set; }
        public int MinUppercase { get; set; }
        public int MinSpecial { get; set; }
        [Required]
        [StringLength(100)]
        public string AllowedSpecial { get; set; }
        public int ExpiryDays { get; set; }
        public int HistorySize { get; set; }
        [Required]
        public bool UseHistory { get; set; }

        public virtual ICollection<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
    }
}
