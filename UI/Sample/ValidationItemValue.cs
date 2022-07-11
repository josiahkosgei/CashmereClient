using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Individual values for the validation
    /// </summary>
    [Table("ValidationItemValue")]
    [Index(nameof(ValidationItemId), nameof(Order), Name = "UX_ValidationItemValue", IsUnique = true)]
    [Index(nameof(ValidationItemId), Name = "ivalidation_item_id_ValidationItemValue")]
    public partial class ValidationItemValue
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("validation_item_id")]
        public Guid ValidationItemId { get; set; }
        [Required]
        [Column("value")]
        [StringLength(255)]
        public string Value { get; set; }
        [Column("order")]
        public int Order { get; set; }

        [ForeignKey(nameof(ValidationItemId))]
        [InverseProperty("ValidationItemValues")]
        public virtual ValidationItem ValidationItem { get; set; }
    }
}
