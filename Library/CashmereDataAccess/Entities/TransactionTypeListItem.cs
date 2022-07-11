
// Type: CashmereDeposit.TransactionTypeListItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TransactionTypeListItem
    {
         public TransactionTypeListItem()
        {
            TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [StringLength(255)]
        public string Description { get; set; }
        public bool ValidateReferenceAccount { get; set; }

        /// <summary>
        /// the default account that pre-polulates the AccountNumber of a transaction
        /// </summary>
        [StringLength(50)]
        public string DefaultAccount { get; set; }
        [StringLength(50)]
        public string DefaultAccountName { get; set; }
        [Required]
        public string DefaultAccountCurrencyId { get; set; }
        public bool ValidateDefaultAccount { get; set; }
        [Required]
        public bool Enabled { get; set; }
        public int TxTypeId { get; set; }
        public int TxTypeGuiScreenListId { get; set; }

        /// <summary>
        /// A string passed to core banking with transaction details so core banking can route the deposit to the correct handler
        /// </summary>
        [StringLength(50)]
        public string CbTxType { get; set; }
        [StringLength(50)]
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] Icon { get; set; }
        public Guid? TxLimitListId { get; set; }
        public Guid? TxTextId { get; set; }
        public bool InitUserRequired { get; set; }
        public bool AuthUserRequired { get; set; }

        [ForeignKey(nameof(DefaultAccountCurrencyId))]
        public virtual Currency DefaultAccountCurrency { get; set; }

        [ForeignKey(nameof(TxLimitListId))]
        public virtual TransactionLimitList TxLimitList { get; set; }

        [ForeignKey(nameof(TxTextId))]
        public virtual TransactionText TxText { get; set; }

        [ForeignKey(nameof(TxTypeGuiScreenListId))]
        public virtual GuiScreenList TxTypeGuiScreenList { get; set; }

        [ForeignKey(nameof(TxTypeId))]
        public virtual TransactionType TxType { get; set; }
        public virtual TransactionText TransactionText { get; set; }
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
