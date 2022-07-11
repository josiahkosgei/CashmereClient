
// Type: CashmereDeposit.AlertEvent


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertEvent
    {
        public AlertEvent()
        {
            AlertEmails = new HashSet<AlertEmail>();
            AlertSMSes = new HashSet<AlertSMS>();
        }

        [Key]
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime Created { get; set; }

        public int AlertTypeId { get; set; }

        public DateTime DateDetected { get; set; }

        public DateTime? DateResolved { get; set; }

        public bool IsResolved { get; set; }

        public bool IsProcessed { get; set; }

        public Guid? AlertEventId { get; set; }

        public bool IsProcessing { get; set; }

        public string MachineName { get; set; }

        public virtual ICollection<AlertEmail> AlertEmails { get; set; }

        public virtual ICollection<AlertSMS> AlertSMSes { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
