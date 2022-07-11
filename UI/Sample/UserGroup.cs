using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// groups together users ho have privileges on the same machine
    /// </summary>
    [Table("UserGroup")]
    [Index(nameof(ParentGroup), Name = "iparent_group_UserGroup")]
    public partial class UserGroup
    {
        public UserGroup()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            Devices = new HashSet<Device>();
            InverseParentGroupNavigation = new HashSet<UserGroup>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(512)]
        public string Description { get; set; }
        [Column("parent_group")]
        public int? ParentGroup { get; set; }

        [ForeignKey(nameof(ParentGroup))]
        [InverseProperty(nameof(UserGroup.InverseParentGroupNavigation))]
        public virtual UserGroup ParentGroupNavigation { get; set; }
        [InverseProperty(nameof(ApplicationUser.UserGroupNavigation))]
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        [InverseProperty(nameof(Device.UserGroupNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(UserGroup.ParentGroupNavigation))]
        public virtual ICollection<UserGroup> InverseParentGroupNavigation { get; set; }
    }
}
