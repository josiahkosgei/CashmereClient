using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TextItemCategory", Schema = "xlns")]
    [Index(nameof(Name), Name = "UX_UI_TextItemCategory_name", IsUnique = true)]
    [Index(nameof(Parent), Name = "iParent_xlns_TextItemCategory_051D04A6")]
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
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        public Guid? Parent { get; set; }

        [ForeignKey(nameof(Parent))]
        [InverseProperty(nameof(TextItemCategory.InverseParentNavigation))]
        public virtual TextItemCategory ParentNavigation { get; set; }
        [InverseProperty(nameof(TextItemCategory.ParentNavigation))]
        public virtual ICollection<TextItemCategory> InverseParentNavigation { get; set; }
        [InverseProperty(nameof(TextItem.CategoryNavigation))]
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}
