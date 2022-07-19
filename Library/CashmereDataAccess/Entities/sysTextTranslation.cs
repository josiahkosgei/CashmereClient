using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextTranslation", Schema = "xlns")]
    public partial class SysTextTranslation
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("SysTextItemID")]
        public Guid SysTextItemId { get; set; }
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; } = null!;
        public string TranslationSysText { get; set; } = null!;

        [ForeignKey("LanguageCode")]
        public virtual Language LanguageCodeNavigation { get; set; } = null!;
        [ForeignKey("SysTextItemId")]
        public virtual SysTextItem SysTextItem { get; set; } = null!;
    }
}