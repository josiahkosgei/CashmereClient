
// Type: CashmereDeposit.CurrencyList_Currency


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class CurrencyListCurrency
    {
        [Key]
        public Guid Id { get; set; }

        public int CurrencyListId { get; set; }

        public string CurrencyItemId { get; set; }

        public int CurrencyOrder { get; set; }

        public long MaxValue { get; set; }

        public int MaxCount { get; set; }

        public virtual Currency CurrencyItem { get; set; }
        public virtual CurrencyList CurrencyList { get; set; }
    }
}
