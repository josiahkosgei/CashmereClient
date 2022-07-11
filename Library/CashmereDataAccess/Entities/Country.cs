
// Type: CashmereDeposit.Country


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Country
    {
        public Country()
        {
            Banks = new HashSet<Bank>();
        }
        [Key]
        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public virtual ICollection<Bank> Banks { get; set; }
    }
}
