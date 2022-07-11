
// Type: CashmereDeposit.Currency


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Currency
    {
        public override bool Equals(object obj)
        {
            try
            {
                return obj != null && Code.Equals((obj as Currency).Code, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Code.ToUpper().ToString();
        }

        public Currency()
        {
            CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
            CurrencyLists = new HashSet<CurrencyList>();
            DeviceCITSuspenseAccounts = new HashSet<DeviceCITSuspenseAccount>();
            DeviceSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            CITDenominations = new HashSet<CITDenomination>();
            Transactions = new HashSet<Transaction>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }
        
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public int Minor { get; set; }

        public string Flag { get; set; }

        public bool Enabled { get; set; }

        public string Iso3NumericCode { get; set; }

        public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }

        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }

        public virtual ICollection<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }

        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }

        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }

        public virtual ICollection<CITDenomination> CITDenominations { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
