using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PermissionPolicyMemberPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string Members { get; set; }
        public int? ReadState { get; set; }
        public int? WriteState { get; set; }
        public string Criteria { get; set; }
        public Guid? TypePermissionObjectId { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(TypePermissionObjectId))]
        public virtual PermissionPolicyTypePermissionsObject TypePermissionObject { get; set; }
    }
}
