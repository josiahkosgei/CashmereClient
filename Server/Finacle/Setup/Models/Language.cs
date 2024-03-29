﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
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
            GuiscreenTexts = new HashSet<GuiscreenText>();
            LanguageListLanguages = new HashSet<LanguageListLanguage>();
            LanguageLists = new HashSet<LanguageList>();
            TransactionTexts = new HashSet<TransactionText>();
            ValidationTexts = new HashSet<ValidationText>();
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

        [InverseProperty("LanguageCodeNavigation")]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        [InverseProperty("Language")]
        public virtual ICollection<GuiscreenText> GuiscreenTexts { get; set; }
        [InverseProperty("LanguageItemNavigation")]
        public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }
        [InverseProperty("DefaultLanguageNavigation")]
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
        [InverseProperty("Language")]
        public virtual ICollection<TransactionText> TransactionTexts { get; set; }
        [InverseProperty("Language")]
        public virtual ICollection<ValidationText> ValidationTexts { get; set; }
    }
}