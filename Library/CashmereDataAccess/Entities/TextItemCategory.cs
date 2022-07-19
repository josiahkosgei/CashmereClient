using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("TextItemCategory", Schema = "xlns")]
    public partial class TextItemCategory
    {
        public TextItemCategory()
        {
            InverseParentNavigation = new HashSet<TextItemCategory>();
            TextItems = new HashSet<TextItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        public Guid? Parent { get; set; }

        [ForeignKey("Parent")]
        public virtual TextItemCategory? ParentNavigation { get; set; }
        public virtual ICollection<TextItemCategory> InverseParentNavigation { get; set; }
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}