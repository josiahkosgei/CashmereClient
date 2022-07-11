using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PermissionPolicyTypePermissionsObject")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_PermissionPolicyTypePermissionsObject")]
    [Index(nameof(Role), Name = "iRole_PermissionPolicyTypePermissionsObject")]
    public partial class PermissionPolicyTypePermissionsObject
    {
        public PermissionPolicyTypePermissionsObject()
        {
            PermissionPolicyMemberPermissionsObjects = new HashSet<PermissionPolicyMemberPermissionsObject>();
            PermissionPolicyObjectPermissionsObjects = new HashSet<PermissionPolicyObjectPermissionsObject>();
        }

        [Key]
        public Guid Oid { get; set; }
        public Guid? Role { get; set; }
        public string TargetType { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public int? CreateState { get; set; }
        public int? DeleteState { get; set; }
        public int? NavigateState { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(Role))]
        [InverseProperty(nameof(PermissionPolicyRole.PermissionPolicyTypePermissionsObjects))]
        public virtual PermissionPolicyRole RoleNavigation { get; set; }
        [InverseProperty(nameof(PermissionPolicyMemberPermissionsObject.TypePermissionObjectNavigation))]
        public virtual ICollection<PermissionPolicyMemberPermissionsObject> PermissionPolicyMemberPermissionsObjects { get; set; }
        [InverseProperty(nameof(PermissionPolicyObjectPermissionsObject.TypePermissionObjectNavigation))]
        public virtual ICollection<PermissionPolicyObjectPermissionsObject> PermissionPolicyObjectPermissionsObjects { get; set; }
    }
}
