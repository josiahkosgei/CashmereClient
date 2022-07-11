using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Link a Device to its configuration
    /// </summary>
    [Table("DeviceConfig")]
    [Index(nameof(ConfigId), nameof(GroupId), Name = "UX_DeviceConfig", IsUnique = true)]
    [Index(nameof(ConfigId), Name = "iconfig_id_DeviceConfig")]
    [Index(nameof(GroupId), Name = "igroup_id_DeviceConfig")]
    public partial class DeviceConfig
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("group_id")]
        public int GroupId { get; set; }
        [Required]
        [Column("config_id")]
        [StringLength(50)]
        public string ConfigId { get; set; }
        [Required]
        [Column("config_value")]
        public string ConfigValue { get; set; }

        [ForeignKey(nameof(ConfigId))]
        [InverseProperty("DeviceConfigs")]
        public virtual Config Config { get; set; }
        [ForeignKey(nameof(GroupId))]
        [InverseProperty(nameof(ConfigGroup.DeviceConfigs))]
        public virtual ConfigGroup Group { get; set; }
    }
}
