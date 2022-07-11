using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Sets the transaction limit amounts for each currency
    /// </summary>
    [Table("TransactionLimitList")]
    public partial class TransactionLimitList
    {
        public TransactionLimitList()
        {
            TransactionLimitListItems = new HashSet<TransactionLimitListItem>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        [InverseProperty(nameof(TransactionLimitListItem.Transactionitemlist))]
        public virtual ICollection<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        [InverseProperty(nameof(TransactionTypeListItem.TxLimitListNavigation))]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
