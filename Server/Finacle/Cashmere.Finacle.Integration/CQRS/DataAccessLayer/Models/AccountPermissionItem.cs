using System.ComponentModel.DataAnnotations;

namespace Cashmere.Finacle.Integration.CQRS.DataAccessLayer.Models
{
    public class AccountPermissionItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid AccountPermission { get; set; }

        public Guid AccountId { get; set; }

        public string ErrorMessage { get; set; }
    }
}
