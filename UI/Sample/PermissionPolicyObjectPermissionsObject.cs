using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PermissionPolicyObjectPermissionsObject")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_PermissionPolicyObjectPermissionsObject")]
    [Index(nameof(TypePermissionObject), Name = "iTypePermissionObject_PermissionPolicyObjectPermissionsObject")]
    public partial class PermissionPolicyObjectPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string Criteria { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public int? DeleteState { get; set; }
        public int? NavigateState { get; set; }
        public Guid? TypePermissionObject { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(TypePermissionObject))]
        [InverseProperty(nameof(PermissionPolicyTypePermissionsObject.PermissionPolicyObjectPermissionsObjects))]
        public virtual PermissionPolicyTypePermissionsObject TypePermissionObjectNavigation { get; set; }
    }
}
