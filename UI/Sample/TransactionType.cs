using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TransactionType")]
    public partial class TransactionType
    {
        public TransactionType()
        {
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Vendor supplied ScreenType GUID
        /// </summary>
        [Column("code")]
        public Guid Code { get; set; }

        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(TransactionTypeListItem.TxTypeNavigation))]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
