
// Type: CashmereDeposit.Printout

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Printout
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Datetime { get; set; }

        public Guid TxId { get; set; }

        public Guid PrintGuid { get; set; }

        public string PrintContent { get; set; }
        public bool IsCopy { get; set; }
        [ForeignKey(nameof(TxId))]
        public virtual Transaction Transaction { get; set; }
    }
}
