using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Available languages in the system
    /// </summary>
    [Table("Language")]
    public partial class Language
    {
        public Language()
        {
            DepositorSessions = new HashSet<DepositorSession>();
            LanguageListLanguages = new HashSet<LanguageListLanguage>();
            LanguageLists = new HashSet<LanguageList>();
            SysTextTranslations = new HashSet<SysTextTranslation>();
            TextTranslations = new HashSet<TextTranslation>();
        }

        [Key]
        [Column("code")]
        [StringLength(5)]
        [Unicode(false)]
        public string Code { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        /// <summary>
        /// two character country code for the national flag to display for the language
        /// </summary>
        [Required]
        [Column("flag")]
        [StringLength(255)]
        public string Flag { get; set; }

        /// <summary>
        /// whether the system supports the language
        /// </summary>
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(DepositorSession.LanguageCodeNavigation))]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        [InverseProperty(nameof(LanguageListLanguage.LanguageItemNavigation))]
        public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }
        [InverseProperty(nameof(LanguageList.DefaultLanguageNavigation))]
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
        [InverseProperty(nameof(SysTextTranslation.LanguageCodeNavigation))]
        public virtual ICollection<SysTextTranslation> SysTextTranslations { get; set; }
        [InverseProperty(nameof(TextTranslation.LanguageCodeNavigation))]
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
    }
}
