
// Type: CashmereDeposit.CrashEvent


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("CrashEvent", Schema = "exp")]
    public class CrashEvent
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime Datetime { get; set; }

        public DateTime DateDetected { get; set; }

        public string Content { get; set; }

        public string MachineName { get; set; }
        public virtual Device Device { get; set; }
    }
}
