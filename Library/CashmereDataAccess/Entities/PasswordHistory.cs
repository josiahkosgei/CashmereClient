
// Type: CashmereDeposit.PasswordHistory

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PasswordHistory
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime? LogDate { get; set; }

        public Guid? ApplicationUserId { get; set; }

        public string Password { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
