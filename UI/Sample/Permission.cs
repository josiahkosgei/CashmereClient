using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// grant a role to perform an activity
    /// </summary>
    [Table("Permission")]
    [Index(nameof(RoleId), nameof(ActivityId), Name = "UX_Permission", IsUnique = true)]
    [Index(nameof(ActivityId), Name = "iactivity_id_Permission")]
    [Index(nameof(RoleId), Name = "irole_id_Permission")]
    public partial class Permission
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("role_id")]
        public Guid RoleId { get; set; }
        [Column("activity_id")]
        public Guid ActivityId { get; set; }
        [Column("standalone_allowed")]
        public bool StandaloneAllowed { get; set; }
        [Column("standalone_authentication_required")]
        public bool StandaloneAuthenticationRequired { get; set; }
        [Column("standalone_can_Authenticate")]
        public bool StandaloneCanAuthenticate { get; set; }

        [ForeignKey(nameof(ActivityId))]
        [InverseProperty("Permissions")]
        public virtual Activity Activity { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("Permissions")]
        public virtual Role Role { get; set; }
    }
}
