
// Type: CashmereDeposit.AlertSM


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("AlertSMS")]
    public class AlertSMS
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Message { get; set; }

        public bool Sent { get; set; }

        public DateTime? SendDate { get; set; }

        public Guid AlertEventId { get; set; }

        public bool SendError { get; set; }

        public string SendErrorMessage { get; set; }

        public virtual AlertEvent AlertEvent { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
