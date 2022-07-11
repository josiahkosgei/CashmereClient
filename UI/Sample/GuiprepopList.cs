using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIPrepopList")]
    public partial class GuiprepopList
    {
        public GuiprepopList()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
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
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }
        public bool AllowFreeText { get; set; }
        public int DefaultIndex { get; set; }
        [Required]
        public bool? UseDefault { get; set; }

        [InverseProperty(nameof(GuiScreenListScreen.Guiprepoplist))]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        [InverseProperty(nameof(GuiprepopListItem.ListNavigation))]
        public virtual ICollection<GuiprepopListItem> GuiprepopListItems { get; set; }
    }
}
