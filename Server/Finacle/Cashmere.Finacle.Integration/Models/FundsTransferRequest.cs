using System;

namespace Cashmere.Finacle.Integration.Models
{
    public class FundsTransferRequest
    {
        public string SystemCode_Cred { get; set; }

        public string BankID { get; set; }

        public string SystemCode_FT { get; set; }
        public string MessageReference { get; set; }

        public string TransactionType { get; set; }

        public string TransactionSubType { get; set; }

        public virtual CreditDebitRequestDto DebitRequest { get; set; }
        public virtual CreditDebitRequestDto CreditRequest { get; set; }
    }
}
