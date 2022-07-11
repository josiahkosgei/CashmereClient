using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Categorisation of configuration opions
    /// </summary>
    [Table("ConfigCategory")]
    public partial class ConfigCategory
    {
        public ConfigCategory()
        {
            Configs = new HashSet<Config>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the AlertMessage
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty(nameof(Config.Category))]
        public virtual ICollection<Config> Configs { get; set; }
    }
}
