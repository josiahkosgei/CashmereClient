using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Exceptions encountered during execution
    /// </summary>
    [Table("TransactionException", Schema = "exp")]
    [Index(nameof(TransactionId), Name = "itransaction_id_exp_TransactionException_0990CC78")]
    public partial class TransactionException1
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
        public string AdditionalInfo { get; set; }
        [Column("message")]
        [StringLength(255)]
        public string Message { get; set; }
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
