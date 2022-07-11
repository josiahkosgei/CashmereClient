using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class PermissionPolicyRole
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
      
        public int? Gcrecord { get; set; }
        public int? ObjectTypeId { get; set; }

        [ForeignKey(nameof(ObjectTypeId))]
        public virtual XpobjectType ObjectType { get; set; }
        public virtual WebPortalRole WebPortalRole { get; set; }
        public virtual ICollection<PermissionPolicyNavigationPermissionsObject> PermissionPolicyNavigationPermissionsObjects { get; set; }
        public virtual ICollection<PermissionPolicyTypePermissionsObject> PermissionPolicyTypePermissionsObjects { get; set; }
    }
}
