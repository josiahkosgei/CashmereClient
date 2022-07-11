
// Type: CashmereDeposit.CITPrintout


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class CITPrintout
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Datetime { get; set; }

        public Guid CITId { get; set; }

        public Guid PrintGuid { get; set; }

        public string PrintContent { get; set; }

        public bool IsCopy { get; set; }

        public virtual CIT CIT { get; set; }
    }
}
