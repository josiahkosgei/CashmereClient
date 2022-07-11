using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Enumeration of allowed Currencies. A device can then associate with a currency list
    /// </summary>
    [Table("CurrencyList")]
    [Index(nameof(DefaultCurrency), Name = "idefault_currency_CurrencyList")]
    public partial class CurrencyList
    {
        public CurrencyList()
        {
            CurrencyListCurrencies = new HashSet<CurrencyListCurrency>();
            Devices = new HashSet<Device>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Required]
        [Column("default_currency")]
        [StringLength(3)]
        [Unicode(false)]
        public string DefaultCurrency { get; set; }

        [ForeignKey(nameof(DefaultCurrency))]
        [InverseProperty(nameof(Currency.CurrencyLists))]
        public virtual Currency DefaultCurrencyNavigation { get; set; }
        [InverseProperty(nameof(CurrencyListCurrency.CurrencyListNavigation))]
        public virtual ICollection<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        [InverseProperty(nameof(Device.CurrencyListNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
    }
}
