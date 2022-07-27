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
    /// Stores CIT receipts
    /// </summary>
    [Table("CITPrintout")]
    [Index("CitId", Name = "icit_id_CITPrintout")]
    public partial class Citprintout
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        /// <summary>
        /// The CIT this rceipt belongs to
        /// </summary>
        [Column("cit_id")]
        public Guid CitId { get; set; }
        /// <summary>
        /// Receipt SHA512 hash
        /// </summary>
        [Column("print_guid")]
        public Guid PrintGuid { get; set; }
        /// <summary>
        /// Text of the receipt
        /// </summary>
        [Column("print_content")]
        public string PrintContent { get; set; }
        /// <summary>
        /// Is this CIT Receipt a copy, used for marking duplicate receipts
        /// </summary>
        [Column("is_copy")]
        public bool IsCopy { get; set; }

        [ForeignKey("CitId")]
        [InverseProperty("Citprintouts")]
        public virtual Cit Cit { get; set; }
    }
}