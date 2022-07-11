using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PermissionPolicyNavigationPermissionsObject
    {
        [Key]
        public Guid Oid { get; set; }
        public string ItemPath { get; set; }
        public int? NavigateState { get; set; }
        public Guid? RoleId { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual PermissionPolicyRole Role { get; set; }
    }
}
