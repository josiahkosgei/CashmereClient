
// Type: CashmereDeposit.GUIPrepopItem


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiPrepopItem
    {
        public override string ToString()
        {
            return Value.DefaultTranslation;
        }

        public GuiPrepopItem()
        {
            GuiPrepopListItems = new HashSet<GuiPrepopListItem>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ValueId { get; set; }

        public bool Enabled { get; set; }

        public virtual TextItem Value { get; set; }

        public virtual ICollection<GuiPrepopListItem> GuiPrepopListItems { get; set; }
    }
}
