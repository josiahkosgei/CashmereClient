﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    /// <summary>
    /// Exceptions encountered during execution
    /// </summary>
    [Table("TransactionException", Schema = "exp")]
    [Index("TransactionId", Name = "itransaction_id_exp_TransactionException_0990CC78")]
    public partial class TransactionException
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("transaction_id")]
        public Guid TransactionId { get; set; }
        [Column("code")]
        public int Code { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("additional_info")]
        [StringLength(255)]
        public string? AdditionalInfo { get; set; }
        [Column("message")]
        [StringLength(255)]
        public string? Message { get; set; }
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; } = null!;
    }
}