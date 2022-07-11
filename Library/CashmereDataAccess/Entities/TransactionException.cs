using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("TransactionException", Schema = "exp")]
    public class TransactionException
    {
        public Guid Id { get; set; }

        public DateTime Datetime { get; set; }

        public Guid TransactionId { get; set; }

        public int Code { get; set; }

        public int Level { get; set; }

        public string AdditionalInfo { get; set; }

        public string Message { get; set; }

        public string MachineName { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public virtual Transaction Transaction { get; set; }
    }
}
