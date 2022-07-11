
// Type: CashmereDeposit.Role

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Role
    {
        public Role()
        {
            Permissions = new HashSet<Permission>();
            ApplicationUsers = new HashSet<ApplicationUser>();
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
    }
}
