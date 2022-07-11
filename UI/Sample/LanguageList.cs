using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// A list of languages a device supports
    /// </summary>
    [Table("LanguageList")]
    [Index(nameof(DefaultLanguage), Name = "idefault_language_LanguageList")]
    public partial class LanguageList
    {
        public LanguageList()
        {
            Devices = new HashSet<Device>();
            LanguageListLanguages = new HashSet<LanguageListLanguage>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(100)]
        [Unicode(false)]
        public string Description { get; set; }
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }
        [Required]
        [Column("default_language")]
        [StringLength(5)]
        [Unicode(false)]
        public string DefaultLanguage { get; set; }

        [ForeignKey(nameof(DefaultLanguage))]
        [InverseProperty(nameof(Language.LanguageLists))]
        public virtual Language DefaultLanguageNavigation { get; set; }
        [InverseProperty(nameof(Device.LanguageListNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(LanguageListLanguage.LanguageListNavigation))]
        public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }
    }
}
