using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TransactionTypeListTransactionTypeListItem
    {
        public Guid Id { get; set; }

        public int TxtypeListItemId { get; set; }

        public int TxtypeListId { get; set; }

        public int ListOrder { get; set; }

        [ForeignKey(nameof(TxtypeListId))]
        public virtual TransactionTypeList TxTypeList { get; set; }
        [ForeignKey(nameof(TxtypeListItemId))]
        public virtual TransactionTypeListItem TxTypeListItem { get; set; }
    }
}
