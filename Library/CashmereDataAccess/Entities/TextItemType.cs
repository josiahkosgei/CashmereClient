
// Type: CashmereDeposit.TextItemType

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class TextItemType
  {
    public TextItemType()
    {
        TextItems = new HashSet<TextItem>();
    }

    [Key]
    public Guid Id { get; set; }

    public string Token { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<TextItem> TextItems { get; set; }
  }
}
