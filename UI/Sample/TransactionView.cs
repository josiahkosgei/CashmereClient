using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Keyless]
    public partial class TransactionView
    {
        [Column("Random-Number")]
        public int? RandomNumber { get; set; }
        [Column("Start-Date")]
        public DateTime StartDate { get; set; }
        [Column("End-Date")]
        public DateTime? EndDate { get; set; }
        [Required]
        [Column("Branch-Name")]
        [StringLength(50)]
        public string BranchName { get; set; }
        [Required]
        [Column("Device-Name")]
        [StringLength(50)]
        [Unicode(false)]
        public string DeviceName { get; set; }
        [Required]
        [Column("Device-Location")]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [Column("Transaction-Type-Name")]
        [StringLength(50)]
        public string TransactionTypeName { get; set; }
        [Column("CB-Transaction-Number")]
        [StringLength(50)]
        [Unicode(false)]
        public string CbTransactionNumber { get; set; }
        [StringLength(3)]
        [Unicode(false)]
        public string Currency { get; set; }
        [Column("Sub-Total")]
        public long? SubTotal { get; set; }
        [Column("Denomination:-50")]
        public long Denomination50 { get; set; }
        [Column("Denomination:-100")]
        public long Denomination100 { get; set; }
        [Column("Denomination:-200")]
        public long Denomination200 { get; set; }
        [Column("Denomination:-500")]
        public long Denomination500 { get; set; }
        [Column("Denomination:-1000")]
        public long Denomination1000 { get; set; }
        [Column("Account-Number")]
        [StringLength(50)]
        [Unicode(false)]
        public string AccountNumber { get; set; }
        [Column("Account-Name")]
        [StringLength(50)]
        [Unicode(false)]
        public string AccountName { get; set; }
        [Column("Reference-Account-Number")]
        [StringLength(50)]
        [Unicode(false)]
        public string ReferenceAccountNumber { get; set; }
        [Column("Reference-Account-Name")]
        [StringLength(50)]
        [Unicode(false)]
        public string ReferenceAccountName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Narration { get; set; }
        [Column("Depositor-Name")]
        [StringLength(50)]
        [Unicode(false)]
        public string DepositorName { get; set; }
        [Column("ID-Number")]
        [StringLength(50)]
        [Unicode(false)]
        public string IdNumber { get; set; }
        [Column("Phone-Number")]
        [StringLength(50)]
        [Unicode(false)]
        public string PhoneNumber { get; set; }
        public int Result { get; set; }
        [Column("Error-Code")]
        public int ErrorCode { get; set; }
        [Column("Error-Message")]
        [StringLength(255)]
        [Unicode(false)]
        public string ErrorMessage { get; set; }
        [Column("CB-Status")]
        [StringLength(16)]
        [Unicode(false)]
        public string CbStatus { get; set; }
        [Column("Jam-Detected")]
        public bool JamDetected { get; set; }
        [Column("Transaction-ID")]
        public Guid TransactionId { get; set; }
        [Column("CIT-ID")]
        public Guid? CITId { get; set; }
        [Column("Device-ID")]
        public Guid DeviceId { get; set; }
    }
}
