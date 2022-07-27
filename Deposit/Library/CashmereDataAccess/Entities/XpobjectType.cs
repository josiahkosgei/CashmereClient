using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{

    public class XpobjectType
    {
        public XpobjectType()
        {
            PermissionPolicyRoles = new HashSet<PermissionPolicyRole>();
        }

        [Key]
        public int Oid { get; set; }
        [StringLength(254)]
        public string TypeName { get; set; }
        [StringLength(254)]
        public string AssemblyName { get; set; }
        public virtual ICollection<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
    }
}
