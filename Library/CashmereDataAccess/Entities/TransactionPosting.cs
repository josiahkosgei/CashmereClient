
// Type: CashmereDeposit.TransactionPosting




using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TransactionPosting
    {
        [Key]
        public Guid Id { get; set; }

        public string CbTxNumber { get; set; }

        public Guid TxId { get; set; }

        public DateTime? PostDate { get; set; }

        public string DrAccount { get; set; }

        public string DrCurrency { get; set; }

        public long DrAmount { get; set; }

        public string CrAccount { get; set; }

        public string CrCurrency { get; set; }

        public long CrAmount { get; set; }

        public string Narration { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? CbDate { get; set; }

        public string CbTxStatus { get; set; }

        public string CbStatusDetail { get; set; }

        public bool DeviceInitiated { get; set; }

        public int PostStatus { get; set; }

        public DateTime InitDate { get; set; }

        public Guid InitialisingUserId { get; set; }
        public Guid? AuthorisingUserId { get; set; }

        public DateTime? AuthDate { get; set; }

        public int? AuthResponse { get; set; }

        public string Reason { get; set; }

        public bool IsComplete { get; set; }

        [ForeignKey("AuthorisingUserId")]
        public virtual ApplicationUser AuthorisingUser { get; set; }
        [ForeignKey("InitialisingUserId")]
        public virtual ApplicationUser InitialisingUser { get; set; }
        [ForeignKey("TxId")]
        public virtual Transaction Transaction { get; set; }

    }
}
