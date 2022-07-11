using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("sysTextTranslation", Schema = "xlns")]
    [Index(nameof(LanguageCode), nameof(SysTextItemId), Name = "UX_Translation_Language_Pair", IsUnique = true)]
    [Index(nameof(LanguageCode), Name = "iLanguageCode_xlns_sysTextTranslation_03BB080F")]
    [Index(nameof(SysTextItemId), Name = "iSysTextItemID_xlns_sysTextTranslation_7FDC4652")]
    public partial class SysTextTranslation
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("SysTextItemID")]
        public Guid SysTextItemId { get; set; }
        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; }
        [Required]
        public string TranslationSysText { get; set; }

        [ForeignKey(nameof(LanguageCode))]
        [InverseProperty(nameof(Language.SysTextTranslations))]
        public virtual Language LanguageCodeNavigation { get; set; }
        [ForeignKey(nameof(SysTextItemId))]
        [InverseProperty("SysTextTranslations")]
        public virtual SysTextItem SysTextItem { get; set; }
    }
}
