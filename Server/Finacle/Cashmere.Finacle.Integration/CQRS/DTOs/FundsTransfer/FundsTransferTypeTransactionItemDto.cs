namespace Cashmere.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferTypeTransactionItemDto
    {
      
         public string TransactionReference { get; set; }
       
        public string TransactionItemKey { get; set; }
        public string AccountNumber  { get; set; }
        public string DebitCreditFlag { get; set; }
        public decimal TransactionAmount { get; set; }
        public bool TransactionAmountSpecified { get; set; }
        public string TransactionCurrency { get; set; }
        public string Narrative { get; set; }
        public string SourceBranch { get; set; }
        public string TransactionCode { get; set; }
        public string AvailableBalance { get; set; }
        public string BookedBalance  { get; set; }
        public string ChequeOrDraftNumber  { get; set; }
        public string RateCode  { get; set; }
        public decimal ExchangeRate  { get; set; }
        public bool ExchangeRateSpecified  { get; set; }
        public string TemporaryODRequired  { get; set; }
        public FundsTransferTypeTransactionItemTemporaryODDetailsDto TemporaryODDetails  { get; set; }
        
    }
}