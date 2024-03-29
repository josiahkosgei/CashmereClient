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
    /// Stores contents of a printout for a transaction
    /// </summary>
    [Table("Printout")]
    [Index("TxId", Name = "itx_id_Printout")]
    public partial class Printout
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("tx_id")]
        public Guid TxId { get; set; }
        [Column("print_guid")]
        public Guid PrintGuid { get; set; }
        [Column("print_content")]
        public string PrintContent { get; set; }
        [Column("is_copy")]
        public bool IsCopy { get; set; }

        [ForeignKey("TxId")]
        [InverseProperty("Printouts")]
        public virtual Transaction Tx { get; set; }
    }
}