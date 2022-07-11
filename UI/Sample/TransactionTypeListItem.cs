using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Transactions that the system can perform e.g. regular deposit, Mpesa deposit, etc
    /// </summary>
    [Table("TransactionTypeListItem")]
    [Index(nameof(DefaultAccountCurrency), Name = "idefault_account_currency_TransactionTypeListItem")]
    [Index(nameof(TxLimitList), Name = "itx_limit_list_TransactionTypeListItem")]
    [Index(nameof(TxText), Name = "itx_text_TransactionTypeListItem")]
    [Index(nameof(TxType), Name = "itx_type_TransactionTypeListItem")]
    [Index(nameof(TxTypeGuiscreenlist), Name = "itx_type_guiscreenlist_TransactionTypeListItem")]
    public partial class TransactionTypeListItem
    {
        public TransactionTypeListItem()
        {
            TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("validate_reference_account")]
        public bool ValidateReferenceAccount { get; set; }

        /// <summary>
        /// the default account that pre-polulates the AccountNumber of a transaction
        /// </summary>
        [Column("default_account")]
        [StringLength(50)]
        public string DefaultAccount { get; set; }
        [Column("default_account_name")]
        [StringLength(50)]
        public string DefaultAccountName { get; set; }
        [Required]
        [Column("default_account_currency")]
        [StringLength(3)]
        [Unicode(false)]
        public string DefaultAccountCurrency { get; set; }
        [Column("validate_default_account")]
        public bool ValidateDefaultAccount { get; set; }
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }
        [Column("tx_type")]
        public int TxType { get; set; }
        [Column("tx_type_guiscreenlist")]
        public int TxTypeGuiscreenlist { get; set; }

        /// <summary>
        /// A string passed to core banking with transaction details so core banking can route the deposit to the correct handler
        /// </summary>
        [Column("cb_tx_type")]
        [StringLength(50)]
        public string CbTxType { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
        public byte[] Icon { get; set; }
        [Column("tx_limit_list")]
        public Guid? TxLimitList { get; set; }
        [Column("tx_text")]
        public Guid? TxText { get; set; }

        [ForeignKey(nameof(DefaultAccountCurrency))]
        [InverseProperty(nameof(Currency.TransactionTypeListItems))]
        public virtual Currency DefaultAccountCurrencyNavigation { get; set; }
        [ForeignKey(nameof(TxLimitList))]
        [InverseProperty(nameof(TransactionLimitList.TransactionTypeListItems))]
        public virtual TransactionLimitList TxLimitListNavigation { get; set; }
        [ForeignKey(nameof(TxText))]
        [InverseProperty("TransactionTypeListItems")]
        public virtual TransactionText TxTextNavigation { get; set; }
        [ForeignKey(nameof(TxTypeGuiscreenlist))]
        [InverseProperty(nameof(GuiscreenList.TransactionTypeListItems))]
        public virtual GuiscreenList TxTypeGuiscreenlistNavigation { get; set; }
        [ForeignKey(nameof(TxType))]
        [InverseProperty(nameof(TransactionType.TransactionTypeListItems))]
        public virtual TransactionType TxTypeNavigation { get; set; }
        [InverseProperty("TxItemNavigation")]
        public virtual TransactionText TransactionText { get; set; }
        [InverseProperty(nameof(TransactionTypeListTransactionTypeListItem.TxtypeListItemNavigation))]
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        [InverseProperty(nameof(Transaction.TxTypeNavigation))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
