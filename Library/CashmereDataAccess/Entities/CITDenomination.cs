
// Type: CashmereDeposit.CITDenomination


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class CITDenomination
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CITId { get; set; }

        public DateTime? Datetime { get; set; }

        public string CurrencyId { get; set; }

        public int Denom { get; set; }

        public long Count { get; set; }

        public long Subtotal { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual CIT CIT { get; set; }
    }
}
