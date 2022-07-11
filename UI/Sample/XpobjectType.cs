using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("XPObjectType")]
    [Index(nameof(TypeName), Name = "iTypeName_XPObjectType", IsUnique = true)]
    public partial class XpobjectType
    {
        public XpobjectType()
        {
            PermissionPolicyRoles = new HashSet<PermissionPolicyRole>();
        }

        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        [StringLength(254)]
        public string TypeName { get; set; }
        [StringLength(254)]
        public string AssemblyName { get; set; }

        [InverseProperty(nameof(PermissionPolicyRole.ObjectTypeNavigation))]
        public virtual ICollection<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
    }
}
