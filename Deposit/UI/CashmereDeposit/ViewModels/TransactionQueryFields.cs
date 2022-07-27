
//.TransactionQueryFields




using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
    public class TransactionQueryFields
    {
        public string start_date;
        public string end_date;
        public Currency currency;
        public TransactionTypeListItem type;
        public string status;
        public string account_number;
    }
}
