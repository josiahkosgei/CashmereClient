
// Type: CashmereDeposit.ValidationItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationItem
  {
   
        public ValidationItem()
        {
            ValidationItemValues = new HashSet<ValidationItemValue>();
            ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
            ValidationTexts = new HashSet<ValidationText>();
        }

        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Category { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        public Guid ValidationTypeId { get; set; }
        public bool Enabled { get; set; }
        public int? ErrorCode { get; set; }
        public Guid? ValidationTextId { get; set; }

        [ForeignKey(nameof(ValidationTypeId))]
        public virtual ValidationType ValidationType { get; set; }
        [ForeignKey(nameof(ValidationTextId))]
        public virtual ValidationText ValidationText { get; set; }
        public virtual ICollection<ValidationItemValue> ValidationItemValues { get; set; }
        public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }
        public virtual ICollection<ValidationText> ValidationTexts { get; set; }
  }
}
