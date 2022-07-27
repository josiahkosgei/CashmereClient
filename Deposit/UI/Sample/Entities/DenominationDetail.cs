﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("DenominationDetail")]
    [Index("TxId", Name = "itx_id_DenominationDetail")]
    public partial class DenominationDetail
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("tx_id")]
        public Guid TxId { get; set; }
        [Column("denom")]
        public int Denom { get; set; }
        [Column("count")]
        public long Count { get; set; }
        [Column("subtotal")]
        public long Subtotal { get; set; }
        [Column("datetime")]
        public DateTime? Datetime { get; set; }

        [ForeignKey("TxId")]
        [InverseProperty("DenominationDetails")]
        public virtual Transaction Tx { get; set; } = null!;
    }
}