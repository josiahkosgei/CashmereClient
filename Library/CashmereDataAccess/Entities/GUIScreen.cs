
// Type: CashmereDeposit.GUIScreen


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiScreen
    {
        public GuiScreen()
        {
            GuiScreenTexts = new HashSet<GuiScreenText>();
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int GuiScreenTypeId { get; set; }

        public bool Enabled { get; set; }

        public int? Keyboard { get; set; }

        public bool? IsMasked { get; set; }

        public string PrefillText { get; set; }

        public string InputMask { get; set; }

        public Guid? GuiScreenTextId { get; set; }

        [ForeignKey(nameof(GuiScreenTextId))]
        public virtual GuiScreenText GuiScreenText { get; set; }

        [ForeignKey(nameof(GuiScreenTypeId))]
        public virtual GuiScreenType GuiScreenType { get; set; }

        public virtual ICollection<GuiScreenText> GuiScreenTexts { get; set; }

        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}
