
// Type: CashmereDeposit.TextItemCategory

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class TextItemCategory
  {
    public TextItemCategory()
    {
      TextItems = new HashSet<TextItem>();
    }

        [Key]
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Guid? ParentId { get; set; }
    [ForeignKey(nameof(ParentId))]
    public virtual TextItemCategory Parent { get; set; }
    public virtual ICollection<TextItem> TextItems { get; set; }
    public virtual ICollection<TextItemCategory> Parents { get; set; }
  }
}
