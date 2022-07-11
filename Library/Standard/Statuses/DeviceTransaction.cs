
// Type: Cashmere.Library.Standard.Statuses.DeviceTransaction


using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceTransaction
  {
    public DeviceTransaction(
      string accountNumber,
      string sessionID,
      string transactionID,
      string currency,
      long transactionLimitCents,
      long transactionValueCents)
    {
      if (transactionLimitCents < transactionValueCents)
        throw new ArgumentOutOfRangeException(string.Format("transactionValueCents of {0} is greater than transactionLimitCents of {1}", transactionValueCents, transactionLimitCents));
      AccountNumber = accountNumber;
      SessionID = sessionID;
      TransactionID = transactionID;
      Currency = currency;
      TransactionLimitCents = transactionLimitCents;
      TransactionValueCents = transactionValueCents;
      StartDate = DateTime.Now;
      CurrentTransactionResult = new TransactionStatusResponseData(sessionID, transactionID);
    }

    public string Currency { get; set; }

    public TransactionStatusResponseData CurrentTransactionResult { get; set; }

    public DeviceDrop DropResults { get; set; } = new DeviceDrop();

    public string SessionID { get; set; }

    public string TransactionID { get; set; }

    public string AccountNumber { get; set; }

    public long TransactionValueCents { get; set; }

    public long TransactionValueCentsLeft
    {
      get
      {
        long num = Math.Min(TransactionLimitCents, TransactionValueCents);
        long? droppedAmountCents = CurrentTransactionResult?.TotalDroppedAmountCents;
        return (droppedAmountCents.HasValue ? new long?(num - droppedAmountCents.GetValueOrDefault()) : new long?()) ?? 0L;
      }
    }

    public long TransactionLimitCents { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
  }
}
