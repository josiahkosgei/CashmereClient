using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PermissionPolicyTypePermissionsObject
    {
        public PermissionPolicyTypePermissionsObject()
        {
            PermissionPolicyMemberPermissionsObjects = new HashSet<PermissionPolicyMemberPermissionsObject>();
            PermissionPolicyObjectPermissionsObjects = new HashSet<PermissionPolicyObjectPermissionsObject>();
        }

        [Key]
        public Guid Oid { get; set; }
        public Guid? RoleId { get; set; }
        public string TargetType { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public int? CreateState { get; set; }
        public int? DeleteState { get; set; }
        public int? NavigateState { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual PermissionPolicyRole Role { get; set; }
        public virtual ICollection<PermissionPolicyMemberPermissionsObject> PermissionPolicyMemberPermissionsObjects { get; set; }
        public virtual ICollection<PermissionPolicyObjectPermissionsObject> PermissionPolicyObjectPermissionsObjects { get; set; }
    }
}
