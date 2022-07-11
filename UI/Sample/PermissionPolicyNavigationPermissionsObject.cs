using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PermissionPolicyNavigationPermissionsObject")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_PermissionPolicyNavigationPermissionsObject")]
    [Index(nameof(Role), Name = "iRole_PermissionPolicyNavigationPermissionsObject")]
    public partial class PermissionPolicyNavigationPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string ItemPath { get; set; }
        public int? NavigateState { get; set; }
        public Guid? Role { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(Role))]
        [InverseProperty(nameof(PermissionPolicyRole.PermissionPolicyNavigationPermissionsObjects))]
        public virtual PermissionPolicyRole RoleNavigation { get; set; }
    }
}
