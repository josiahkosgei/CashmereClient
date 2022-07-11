
// Type: CashmereDeposit.DeviceCITSuspenseAccount


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class DeviceCITSuspenseAccount
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("Device")]
        public Guid DeviceId { get; set; }
        
        [ForeignKey("Currency")]
        public string CurrencyCode { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public bool Enabled { get; set; }

        public Guid? Account { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Device Device { get; set; }
    }
}
