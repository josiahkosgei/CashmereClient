using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// [m2m] Currency and CurrencyList
    /// </summary>
    [Table("CurrencyList_Currency")]
    [Index(nameof(CurrencyList), nameof(CurrencyItem), Name = "UX_CurrencyList_Currency_CurrencyItem", IsUnique = true)]
    [Index(nameof(CurrencyList), nameof(CurrencyOrder), Name = "UX_Currency_CurrencyList_Order", IsUnique = true)]
    [Index(nameof(CurrencyItem), Name = "icurrency_item_CurrencyList_Currency")]
    [Index(nameof(CurrencyList), Name = "icurrency_list_CurrencyList_Currency")]
    public partial class CurrencyListCurrency
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The Currency list to which the currency is associated
        /// </summary>
        [Column("currency_list")]
        public int CurrencyList { get; set; }

        /// <summary>
        /// The currency in the list
        /// </summary>
        [Required]
        [Column("currency_item")]
        [StringLength(3)]
        [Unicode(false)]
        public string CurrencyItem { get; set; }

        /// <summary>
        /// ASC Order of sorting for currencies in list.
        /// </summary>
        [Column("currency_order")]
        public int CurrencyOrder { get; set; }
        [Column("max_value")]
        public long MaxValue { get; set; }
        [Column("max_count")]
        public int MaxCount { get; set; }

        [ForeignKey(nameof(CurrencyItem))]
        [InverseProperty(nameof(Currency.CurrencyListCurrencies))]
        public virtual Currency CurrencyItemNavigation { get; set; }
        [ForeignKey(nameof(CurrencyList))]
        [InverseProperty("CurrencyListCurrencies")]
        public virtual CurrencyList CurrencyListNavigation { get; set; }
    }
}
