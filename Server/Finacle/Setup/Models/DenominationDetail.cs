﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    /// <summary>
    /// Denomination enumeration for a Transaction
    /// </summary>
    [Table("DenominationDetail")]
    [Index("TxId", Name = "itx_id_DenominationDetail")]
    public partial class DenominationDetail
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("tx_id")]
        public Guid TxId { get; set; }
        /// <summary>
        /// denomination of note or coin in major currency
        /// </summary>
        [Column("denom")]
        public int Denom { get; set; }
        /// <summary>
        /// How many of the denomination were counted
        /// </summary>
        [Column("count")]
        public int Count { get; set; }
        /// <summary>
        /// The subtotal of the denomination calculated as denom*count
        /// </summary>
        [Column("subtotal")]
        public int Subtotal { get; set; }
        [Column("datetime")]
        public DateTime? Datetime { get; set; }

        [ForeignKey("TxId")]
        [InverseProperty("DenominationDetails")]
        public virtual Transaction Tx { get; set; }
    }
}