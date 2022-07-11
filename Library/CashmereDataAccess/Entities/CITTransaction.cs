
// Type: CashmereDeposit.CITTransaction


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class CITTransaction
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Datetime { get; set; }

        public Guid CITId { get; set; }

        public string Currency { get; set; }

        public long Amount { get; set; }

        public string SuspenseAccount { get; set; }

        public string AccountNumber { get; set; }

        public string Narration { get; set; }

        public string CbTxNumber { get; set; }

        public DateTime? CbDate { get; set; }

        public string CbTxStatus { get; set; }

        public string CbStatusDetail { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public virtual CIT CIT { get; set; }
    }
}
