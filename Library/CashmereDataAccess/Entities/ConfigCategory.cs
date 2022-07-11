
// Type: CashmereDeposit.ConfigCategory


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ConfigCategory
  {
    public ConfigCategory()
    {
        Configs = new HashSet<Config>();
    }

    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Config> Configs { get; set; }
  }
}
