
// Type: CashmereDeposit.DepositorSession


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DepositorSession
    {
        public DepositorSession()
        {
            ApplicationLogs = new HashSet<ApplicationLog>();
            SessionExceptions = new HashSet<SessionException>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime SessionStart { get; set; }

        public DateTime? SessionEnd { get; set; }

        public string LanguageCode { get; set; }

        public bool Complete { get; set; }

        public bool CompleteSuccess { get; set; }

        public int? ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public bool TermsAccepted { get; set; }

        public bool AccountVerified { get; set; }

        public bool ReferenceAccountVerified { get; set; }

        public string Salt { get; set; }


        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; }

        [ForeignKey(nameof(LanguageCode))]
        public virtual Language Language { get; set; }
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        public virtual ICollection<SessionException> SessionExceptions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
