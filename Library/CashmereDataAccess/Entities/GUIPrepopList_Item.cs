
// Type: CashmereDeposit.GUIPrepopList_Item


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiPrepopListItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid GuiPrepopListId { get; set; }

        public Guid GuiPrepopItemId { get; set; }

        public int ListOrder { get; set; }

        public virtual GuiPrepopItem GuiPrepopItem { get; set; }

        public virtual GuiPrepopList GuiPrepopList { get; set; }
    }
}
