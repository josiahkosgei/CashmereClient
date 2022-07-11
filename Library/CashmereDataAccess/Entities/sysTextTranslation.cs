
// Type: CashmereDeposit.sysTextTranslation

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextTranslation", Schema = "xlns")]
    public class SysTextTranslation
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SysTextItemId { get; set; }

        public string LanguageCode { get; set; }

        public string TranslationSysText { get; set; }
        [ForeignKey(nameof(LanguageCode))]
        public virtual Language Language { get; set; }
        [ForeignKey(nameof(SysTextItemId))]
        public virtual SysTextItem SysTextItem { get; set; }
    }
}
