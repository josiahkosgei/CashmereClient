
// Type: CashmereDeposit.DeviceLogin


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DeviceLogin
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime LoginDate { get; set; }

        public DateTime? LogoutDate { get; set; }
        [ForeignKey("ApplicationUser")]
        public Guid UserId { get; set; }

        public bool? Success { get; set; }

        public bool? DepositorEnabled { get; set; }

        public bool? ChangePassword { get; set; }

        public string Message { get; set; }

        public bool? ForcedLogout { get; set; }

        [ForeignKey("Device")]
        public Guid DeviceId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Device Device { get; set; }
    }
}
