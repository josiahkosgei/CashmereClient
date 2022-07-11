using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class AlertMessageRegistry
    {
        [Key]
        public Guid Id { get; set; }

        public int AlertTypeId { get; set; }

        public Guid RoleId { get; set; }

        public bool EmailEnabled { get; set; }

        public bool PhoneEnabled { get; set; }

        public virtual AlertMessageType AlertMessageType { get; set; }

        public virtual Role Role { get; set; }
    }
}
