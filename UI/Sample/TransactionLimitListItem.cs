using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Limit values for each currency
    /// </summary>
    [Table("TransactionLimitListItem")]
    [Index(nameof(TransactionitemlistId), nameof(CurrencyCode), Name = "UX_TransactionLimitListItem", IsUnique = true)]
    [Index(nameof(CurrencyCode), Name = "icurrency_code_TransactionLimitListItem")]
    [Index(nameof(TransactionitemlistId), Name = "itransactionitemlist_id_TransactionLimitListItem")]
    public partial class TransactionLimitListItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("transactionitemlist_id")]
        public Guid TransactionitemlistId { get; set; }

        /// <summary>
        /// ISO 4217 Three Character Currency Code
        /// </summary>
        [Required]
        [Column("currency_code")]
        [StringLength(3)]
        [Unicode(false)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Whether to show the source of funds screen after deposit limit is reached or passed
        /// </summary>
        [Column("show_funds_source")]
        public bool ShowFundsSource { get; set; }
        [Column("show_funds_form")]
        public Guid? ShowFundsForm { get; set; }

        /// <summary>
        /// The amount after which the Source of Funds screen will be shown
        /// </summary>
        [Column("funds_source_amount")]
        public long FundsSourceAmount { get; set; }

        /// <summary>
        /// CDM will not accept further deposits past the maximum
        /// </summary>
        [Column("prevent_overdeposit")]
        public bool PreventOverdeposit { get; set; }

        /// <summary>
        /// The amount after which the CDM will disable the counter
        /// </summary>
        [Column("overdeposit_amount")]
        public long OverdepositAmount { get; set; }
        [Required]
        [Column("prevent_underdeposit")]
        public bool? PreventUnderdeposit { get; set; }
        [Column("underdeposit_amount")]
        public long UnderdepositAmount { get; set; }
        [Column("prevent_overcount")]
        public bool PreventOvercount { get; set; }
        [Column("overcount_amount")]
        public int OvercountAmount { get; set; }

        [ForeignKey(nameof(CurrencyCode))]
        [InverseProperty(nameof(Currency.TransactionLimitListItems))]
        public virtual Currency CurrencyCodeNavigation { get; set; }
        [ForeignKey(nameof(TransactionitemlistId))]
        [InverseProperty(nameof(TransactionLimitList.TransactionLimitListItems))]
        public virtual TransactionLimitList Transactionitemlist { get; set; }
    }
}
