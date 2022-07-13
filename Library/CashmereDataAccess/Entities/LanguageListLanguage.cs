﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("LanguageList_Language")]
    //// [Index("LanguageItem", "LanguageList", Name = "UX_LanguageList_Language_LanguageItem", IsUnique = true)]
    //// [Index("LanguageList", "LanguageOrder", Name = "UX_LanguageList_Language_Order", IsUnique = true)]
    //// [Index("LanguageItem", Name = "ilanguage_item_LanguageList_Language")]
    //// [Index("LanguageList", Name = "ilanguage_list_LanguageList_Language")]
    public partial class LanguageListLanguage
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("language_list")]
        public int LanguageList { get; set; }
        [Column("language_item")]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageItem { get; set; } = null!;
        [Column("language_order")]
        public int LanguageOrder { get; set; }

        [ForeignKey("LanguageItem")]
       //  //[InverseProperty("LanguageListLanguages")]
        public virtual Language LanguageItemNavigation { get; set; } = null!;
        [ForeignKey("LanguageList")]
       //  //[InverseProperty("LanguageListLanguages")]
        public virtual LanguageList LanguageListNavigation { get; set; } = null!;
    }
}