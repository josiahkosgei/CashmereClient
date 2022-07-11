
// Type: CashmereDeposit.EscrowJam


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("EscrowJam", Schema = "exp")]
    public class EscrowJam
    {
        public string ToRawTextString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t", Id, TransactionId,
                DateDetected, DroppedAmount, EscrowAmount, PostedAmount, RetreivedAmount, RecoveryDate,
                InitialisinguserId, AuthorisinguserId, AdditionalInfo);
        }

        public string ToEmailString()
        {
            return string.Format(
                "<tr><td>{0:yyyy-MM-dd HH:mm:ss.fff}</td><td>{1:#,#0.##}</td><td>{2:#,#0.##}</td><td>{3:#,#0.##}</td><td>{4:#,#0.##}</td><td>{5:yyyy-MM-dd HH:mm:ss.fff}</td><td>{6}</td><td>{7}</td></tr>",
                DateDetected, DroppedAmount / 100M, EscrowAmount / 100M, PostedAmount / 100M, RetreivedAmount,
                RecoveryDate, InitialisingUser?.Username, AuthorisingUser?.Username);
        }

        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Transaction")]

        public Guid TransactionId { get; set; }

        public DateTime DateDetected { get; set; }

        public long DroppedAmount { get; set; }

        public long EscrowAmount { get; set; }

        public long PostedAmount { get; set; }

        public long RetreivedAmount { get; set; }

        public DateTime? RecoveryDate { get; set; }

        [ForeignKey("InitialisingUser")]
        public Guid? InitialisinguserId { get; set; }

        [ForeignKey("AuthorisingUser")]
        public Guid? AuthorisinguserId { get; set; }

        public string AdditionalInfo { get; set; }
        public virtual ApplicationUser AuthorisingUser { get; set; }
        public virtual ApplicationUser InitialisingUser { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
