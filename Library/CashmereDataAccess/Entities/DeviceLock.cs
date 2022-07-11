
// Type: CashmereDeposit.DeviceLock


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DeviceLock
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Device")]
        public Guid DeviceId { get; set; }

        public DateTime LockDate { get; set; }

        public bool Locked { get; set; }

        public Guid? LockingUser { get; set; }

        public string WebLockingUser { get; set; }

        public bool LockedByDevice { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Device Device { get; set; }
    }
}
