
// Type: CashmereDeposit.GuiScreenList_Screen


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiScreenListScreen
    {
        [Key]
        public Guid Id { get; set; }

        public int GuiScreenId { get; set; }

        public int GuiScreenListId { get; set; }

        public int ScreenOrder { get; set; }

        public bool Required { get; set; }

        public Guid? ValidationListId { get; set; }

        public Guid? GuiPrepopListId { get; set; }

        public bool Enabled { get; set; }
        [ForeignKey(nameof(GuiPrepopListId))]
        public virtual GuiPrepopList GuiPrepopList { get; set; }
        [ForeignKey(nameof(ValidationListId))]
        public virtual ValidationList ValidationList { get; set; }
        [ForeignKey(nameof(GuiScreenId))]
        public virtual GuiScreen GuiScreen { get; set; }
        [ForeignKey(nameof(GuiScreenListId))]
        public virtual GuiScreenList GuiScreenList { get; set; }
    }
}
