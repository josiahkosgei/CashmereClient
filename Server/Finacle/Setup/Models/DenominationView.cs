﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    [Keyless]
    public partial class DenominationView
    {
        [Column("tx_id")]
        public Guid TxId { get; set; }
        [Column("subtotal")]
        public int? Subtotal { get; set; }
        [Column("50")]
        public int _50 { get; set; }
        [Column("100")]
        public int _100 { get; set; }
        [Column("200")]
        public int _200 { get; set; }
        [Column("500")]
        public int _500 { get; set; }
        [Column("1000")]
        public int _1000 { get; set; }
    }
}