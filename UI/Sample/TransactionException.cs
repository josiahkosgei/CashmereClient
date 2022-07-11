using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TransactionException", Schema = "bak")]
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
        public string AdditionalInfo { get; set; }
        [Column("message")]
        [StringLength(255)]
        public string Message { get; set; }

        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("TransactionExceptions")]
        public virtual Transaction Transaction { get; set; }
    }
}
