using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Group together configurations so devices can share configs
    /// </summary>
    [Table("ConfigGroup")]
    [Index(nameof(ParentGroup), Name = "iparent_group_ConfigGroup")]
    public partial class ConfigGroup
    {
        public ConfigGroup()
        {
            DeviceConfigs = new HashSet<DeviceConfig>();
            Devices = new HashSet<Device>();
            InverseParentGroupNavigation = new HashSet<ConfigGroup>();
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
        [InverseProperty(nameof(ConfigGroup.InverseParentGroupNavigation))]
        public virtual ConfigGroup ParentGroupNavigation { get; set; }
        [InverseProperty(nameof(DeviceConfig.Group))]
        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
        [InverseProperty(nameof(Device.ConfigGroupNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(ConfigGroup.ParentGroupNavigation))]
        public virtual ICollection<ConfigGroup> InverseParentGroupNavigation { get; set; }
    }
}
