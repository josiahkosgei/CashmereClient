
// Type: CashmereDeposit.AlertMessageType


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertMessageType
    {
        public AlertMessageType()
        {
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string EmailContentTemplate { get; set; }

        public string RawEmailContentTemplate { get; set; }

        public string PhoneContentTemplate { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
    }
}
