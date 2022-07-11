using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("sysTextItem", Schema = "xlns")]
    [Index(nameof(Token), Name = "UX_SysTextItem_name", IsUnique = true)]
    [Index(nameof(Category), Name = "iCategory_xlns_sysTextItem_A264365A")]
    [Index(nameof(TextItemTypeId), Name = "iTextItemTypeID_xlns_sysTextItem_BD18CE82")]
    public partial class SysTextItem
    {
        public SysTextItem()
        {
            SysTextTranslations = new HashSet<SysTextTranslation>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Token { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public string DefaultTranslation { get; set; }
        public Guid Category { get; set; }
        [Column("TextItemTypeID")]
        public Guid? TextItemTypeId { get; set; }

        [ForeignKey(nameof(Category))]
        [InverseProperty(nameof(SysTextItemCategory.SysTextItems))]
        public virtual SysTextItemCategory CategoryNavigation { get; set; }
        [ForeignKey(nameof(TextItemTypeId))]
        [InverseProperty(nameof(SysTextItemType.SysTextItems))]
        public virtual SysTextItemType TextItemType { get; set; }
        [InverseProperty(nameof(SysTextTranslation.SysTextItem))]
        public virtual ICollection<SysTextTranslation> SysTextTranslations { get; set; }
    }
}
