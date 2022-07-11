using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIPrepopItem")]
    [Index(nameof(Value), Name = "iValue_GUIPrepopItem")]
    public partial class GuiprepopItem
    {
        public GuiprepopItem()
        {
            GuiprepopListItems = new HashSet<GuiprepopListItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }
        [Column("value")]
        public Guid Value { get; set; }
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }

        [ForeignKey(nameof(Value))]
        [InverseProperty(nameof(TextItem.GuiprepopItems))]
        public virtual TextItem ValueNavigation { get; set; }
        [InverseProperty(nameof(GuiprepopListItem.ItemNavigation))]
        public virtual ICollection<GuiprepopListItem> GuiprepopListItems { get; set; }
    }
}
