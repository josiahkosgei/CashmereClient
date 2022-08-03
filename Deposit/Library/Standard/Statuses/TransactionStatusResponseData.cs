//TransactionStatusResponseData

using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class TransactionStatusResponseData
  {
    public TransactionStatusResponseData(string sessionID, string transactionID)
    {
      this.SessionID = sessionID ?? throw new ArgumentNullException(nameof (sessionID));
      this.TransactionID = transactionID ?? throw new ArgumentNullException(nameof (transactionID));
    }

    public long EscrowPlusDroppedTotalCents => this.EscrowTotalCents + this.TotalDroppedAmountCents;

    public double EscrowPlusDroppedTotalMajorCurrency => (double) this.EscrowPlusDroppedTotalCents / 100.0;

    public DropStatusResult CurrentDropStatus { get; set; }

    public long EscrowTotalCents => (long)(this.CurrentDropStatus?.data?.DenominationResult?.data?.TotalValue);

    public string EscrowTotalDisplayString => ((double) this.EscrowTotalCents / 100.0).ToString("N2");

    public Denomination EscrowNotes => this.CurrentDropStatus?.data?.DenominationResult?.data;

    public TransactionResultStatus Status { get; set; }

    public TransactionResultType Type { get; set; }

    public TransactionResultResult Result { get; set; }

    public string SessionID { get; set; }

    public string TransactionID { get; set; }

    public long RequestedDropAmount { get; set; }

    public string RequestedDropAmountDisplayString => ((double) this.RequestedDropAmount / 100.0).ToString("N2");

    public long TotalDroppedAmountCents { get; set; }

    public string TotalDroppedAmountDisplayString => ((double) this.TotalDroppedAmountCents / 100.0).ToString("N2");

    public int NumberOfDrops { get; set; }

    public Denomination TotalDroppedNotes { get; set; }

    public long LastDroppedAmountCents { get; set; }

    public string LastDroppedAmountDisplayString => ((double) this.LastDroppedAmountCents / 100.0).ToString("N2");

    public Denomination LastDroppedNotes { get; set; }

    public long RequestedDispenseAmount { get; set; }

    public string RequestedDispenseAmountDisplayString => ((double) this.RequestedDispenseAmount / 100.0).ToString("N2");

    public long DispensedAmountCents { get; set; }

    public string DispensedAmountDisplayString => ((double) this.DispensedAmountCents / 100.0).ToString("N2");

    public Denomination RequestedDispenseNotes { get; set; }

    public Denomination DispensedNotes { get; set; }

    public long ResultAmountCents => this.TotalDroppedAmountCents - this.DispensedAmountCents;

    public string ResultAmountDisplayString => ((double) this.ResultAmountCents / 100.0).ToString("N2");
  }
}
