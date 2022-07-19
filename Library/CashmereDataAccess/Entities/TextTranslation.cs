﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("TextTranslation", Schema = "xlns")]
    public partial class TextTranslation
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("TextItemID")]
        public Guid TextItemId { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; } = null!;
        public string TranslationText { get; set; } = null!;

        [ForeignKey("LanguageCode")]
        public virtual Language LanguageCodeNavigation { get; set; } = null!;
        [ForeignKey("TextItemId")]
        public virtual TextItem TextItem { get; set; } = null!;
    }
}