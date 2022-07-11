
// Type: CashmereDeposit.SessionException

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("SessionException", Schema = "exp")]
    public class SessionException
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Datetime { get; set; }

        public Guid SessionId { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string Message { get; set; }

        public string AdditionalInfo { get; set; }

        public string Stack { get; set; }

        public string MachineName { get; set; }
        [ForeignKey(nameof(SessionId))]
        public virtual DepositorSession Session { get; set; }
    }
}
