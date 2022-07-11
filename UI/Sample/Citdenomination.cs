using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// currency and deomination breakdown of the CIT bag
    /// </summary>
    [Table("CITDenominations")]
    [Index(nameof(CITId), Name = "icit_id_CITDenominations")]
    [Index(nameof(CurrencyId), Name = "icurrency_id_CITDenominations")]
    public partial class CITdenomination
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The CIT the record belongs to
        /// </summary>
        [Column("cit_id")]
        public Guid CITId { get; set; }

        /// <summary>
        /// When this item was recorded
        /// </summary>
        [Column("datetime")]
        public DateTime? Datetime { get; set; }

        /// <summary>
        /// The currency code
        /// </summary>
        [Required]
        [Column("currency_id")]
        [StringLength(3)]
        [Unicode(false)]
        public string CurrencyId { get; set; }

        /// <summary>
        /// denomination of note or coin in major currency
        /// </summary>
        [Column("denom")]
        public int Denom { get; set; }

        /// <summary>
        /// How many of the denomination were counted
        /// </summary>
        [Column("count")]
        public long Count { get; set; }

        /// <summary>
        /// The subtotal of the denomination calculated as denom*count
        /// </summary>
        [Column("subtotal")]
        public long Subtotal { get; set; }

        [ForeignKey(nameof(CITId))]
        [InverseProperty("CITdenominations")]
        public virtual CIT CIT { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        [InverseProperty("CITdenominations")]
        public virtual Currency Currency { get; set; }
    }
}
