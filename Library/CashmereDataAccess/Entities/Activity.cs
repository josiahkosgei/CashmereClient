
// Type: CashmereDeposit.Activity


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Activity
    {
        public Activity()
        {
            Permissions = new HashSet<Permission>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
