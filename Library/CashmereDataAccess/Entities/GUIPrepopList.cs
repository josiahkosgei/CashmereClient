
// Type: CashmereDeposit.GUIPrepopList


using System.ComponentModel.DataAnnotations;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiPrepopList
    {
        public GuiPrepopList()
        {
            GuiPrepopListItems = new HashSet<GuiPrepopListItem>();
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public bool AllowFreeText { get; set; }

        public int DefaultIndex { get; set; }

        public bool UseDefault { get; set; }

        public virtual ICollection<GuiPrepopListItem> GuiPrepopListItems { get; set; }

        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}
