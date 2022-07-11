using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TransactionTypeList")]
    public partial class TransactionTypeList
    {
        public TransactionTypeList()
        {
            Devices = new HashSet<Device>();
            TransactionTypeListTransactionTypeListItems = new HashSet<TransactionTypeListTransactionTypeListItem>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(Device.TransactionTypeListNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(TransactionTypeListTransactionTypeListItem.TxtypeListNavigation))]
        public virtual ICollection<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
    }
}
