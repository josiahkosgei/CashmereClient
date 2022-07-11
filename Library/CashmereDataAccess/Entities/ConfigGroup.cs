
// Type: CashmereDeposit.ConfigGroup


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class ConfigGroup
    {
        public ConfigGroup()
        {
            ParentGroups = new HashSet<ConfigGroup>();
            DeviceConfigs = new HashSet<DeviceConfig>();
            Devices = new HashSet<Device>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentGroupId { get; set; }

        [ForeignKey(nameof(ParentGroupId))]
        public virtual ConfigGroup ParentGroup { get; set; }

        public virtual ICollection<ConfigGroup> ParentGroups { get; set; }
        public virtual ICollection<DeviceConfig> DeviceConfigs { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
    }
}
