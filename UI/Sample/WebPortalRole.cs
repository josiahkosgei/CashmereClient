using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("WebPortalRole")]
    public partial class WebPortalRole
    {
        public WebPortalRole()
        {
            WebPortalRoleRolesApplicationUserApplicationUsers = new HashSet<WebPortalRoleRolesApplicationUserApplicationUser>();
        }

        [Key]
        public Guid Oid { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        [ForeignKey(nameof(Oid))]
        [InverseProperty(nameof(PermissionPolicyRole.WebPortalRole))]
        public virtual PermissionPolicyRole OidNavigation { get; set; }
        [InverseProperty(nameof(WebPortalRoleRolesApplicationUserApplicationUser.RolesNavigation))]
        public virtual ICollection<WebPortalRoleRolesApplicationUserApplicationUser> WebPortalRoleRolesApplicationUserApplicationUsers { get; set; }
    }
}
