
// Type: CashmereDeposit.Config


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Config
    {
        public Config()
        {
            DeviceConfigs = new HashSet<DeviceConfig>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public string? Description { get; set; }

        [ForeignKey("ConfigCategory")]
        public Guid CategoryId { get; set; }

        public virtual ConfigCategory ConfigCategory { get; set; }

        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
    }
}
