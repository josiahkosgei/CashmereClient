
// Type: CashmereDeposit.ApplicationLog


using System.ComponentModel.DataAnnotations;
using Cashmere.Library.Standard.Utilities;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class ApplicationLog
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? SessionId { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime LogDate { get; set; }

        public string EventName { get; set; }

        public string EventDetail { get; set; }

        public string EventType { get; set; }

        public string Component { get; set; }

        public int LogLevel { get; set; }

        public string MachineName { get; set; }
        
        public virtual Device Device { get; set; }
        public virtual DepositorSession Session { get; set; }
    }
}
