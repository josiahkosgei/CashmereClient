
// Type: CashmereDeposit.GUIScreenText


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class GuiScreenText
    {
        public GuiScreenText()
        {
            GuiScreens = new HashSet<GuiScreen>();
        }

        [Key]
        public Guid Id { get; set; }

        public int GuiScreenId { get; set; }

        public Guid ScreenTitleId { get; set; }

        public Guid? ScreenTitleInstructionId { get; set; }

        public Guid? FullInstructionsId { get; set; }

        public Guid? BtnAcceptCaptionId { get; set; }

        public Guid? BtnBackCaptionId { get; set; }

        public Guid? BtnCancelCaptionId { get; set; }

        [ForeignKey(nameof(BtnAcceptCaptionId))]
        public virtual TextItem BtnAcceptCaption { get; set; }

        [ForeignKey(nameof(BtnBackCaptionId))]
        public virtual TextItem BtnBackCaption { get; set; }

        [ForeignKey(nameof(BtnCancelCaptionId))]
        public virtual TextItem BtnCancelCaption { get; set; }

        [ForeignKey(nameof(FullInstructionsId))]
        public virtual TextItem FullInstructions { get; set; }

        [ForeignKey(nameof(GuiScreenId))]
        public virtual GuiScreen GuiScreen { get; set; }

        [ForeignKey(nameof(ScreenTitleInstructionId))]
        public virtual TextItem ScreenTitleInstruction { get; set; }

        [ForeignKey(nameof(ScreenTitleId))]
        public virtual TextItem ScreenTitle { get; set; }
        public virtual ICollection<GuiScreen> GuiScreens { get; set; }
    }
}
