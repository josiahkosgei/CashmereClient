using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("WebPortalRoleRoles_ApplicationUserApplicationUsers")]
    [Index(nameof(ApplicationUsers), nameof(Roles), Name = "iApplicationUsersRoles_WebPortalRoleRoles_ApplicationUserApplicationUsers", IsUnique = true)]
    [Index(nameof(ApplicationUsers), Name = "iApplicationUsers_WebPortalRoleRoles_ApplicationUserApplicationUsers")]
    [Index(nameof(Roles), Name = "iRoles_PermissionPolicyUserUsers_PermissionPolicyRoleRoles")]
    [Index(nameof(Roles), Name = "iRoles_WebPortalRoleRoles_ApplicationUserApplicationUsers")]
    public partial class WebPortalRoleRolesApplicationUserApplicationUser
    {
        public Guid? Roles { get; set; }
        [Key]
        [Column("OID")]
        public Guid Oid { get; set; }
        public int? OptimisticLockField { get; set; }
        public Guid? ApplicationUsers { get; set; }

        [ForeignKey(nameof(ApplicationUsers))]
        [InverseProperty(nameof(ApplicationUser.WebPortalRoleRolesApplicationUserApplicationUsers))]
        public virtual ApplicationUser ApplicationUsersNavigation { get; set; }
        [ForeignKey(nameof(Roles))]
        [InverseProperty(nameof(WebPortalRole.WebPortalRoleRolesApplicationUserApplicationUsers))]
        public virtual WebPortalRole RolesNavigation { get; set; }
    }
}
