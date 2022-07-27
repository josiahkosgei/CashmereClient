using System.ComponentModel.DataAnnotations;

namespace Cashmere.Finacle.Integration.CQRS.DataAccessLayer.Models
{
    public class AccountPermission
    {
        [Key]
        public Guid id { get; set; }

        public int txType { get; set; }

        public int ListType { get; set; }

        public Guid? error_message { get; set; }

        public bool enabled { get; set; }
    }
}
