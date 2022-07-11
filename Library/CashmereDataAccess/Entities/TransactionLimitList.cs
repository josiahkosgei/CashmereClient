namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TransactionLimitList
    {
        public bool Get_prevent_overdeposit(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem != null && transactionLimitListItem.PreventOverdeposit;
        }

        public long Get_overdeposit_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem == null ? 0L : transactionLimitListItem.OverdepositAmount;
        }

        public bool Get_prevent_underdeposit(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem != null && transactionLimitListItem.PreventUnderdeposit;
        }

        public long Get_underdeposit_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem == null ? 0L : transactionLimitListItem.UnderdepositAmount;
        }

        public bool Get_prevent_overcount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem != null && transactionLimitListItem.PreventOvercount;
        }

        public long Get_overcount_amount(Currency currency)
        {
            TransactionLimitListItem transactionLimitListItem = TransactionLimitListItems.FirstOrDefault(x => x.Currency == currency);
            return transactionLimitListItem != null ? transactionLimitListItem.OvercountAmount : 0L;
        }

        public TransactionLimitList()
        {
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }

        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
