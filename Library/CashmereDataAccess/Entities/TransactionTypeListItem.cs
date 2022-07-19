
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("TransactionTypeListItem")]
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
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        [Column("validate_reference_account")]
        public bool ValidateReferenceAccount { get; set; }
        [Column("default_account")]
        [StringLength(50)]
        public string? DefaultAccount { get; set; }
        [Column("default_account_name")]
        [StringLength(50)]
        public string? DefaultAccountName { get; set; }
        [Column("default_account_currency")]
        [StringLength(3)]
        [Unicode(false)]
        public string DefaultAccountCurrency { get; set; } = null!;
        [Column("validate_default_account")]
        public bool ValidateDefaultAccount { get; set; }
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }
        [Column("tx_type")]
        public int TxType { get; set; }
        [Column("tx_type_guiscreenlist")]
        public int TxTypeGUIScreenlist { get; set; }
        [Column("cb_tx_type")]
        [StringLength(50)]
        public string? CbTxType { get; set; }
        [Column("username")]
        [StringLength(50)]
        public string? Username { get; set; }
        [Column("password")]
        public string? Password { get; set; }
        public byte[]? Icon { get; set; }
        [Column("tx_limit_list")]
        public Guid? TxLimitList { get; set; }
        [Column("tx_text")]
        public Guid? TxText { get; set; }
        [Column("account_permission")]
        public Guid? AccountPermission { get; set; }
        [Column("init_user_required")]
        public bool InitUserRequired { get; set; }
        [Column("auth_user_required")]
        public bool AuthUserRequired { get; set; }

        [ForeignKey("DefaultAccountCurrency")]
        public virtual Currency DefaultAccountCurrencyNavigation { get; set; }
        [ForeignKey("TxLimitList")]
        public virtual TransactionLimitList? TxLimitListNavigation { get; set; }
        [ForeignKey("TxText")]
        public virtual TransactionText TxTextNavigationText { get; set; }
        [ForeignKey("TxTypeGUIScreenlist")]
        public virtual GUIScreenList TxTypeGUIScreenlistNavigation { get; set; }
        [ForeignKey("TxType")]
        public virtual TransactionType TxTypeNavigation { get; set; }
        public virtual TransactionText TransactionTextNav { get; set; } = null!;
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
//TxTextNavigation
//TxTextNavigationText