using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores the text for a screen for a language
    /// </summary>
    [Table("GUIScreenText")]
    [Index(nameof(GuiscreenId), Name = "UX_GUIScreenText_gui_screen_id", IsUnique = true)]
    [Index(nameof(BtnAcceptCaption), Name = "ibtn_accept_caption_GUIScreenText")]
    [Index(nameof(BtnBackCaption), Name = "ibtn_back_caption_GUIScreenText")]
    [Index(nameof(BtnCancelCaption), Name = "ibtn_cancel_caption_GUIScreenText")]
    [Index(nameof(FullInstructions), Name = "ifull_instructions_GUIScreenText")]
    [Index(nameof(GuiscreenId), Name = "iguiscreen_id_GUIScreenText")]
    [Index(nameof(ScreenTitle), Name = "iscreen_title_GUIScreenText")]
    [Index(nameof(ScreenTitleInstruction), Name = "iscreen_title_instruction_GUIScreenText")]
    public partial class GuiscreenText
    {
        public GuiscreenText()
        {
            Guiscreens = new HashSet<Guiscreen>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The GUIScreen this entry corresponds to
        /// </summary>
        [Column("guiscreen_id")]
        public int GuiscreenId { get; set; }
        [Column("screen_title")]
        public Guid? ScreenTitle { get; set; }
        [Column("screen_title_instruction")]
        public Guid? ScreenTitleInstruction { get; set; }
        [Column("full_instructions")]
        public Guid? FullInstructions { get; set; }
        [Column("btn_accept_caption")]
        public Guid? BtnAcceptCaption { get; set; }
        [Column("btn_back_caption")]
        public Guid? BtnBackCaption { get; set; }
        [Column("btn_cancel_caption")]
        public Guid? BtnCancelCaption { get; set; }

        [ForeignKey(nameof(BtnAcceptCaption))]
        [InverseProperty(nameof(TextItem.GuiscreenTextBtnAcceptCaptionNavigations))]
        public virtual TextItem BtnAcceptCaptionNavigation { get; set; }
        [ForeignKey(nameof(BtnBackCaption))]
        [InverseProperty(nameof(TextItem.GuiscreenTextBtnBackCaptionNavigations))]
        public virtual TextItem BtnBackCaptionNavigation { get; set; }
        [ForeignKey(nameof(BtnCancelCaption))]
        [InverseProperty(nameof(TextItem.GuiscreenTextBtnCancelCaptionNavigations))]
        public virtual TextItem BtnCancelCaptionNavigation { get; set; }
        [ForeignKey(nameof(FullInstructions))]
        [InverseProperty(nameof(TextItem.GuiscreenTextFullInstructionsNavigations))]
        public virtual TextItem FullInstructionsNavigation { get; set; }
        [ForeignKey(nameof(GuiscreenId))]
        [InverseProperty("GuiscreenText")]
        public virtual Guiscreen Guiscreen { get; set; }
        [ForeignKey(nameof(ScreenTitleInstruction))]
        [InverseProperty(nameof(TextItem.GuiscreenTextScreenTitleInstructionNavigations))]
        public virtual TextItem ScreenTitleInstructionNavigation { get; set; }
        [ForeignKey(nameof(ScreenTitle))]
        [InverseProperty(nameof(TextItem.GuiscreenTextScreenTitleNavigations))]
        public virtual TextItem ScreenTitleNavigation { get; set; }
        [InverseProperty("GuiTextNavigation")]
        public virtual ICollection<Guiscreen> Guiscreens { get; set; }
    }
}
