using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Keyless]
    public class TransactionView
    {
        public int? RandomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [StringLength(50)]
        public string BranchName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string DeviceName { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [StringLength(50)]
        public string TransactionTypeName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CbTransactionNumber { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string Currency { get; set; }
        public long? SubTotal { get; set; }
        public long Denomination50 { get; set; }
        public long Denomination100 { get; set; }
        public long Denomination200 { get; set; }
        public long Denomination500 { get; set; }
        public long Denomination1000 { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string AccountNumber { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string AccountName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string ReferenceAccountNumber { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string ReferenceAccountName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Narration { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string DepositorName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string IdNumber { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PhoneNumber { get; set; }
        public int Result { get; set; }
        public int ErrorCode { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string ErrorMessage { get; set; }
        [StringLength(16)]
        [Unicode(false)]
        public string CbStatus { get; set; }
        public bool JamDetected { get; set; }
        public Guid TransactionId { get; set; }
        public Guid? CITId { get; set; }
        public Guid DeviceId { get; set; }
    }
}
