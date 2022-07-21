// Transactions.PostTransactionResponseBase


using System;

namespace Cashmere.API.Messaging.Integration.Transactions
{
    public class PostTransactionResponseBase : APIResponseBase
    {
        public DateTime TransactionDateTime { get; set; }

        public string TransactionID { get; set; }

        public string PostResponseCode { get; set; }

        public string PostResponseMessage { get; set; }
    }
}
