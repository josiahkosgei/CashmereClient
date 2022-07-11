
// Type: CashmereDeposit.LanguageList


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class LanguageList
  {
    public LanguageList()
    {
      LanguageListLanguages = new HashSet<LanguageListLanguage>();
      Devices = new HashSet<Device>();
    }

    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public string DefaultLanguageId { get; set; }
    
    [ForeignKey(nameof(DefaultLanguageId))]
    public virtual Language DefaultLanguage { get; set; }
    public virtual ICollection<LanguageListLanguage> LanguageListLanguages { get; set; }

    public virtual ICollection<Device> Devices { get; set; }
  }
}
