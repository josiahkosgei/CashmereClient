using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PermissionPolicyMemberPermissionsObject")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_PermissionPolicyMemberPermissionsObject")]
    [Index(nameof(TypePermissionObject), Name = "iTypePermissionObject_PermissionPolicyMemberPermissionsObject")]
    public partial class PermissionPolicyMemberPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string Members { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public string Criteria { get; set; }
        public Guid? TypePermissionObject { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(TypePermissionObject))]
        [InverseProperty(nameof(PermissionPolicyTypePermissionsObject.PermissionPolicyMemberPermissionsObjects))]
        public virtual PermissionPolicyTypePermissionsObject TypePermissionObjectNavigation { get; set; }
    }
}
