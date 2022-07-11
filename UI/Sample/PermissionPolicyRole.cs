using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PermissionPolicyRole")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_PermissionPolicyRole")]
    [Index(nameof(ObjectType), Name = "iObjectType_PermissionPolicyRole")]
    public partial class PermissionPolicyRole
    {
        public PermissionPolicyRole()
        {
            PermissionPolicyNavigationPermissionsObjects = new HashSet<PermissionPolicyNavigationPermissionsObject>();
            PermissionPolicyTypePermissionsObjects = new HashSet<PermissionPolicyTypePermissionsObject>();
        }

        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool? IsAdministrative { get; set; }
        public bool? CanEditModel { get; set; }
        public int? PermissionPolicy { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
        public int? ObjectType { get; set; }

        [ForeignKey(nameof(ObjectType))]
        [InverseProperty(nameof(XpobjectType.PermissionPolicyRoles))]
        public virtual XpobjectType ObjectTypeNavigation { get; set; }
        [InverseProperty("OidNavigation")]
        public virtual WebPortalRole WebPortalRole { get; set; }
        [InverseProperty(nameof(PermissionPolicyNavigationPermissionsObject.RoleNavigation))]
        public virtual ICollection<PermissionPolicyNavigationPermissionsObject> PermissionPolicyNavigationPermissionsObjects { get; set; }
        [InverseProperty(nameof(PermissionPolicyTypePermissionsObject.RoleNavigation))]
        public virtual ICollection<PermissionPolicyTypePermissionsObject> PermissionPolicyTypePermissionsObjects { get; set; }
    }
}
