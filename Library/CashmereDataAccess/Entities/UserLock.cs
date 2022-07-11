using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class UserLock
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? LogDate { get; set; }
        public Guid? ApplicationUserLoginDetailId { get; set; }
        public int? LockType { get; set; }
        public bool? WebPortalInitiated { get; set; }
        public Guid? InitiatingUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserLoginDetailId))]
        public virtual ApplicationUserLoginDetail ApplicationUserLoginDetail { get; set; }

        [ForeignKey(nameof(InitiatingUserId))] 
        public virtual ApplicationUser InitiatingUser { get; set; }
    }
}
