using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("sysTextItemCategory", Schema = "xlns")]
    [Index(nameof(Name), Name = "UX_TextItemCategory_name", IsUnique = true)]
    [Index(nameof(Parent), Name = "iParent_xlns_sysTextItemCategory_51488F7B")]
    public partial class SysTextItemCategory
    {
        public SysTextItemCategory()
        {
            InverseParentNavigation = new HashSet<SysTextItemCategory>();
            SysTextItems = new HashSet<SysTextItem>();
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
        [InverseProperty(nameof(SysTextItemCategory.InverseParentNavigation))]
        public virtual SysTextItemCategory ParentNavigation { get; set; }
        [InverseProperty(nameof(SysTextItemCategory.ParentNavigation))]
        public virtual ICollection<SysTextItemCategory> InverseParentNavigation { get; set; }
        [InverseProperty(nameof(SysTextItem.CategoryNavigation))]
        public virtual ICollection<SysTextItem> SysTextItems { get; set; }
    }
}
