using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// a user&apos;s role storing all their permissions
    /// </summary>
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
            ApplicationUsers = new HashSet<ApplicationUser>();
            Permissions = new HashSet<Permission>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(512)]
        public string Description { get; set; }

        [InverseProperty(nameof(AlertMessageRegistry.Role))]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        [InverseProperty(nameof(ApplicationUser.Role))]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        [InverseProperty(nameof(Permission.Role))]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
