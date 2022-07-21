
//Reporting.CITReporting.Transaction


using CashmereUtil.Reporting.MSExcel;
using System;

namespace CashmereUtil.Reporting.CITReporting
{
    public class Transaction
    {
        [EpplusIgnore]
        public Guid id;

        public string DeviceName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string TransactionType { get; set; }

        public string CB_Reference { get; set; }

        public string Currency { get; set; }

        public Decimal Amount { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string RefAccountNumber { get; set; }

        public string RefAccountName { get; set; }

        public string Narration { get; set; }

        public string DepositorName { get; set; }

        public string DepositorIDNumber { get; set; }

        public string DepositorPhone { get; set; }

        public string FundsSource { get; set; }

        public string DeviceReferenceNumber { get; set; }

        [EpplusIgnore]
        public string SuspenseAccount { get; set; }

        [EpplusIgnore]
        public Guid DeviceID { get; set; }

        public override string ToString() => string.Format("id={0}\tTransactionType={1}\tAccountNumber={2}\tAccountName={3}\tRefAccountNumber={4}\tRefAccountName={5}\tCurrency={6}\tAmount={7:n0}\tDeviceNumber={8}\tDateTime={9:yyyy-MM-dd HH:mm:ss.fff}\tNarration={10}\tDepositorName={11}\tDepositorIDNumber={12}\tDepositorPhone={13}\tFundsSource={14}\tSuspenseAccount={15}\tDeviceReferenceNumber={16}", id, TransactionType, AccountNumber, AccountName, RefAccountNumber, RefAccountName, Currency, Amount, DeviceID, EndTime, Narration, DepositorName, DepositorIDNumber, DepositorPhone, FundsSource, SuspenseAccount, DeviceReferenceNumber);
    }
}
