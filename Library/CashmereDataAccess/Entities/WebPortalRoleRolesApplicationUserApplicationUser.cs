using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class WebPortalRoleRolesApplicationUserApplicationUser
    {
        public Guid? RolesId { get; set; }
        [Key]
        public Guid Oid { get; set; }
        public int? OptimisticLockField { get; set; }
        public Guid? ApplicationUsersId { get; set; }

        [ForeignKey(nameof(ApplicationUsersId))]
        public virtual ApplicationUser ApplicationUsers { get; set; }
        [ForeignKey(nameof(RolesId))]
        public virtual WebPortalRole Roles { get; set; }
    }
}
