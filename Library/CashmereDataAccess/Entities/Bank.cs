
// Type: CashmereDeposit.Bank


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Bank
    {
        public Bank()
        {
            Branches = new HashSet<Branch>();
        }

        [Key]
        public Guid Id { get; set; }

        public string BankCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CountryCode { get; set; }
        [ForeignKey("CountryCode")]
        public virtual Country Country { get; set; }

        public virtual ICollection<Branch> Branches { get; set; }
    }
}
