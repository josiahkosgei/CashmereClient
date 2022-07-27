using System.ComponentModel.DataAnnotations;

namespace Cashmere.Finacle.Integration.CQRS.DataAccessLayer.Models
{
    public class Account
    {
        [Key]
        public Guid id { get; set; }

        public byte[] Icon { get; set; }

        public string Currency { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public bool Enabed { get; set; } = true;
    }
}
