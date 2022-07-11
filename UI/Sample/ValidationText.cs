using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Multilanguage validation result text
    /// </summary>
    [Table("ValidationText")]
    [Index(nameof(ErrorMessage), Name = "ierror_message_ValidationText")]
    [Index(nameof(SuccessMessage), Name = "isuccess_message_ValidationText")]
    [Index(nameof(ValidationItemId), Name = "ivalidation_item_id_ValidationText")]
    public partial class ValidationText
    {
        public ValidationText()
        {
            ValidationItems = new HashSet<ValidationItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("validation_item_id")]
        public Guid ValidationItemId { get; set; }
        [Column("error_message")]
        public Guid? ErrorMessage { get; set; }
        [Column("success_message")]
        public Guid? SuccessMessage { get; set; }

        [ForeignKey(nameof(ErrorMessage))]
        [InverseProperty(nameof(TextItem.ValidationTextErrorMessageNavigations))]
        public virtual TextItem ErrorMessageNavigation { get; set; }
        [ForeignKey(nameof(SuccessMessage))]
        [InverseProperty(nameof(TextItem.ValidationTextSuccessMessageNavigations))]
        public virtual TextItem SuccessMessageNavigation { get; set; }
        [ForeignKey(nameof(ValidationItemId))]
        [InverseProperty("ValidationTexts")]
        public virtual ValidationItem ValidationItem { get; set; }
        [InverseProperty("ValidationText")]
        public virtual ICollection<ValidationItem> ValidationItems { get; set; }
    }
}
