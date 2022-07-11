using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// [m2m] LanguageList and Language
    /// </summary>
    [Table("LanguageList_Language")]
    [Index(nameof(LanguageItem), nameof(LanguageList), Name = "UX_LanguageList_Language_LanguageItem", IsUnique = true)]
    [Index(nameof(LanguageList), nameof(LanguageOrder), Name = "UX_LanguageList_Language_Order", IsUnique = true)]
    [Index(nameof(LanguageItem), Name = "ilanguage_item_LanguageList_Language")]
    [Index(nameof(LanguageList), Name = "ilanguage_list_LanguageList_Language")]
    public partial class LanguageListLanguage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("language_list")]
        public int LanguageList { get; set; }
        [Required]
        [Column("language_item")]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageItem { get; set; }
        [Column("language_order")]
        public int LanguageOrder { get; set; }

        [ForeignKey(nameof(LanguageItem))]
        [InverseProperty(nameof(Language.LanguageListLanguages))]
        public virtual Language LanguageItemNavigation { get; set; }
        [ForeignKey(nameof(LanguageList))]
        [InverseProperty("LanguageListLanguages")]
        public virtual LanguageList LanguageListNavigation { get; set; }
    }
}
