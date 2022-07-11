
// Type: CashmereDeposit.ValidationText

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class ValidationText
  {
    public ValidationText()
    {
        ValidationItems = new HashSet<ValidationItem>();
    }

    [Key]
    public Guid Id { get; set; }
    public Guid ValidationItemId { get; set; }
    public Guid? ErrorMessageId { get; set; }
    public Guid? SuccessMessageId { get; set; }

    [ForeignKey(nameof(ErrorMessageId))]
    public virtual TextItem ErrorMessage { get; set; }
    [ForeignKey(nameof(SuccessMessageId))]
    public virtual TextItem SuccessMessage { get; set; }
    [ForeignKey(nameof(ValidationItemId))]
    public virtual ValidationItem ValidationItem { get; set; }
    public virtual ICollection<ValidationItem> ValidationItems { get; set; }
  }
}
