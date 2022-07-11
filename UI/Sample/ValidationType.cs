using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// The type of validation e.g. regex, etc
    /// </summary>
    [Table("ValidationType")]
    public partial class ValidationType
    {
        public ValidationType()
        {
            ValidationItems = new HashSet<ValidationItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

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

        [InverseProperty(nameof(ValidationItem.Type))]
        public virtual ICollection<ValidationItem> ValidationItems { get; set; }
    }
}
