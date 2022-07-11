
// Type: CashmereDeposit.LanguageList_Language

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class LanguageListLanguage
    {
        [Key]
        public Guid Id { get; set; }

        public int LanguageListId { get; set; }

        public string LanguageItemId { get; set; }

        public int LanguageOrder { get; set; }

        [ForeignKey(nameof(LanguageItemId))]
        public virtual Language LanguageItem { get; set; }
        [ForeignKey(nameof(LanguageListId))]
        public virtual LanguageList LanguageList { get; set; }
    }
}
