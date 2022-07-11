
// Type: CashmereDeposit.CIT


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class CIT
    {

        internal void ToRawEscrowJamString(StringBuilder s)
        {
            List<ICollection<EscrowJam>> list = Transactions.Select(x => x.EscrowJams).ToList();
            if (list.Count <= 0)
                return;
            s.AppendLine("================================================================================================================================");
            s.AppendLine("Escrow Jams");
            s.AppendLine("================================================================================================================================");
            s.AppendLine("date_detected\tdropped_amount\tescrow_amount\tposted_amount\tretreived_amount\trecovery_date\tInitiating User\tAuthorising User");
            foreach (EscrowJam escrowJam in list)
                s.AppendLine(escrowJam.ToRawTextString());
        }

        public CIT()
        {
            CITDenominations = new HashSet<CITDenomination>();
            CITPrintouts = new HashSet<CITPrintout>();
            CITTransactions = new HashSet<CITTransaction>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public Guid Id { get; set; }

        public Guid DeviceId { get; set; }

        public DateTime CITDate { get; set; }

        public DateTime? CITCompleteDate { get; set; }
        public Guid StartUserId { get; set; }
        public Guid? AuthUserId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string OldBagNumber { get; set; }

        public string NewBagNumber { get; set; }

        public string SealNumber { get; set; }

        public bool Complete { get; set; }

        public int CITError { get; set; }

        public string CITErrorMessage { get; set; }

        public virtual ICollection<CITDenomination> CITDenominations { get; set; }

        public virtual ICollection<CITPrintout> CITPrintouts { get; set; }

        public ICollection<CITTransaction> CITTransactions { get; set; }

        [ForeignKey("AuthUserId")]
        public ApplicationUser AuthorisingUser { get; set; }

        [ForeignKey("StartUserId")]
        public virtual ApplicationUser StartUser { get; set; }

        public virtual Device Device { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
