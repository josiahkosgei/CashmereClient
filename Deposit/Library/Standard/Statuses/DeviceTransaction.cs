// DeviceTransaction


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
                throw new ArgumentOutOfRangeException(string.Format("transactionValueCents of {0} is greater than transactionLimitCents of {1}", (object)transactionValueCents, (object)transactionLimitCents));
            this.AccountNumber = accountNumber;
            this.SessionID = sessionID;
            this.TransactionID = transactionID;
            this.Currency = currency;
            this.TransactionLimitCents = transactionLimitCents;
            this.TransactionValueCents = transactionValueCents;
            this.StartDate = DateTime.Now;
            this.CurrentTransactionResult = new TransactionStatusResponseData(sessionID, transactionID);
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
                long num = Math.Min(this.TransactionLimitCents, this.TransactionValueCents);
                long? droppedAmountCents = this.CurrentTransactionResult?.TotalDroppedAmountCents;
                return (droppedAmountCents.HasValue ? new long?(num - droppedAmountCents.GetValueOrDefault()) : new long?()).GetValueOrDefault();
            }
        }

        public long TransactionLimitCents { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
