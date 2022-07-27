﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("CITDenominations")]
    // [Index("CITId", Name = "icit_id_CITDenominations")]
    // [Index("CurrencyId", Name = "icurrency_id_CITDenominations")]
    public partial class CITDenomination
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("cit_id")]
        public Guid CITId { get; set; }
        [Column("datetime")]
        public DateTime? Datetime { get; set; }
        [Column("currency_id")]
        [StringLength(3)]
        [Unicode(false)]
        public string CurrencyId { get; set; } = null!;
        [Column("denom")]
        public int Denom { get; set; }
        [Column("count")]
        public long Count { get; set; }
        [Column("subtotal")]
        public long Subtotal { get; set; }

        [ForeignKey("CITId")]
        //[InverseProperty("CITDenominations")]
        public virtual CIT CIT { get; set; } = null!;
        [ForeignKey("CurrencyId")]
        //[InverseProperty("CITDenominations")]
        public virtual Currency Currency { get; set; } = null!;
    }
}