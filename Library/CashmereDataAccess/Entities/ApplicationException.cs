
// Type: CashmereDeposit.ApplicationException


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("ApplicationException", Schema = "exp")]
    public class ApplicationException
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime Datetime { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string Message { get; set; }

        public string AdditionalInfo { get; set; }

        public string Stack { get; set; }

        public string MachineName { get; set; }
        public virtual Device Device { get; set; }
    }
}
