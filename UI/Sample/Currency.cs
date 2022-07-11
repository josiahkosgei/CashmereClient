using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Currency enumeration
    /// </summary>
    [Table("Currency")]
    public partial class Currency
    {
        public Currency()
        {
            CITdenominations = new HashSet<CITdenomination>();
            CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
            CurrencyLists = new HashSet<CurrencyList>();
            DeviceSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
            Transactions = new HashSet<Transaction>();
        }


        /// <summary>
        /// ISO 4217 Three Character Currency Code
        /// </summary>
        [Key]
        [Column("code")]
        [StringLength(3)]
        [Unicode(false)]
        public string Code { get; set; }

        /// <summary>
        /// Name of the currency
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        /// <summary>
        /// Expresses the relationship between a major currency unit and its corresponding minor currency unit. This mechanism is called the currency &quot;exponent&quot; and assumes a base of 10. Will be used with converters in the GUI
        /// </summary>
        [Column("minor")]
        public int Minor { get; set; }

        /// <summary>
        /// two character country code for the national flag to display for the language
        /// </summary>
        [Column("flag")]
        [StringLength(255)]
        public string Flag { get; set; }

        /// <summary>
        /// whether the system supports the language
        /// </summary>
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(CITdenomination.Currency))]
        public virtual ICollection<CITdenomination> CITdenominations { get; set; }
        [InverseProperty(nameof(CurrencyListCurrency.CurrencyItemNavigation))]
        public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        [InverseProperty(nameof(CurrencyList.DefaultCurrencyNavigation))]
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }
        [InverseProperty(nameof(DeviceSuspenseAccount.CurrencyCodeNavigation))]
        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        [InverseProperty(nameof(TransactionLimitListItem.CurrencyCodeNavigation))]
        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        [InverseProperty(nameof(TransactionTypeListItem.DefaultAccountCurrencyNavigation))]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
        [InverseProperty(nameof(Transaction.TxCurrencyNavigation))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
