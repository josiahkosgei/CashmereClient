namespace Cashmere.Finacle.Integration.Models
{
    public class CreditDebitRequestDto
    {
         public string TransactionReference { get; set; }

        public string TransactionItemKey { get; set; }

        public string AccountNumber { get; set; }

        public Decimal TransactionAmount { get; set; }

        public string TransactionCurrency { get; set; }

        public string Narrative { get; set; }
    }
}