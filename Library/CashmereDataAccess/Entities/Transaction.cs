
// Type: CashmereDeposit.Transaction

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Transaction
  {
    public Transaction()
    {
   
        DenominationDetails = new HashSet<DenominationDetail>();
        EscrowJams = new HashSet<EscrowJam>();
        Printouts = new HashSet<Printout>();
        TransactionExceptions = new HashSet<TransactionException>();
    }

    public Guid Id { get; set; }

    public int? TxType { get; set; }

    public Guid SessionId { get; set; }

    public int? TxRandomNumber { get; set; }

    public Guid DeviceId { get; set; }

    public DateTime TxStartDate { get; set; }

    public DateTime? TxEndDate { get; set; }

    public bool TxCompleted { get; set; }

    public string TxCurrency { get; set; }

    public long? TxAmount { get; set; }

    public string TxAccountNumber { get; set; }

    public string CbAccountName { get; set; }

    public string TxRefAccount { get; set; }

    public string CbRefAccountName { get; set; }

    public string TxNarration { get; set; }

    public string TxDepositorName { get; set; }

    public string TxIdNumber { get; set; }

    public string TxPhone { get; set; }

    public string FundsSource { get; set; }

    public int TxResult { get; set; }

    public int TxErrorCode { get; set; }

    public string TxErrorMessage { get; set; }

    public string CbTxNumber { get; set; }

    public DateTime? CbDate { get; set; }

    public string CbTxStatus { get; set; }

    public string CbStatusDetail { get; set; }

    public bool NotesRejected { get; set; }

    public bool JamDetected { get; set; }

    public Guid? CITId { get; set; }

    public bool EscrowJam { get; set; }

    public string TxSuspenseAccount { get; set; }

    public Guid? InitUser { get; set; }

    public Guid? AuthUser { get; set; }
    [ForeignKey(nameof(CITId))]
    public virtual CIT CIT { get; set; }
    [ForeignKey(nameof(TxCurrency))]
    public virtual Currency Currency { get; set; }

    [ForeignKey(nameof(SessionId))]
    public virtual DepositorSession Session { get; set; }
    [ForeignKey(nameof(DeviceId))]
    public virtual Device Device { get; set; }
    [ForeignKey(nameof(TxType))]
    public virtual TransactionTypeListItem TransactionTypeListItem { get; set; }

    public virtual ICollection<Printout> Printouts { get; set; }
    public virtual ICollection<TransactionPosting> TransactionPostings { get; set; }
    public virtual ICollection<EscrowJam> EscrowJams { get; set; }
    public virtual ICollection<DenominationDetail> DenominationDetails { get; set; }
    public virtual ICollection<TransactionException> TransactionExceptions { get; set; }
      }
}
