using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Link a ValidationItem to a ValidationList
    /// </summary>
    [Table("ValidationList_ValidationItem")]
    [Index(nameof(ValidationItemId), nameof(ValidationListId), Name = "UX_ValidationList_ValidationItem_UniqueItem", IsUnique = true)]
    [Index(nameof(ValidationListId), nameof(Order), Name = "UX_ValidationList_ValidationItem_UniqueOrder", IsUnique = true)]
    [Index(nameof(ValidationItemId), Name = "ivalidation_item_id_ValidationList_ValidationItem")]
    [Index(nameof(ValidationListId), Name = "ivalidation_list_id_ValidationList_ValidationItem")]
    public partial class ValidationListValidationItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("validation_list_id")]
        public Guid ValidationListId { get; set; }
        [Column("validation_item_id")]
        public Guid ValidationItemId { get; set; }
        [Column("order")]
        public int Order { get; set; }
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }

        [ForeignKey(nameof(ValidationItemId))]
        [InverseProperty("ValidationListValidationItems")]
        public virtual ValidationItem ValidationItem { get; set; }
        [ForeignKey(nameof(ValidationListId))]
        [InverseProperty("ValidationListValidationItems")]
        public virtual ValidationList ValidationList { get; set; }
    }
}
