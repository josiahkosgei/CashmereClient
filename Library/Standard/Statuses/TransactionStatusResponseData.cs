
// Type: Cashmere.Library.Standard.Statuses.TransactionStatusResponseData


using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class TransactionStatusResponseData
  {
    public TransactionStatusResponseData(string sessionID, string transactionID)
    {
      SessionID = sessionID ?? throw new ArgumentNullException(nameof (sessionID));
      TransactionID = transactionID ?? throw new ArgumentNullException(nameof (transactionID));
    }

    public long EscrowPlusDroppedTotalCents => EscrowTotalCents + TotalDroppedAmountCents;

    public double EscrowPlusDroppedTotalMajorCurrency => EscrowPlusDroppedTotalCents / 100.0;

    public DropStatusResult CurrentDropStatus { get; set; }

    public long EscrowTotalCents => CurrentDropStatus?.data?.DenominationResult?.data?.TotalValue ?? 0L;

    public string EscrowTotalDisplayString => (EscrowTotalCents / 100.0).ToString("N2");

    public Denomination EscrowNotes => CurrentDropStatus?.data?.DenominationResult?.data;

    public TransactionResultStatus Status { get; set; }

    public TransactionResultType Type { get; set; }

    public TransactionResultResult Result { get; set; }

    public string SessionID { get; set; }

    public string TransactionID { get; set; }

    public long RequestedDropAmount { get; set; }

    public string RequestedDropAmountDisplayString => (RequestedDropAmount / 100.0).ToString("N2");

    public long TotalDroppedAmountCents { get; set; }

    public string TotalDroppedAmountDisplayString => (TotalDroppedAmountCents / 100.0).ToString("N2");

    public int NumberOfDrops { get; set; }

    public Denomination TotalDroppedNotes { get; set; }

    public long LastDroppedAmountCents { get; set; }

    public string LastDroppedAmountDisplayString => (LastDroppedAmountCents / 100.0).ToString("N2");

    public Denomination LastDroppedNotes { get; set; }

    public long RequestedDispenseAmount { get; set; }

    public string RequestedDispenseAmountDisplayString => (RequestedDispenseAmount / 100.0).ToString("N2");

    public long DispensedAmountCents { get; set; }

    public string DispensedAmountDisplayString => (DispensedAmountCents / 100.0).ToString("N2");

    public Denomination RequestedDispenseNotes { get; set; }

    public Denomination DispensedNotes { get; set; }

    public long ResultAmountCents => TotalDroppedAmountCents - DispensedAmountCents;

    public string ResultAmountDisplayString => (ResultAmountCents / 100.0).ToString("N2");
  }
}
