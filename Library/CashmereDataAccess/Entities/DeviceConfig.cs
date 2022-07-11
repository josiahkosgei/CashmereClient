
// Type: CashmereDeposit.DeviceConfig


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DeviceConfig
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("ConfigGroup")]
        public int GroupId { get; set; }

        [ForeignKey("Config")]
        public Guid ConfigId { get; set; }

        public string ConfigValue { get; set; }

        public virtual Config Config { get; set; }

        public virtual ConfigGroup ConfigGroup { get; set; }
    }
}
