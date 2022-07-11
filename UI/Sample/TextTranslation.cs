using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TextTranslation", Schema = "xlns")]
    [Index(nameof(LanguageCode), nameof(TextItemId), Name = "UX_UI_Translation_Language_Pair", IsUnique = true)]
    [Index(nameof(LanguageCode), Name = "iLanguageCode_xlns_TextTranslation_0EDE47B6")]
    [Index(nameof(TextItemId), Name = "iTextItemID_xlns_TextTranslation_00B6C5AC")]
    public partial class TextTranslation
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("TextItemID")]
        public Guid TextItemId { get; set; }
        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; }
        [Required]
        public string TranslationText { get; set; }

        [ForeignKey(nameof(LanguageCode))]
        [InverseProperty(nameof(Language.TextTranslations))]
        public virtual Language LanguageCodeNavigation { get; set; }
        [ForeignKey(nameof(TextItemId))]
        [InverseProperty("TextTranslations")]
        public virtual TextItem TextItem { get; set; }
    }
}
