using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// a task a user needs permission to perform
    /// </summary>
    [Table("Activity")]
    public partial class Activity
    {
        public Activity()
        {
            Permissions = new HashSet<Permission>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the activity. will be used in lookups
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Short description of the activity being performed
        /// </summary>
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty(nameof(Permission.Activity))]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
