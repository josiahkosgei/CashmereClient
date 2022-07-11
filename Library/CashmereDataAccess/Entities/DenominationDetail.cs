
// Type: CashmereDeposit.DenominationDetail


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DenominationDetail
    {
        [Key]
        public Guid Id { get; set; }

        public Guid TxId { get; set; }

        public int Denom { get; set; }

        public long Count { get; set; }

        public long Subtotal { get; set; }

        public DateTime? Datetime { get; set; }

        public virtual Transaction Transaction { get; set; }
    }
}
