
// Type: CashmereDeposit.DevicePrinter


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DevicePrinter
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Device")]
        public Guid DeviceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInfront { get; set; }

        public string Port { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Serial { get; set; }

        public virtual Device Device { get; set; }
    }
}
