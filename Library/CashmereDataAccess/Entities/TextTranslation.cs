
// Type: CashmereDeposit.TextTranslation

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TextTranslation
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TextItemId { get; set; }

        public string LanguageCode { get; set; }

        public string TranslationText { get; set; }

        [ForeignKey(nameof(LanguageCode))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(TextItemId))]
        public virtual TextItem TextItem { get; set; }
    }
}
