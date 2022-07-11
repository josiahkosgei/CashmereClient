
// Type: CashmereDeposit.Permission

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public Guid ActivityId { get; set; }

        public bool StandaloneAllowed { get; set; }

        public bool StandaloneAuthenticationRequired { get; set; }

        public bool StandaloneCanAuthenticate { get; set; }

        public virtual Activity Activity { get; set; }

        public virtual Role Role { get; set; }
    }
}
