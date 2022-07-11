using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Configuration list
    /// </summary>
    [Table("Config")]
    [Index(nameof(CategoryId), Name = "icategory_id_Config")]
    public partial class Config
    {
        public Config()
        {
            DeviceConfigs = new HashSet<DeviceConfig>();
        }

        [Key]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("default_value")]
        public string DefaultValue { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(ConfigCategory.Configs))]
        public virtual ConfigCategory Category { get; set; }
        [InverseProperty(nameof(DeviceConfig.Config))]
        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
    }
}
