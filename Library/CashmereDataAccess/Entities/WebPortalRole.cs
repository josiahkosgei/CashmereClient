using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class WebPortalRole
    {
        public WebPortalRole()
        {
            WebPortalRoleRolesApplicationUserApplicationUsers = new HashSet<WebPortalRoleRolesApplicationUserApplicationUser>();
        }

        [Key]
        public Guid Oid { get; set; }
        [StringLength(255)]
        public string Description { get; set; }

        [ForeignKey(nameof(Oid))]
        public virtual PermissionPolicyRole PermissionPolicyRole { get; set; }
        public virtual ICollection<WebPortalRoleRolesApplicationUserApplicationUser> WebPortalRoleRolesApplicationUserApplicationUsers { get; set; }
    }
}
