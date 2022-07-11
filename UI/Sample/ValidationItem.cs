using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ValidationItem")]
    [Index(nameof(TypeId), Name = "itype_id_ValidationItem")]
    [Index(nameof(ValidationTextId), Name = "ivalidation_text_id_ValidationItem")]
    public partial class ValidationItem
    {
        public ValidationItem()
        {
            ValidationItemValues = new HashSet<ValidationItemValue>();
            ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
            ValidationTexts = new HashSet<ValidationText>();
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
        [Column("category")]
        [StringLength(10)]
        public string Category { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("type_id")]
        public Guid TypeId { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("error_code")]
        public int? ErrorCode { get; set; }
        [Column("validation_text_id")]
        public Guid? ValidationTextId { get; set; }

        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(ValidationType.ValidationItems))]
        public virtual ValidationType Type { get; set; }
        [ForeignKey(nameof(ValidationTextId))]
        [InverseProperty("ValidationItems")]
        public virtual ValidationText ValidationText { get; set; }
        [InverseProperty(nameof(ValidationItemValue.ValidationItem))]
        public virtual ICollection<ValidationItemValue> ValidationItemValues { get; set; }
        [InverseProperty(nameof(ValidationListValidationItem.ValidationItem))]
        public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }
        [InverseProperty("ValidationItem")]
        public virtual ICollection<ValidationText> ValidationTexts { get; set; }
    }
}
