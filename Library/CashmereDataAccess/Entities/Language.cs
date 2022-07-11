
// Type: CashmereDeposit.Language


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class Language
  {
    public Language()
    {
      LanguageLists = new HashSet<LanguageList>();
      LanguageListLanguages = new HashSet<LanguageListLanguage>();
      SysTextTranslations = new HashSet<SysTextTranslation>();
      TextTranslations = new HashSet<TextTranslation>();
      DepositorSessions = new HashSet<DepositorSession>();
    }

    [Key]
    [StringLength(5)]
    [Unicode(false)]
    public string Code { get; set; }
    public string Name { get; set; }

    public string Flag { get; set; }

    public bool Enabled { get; set; }

    public virtual ICollection<LanguageList> LanguageLists { get; set; }

    public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }

    public virtual ICollection<SysTextTranslation> SysTextTranslations { get; set; }

    public virtual ICollection<TextTranslation> TextTranslations { get; set; }

    public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
  }
}
