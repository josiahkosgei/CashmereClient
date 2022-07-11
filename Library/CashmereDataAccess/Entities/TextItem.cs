using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TextItem
    {
        public TextItem()
        {
            GuiPrepopItems = new HashSet<GuiPrepopItem>();
            GuiScreenTextBtnAcceptCaptions = new HashSet<GuiScreenText>();
            GuiScreenTextBtnBackCaptions = new HashSet<GuiScreenText>();
            GuiScreenTextBtnCancelCaptions = new HashSet<GuiScreenText>();
            GuiScreenTextFullInstructions = new HashSet<GuiScreenText>();
            GuiScreenTextScreenTitleInstructions = new HashSet<GuiScreenText>();
            GuiScreenTextScreenTitles = new HashSet<GuiScreenText>();
            TextTranslations = new HashSet<TextTranslation>();
            TransactionTextAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextAliasAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextAliasAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextDepositorNameCaptions = new HashSet<TransactionText>();
            TransactionTextDisclaimers = new HashSet<TransactionText>();
            TransactionTextFullInstructions = new HashSet<TransactionText>();
            TransactionTextFundsSourceCaptions = new HashSet<TransactionText>();
            TransactionTextIdNumberCaptions = new HashSet<TransactionText>();
            TransactionTextListItemCaptions = new HashSet<TransactionText>();
            TransactionTextNarrationCaptions = new HashSet<TransactionText>();
            TransactionTextPhoneNumberCaptions = new HashSet<TransactionText>();
            TransactionTextReceiptTemplates = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextTerms = new HashSet<TransactionText>();
            ValidationTextErrorMessages = new HashSet<ValidationText>();
            ValidationTextSuccessMessages = new HashSet<ValidationText>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public string DefaultTranslation { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? TextItemTypeId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual TextItemCategory Category { get; set; }

        [ForeignKey(nameof(TextItemTypeId))]
        public virtual TextItemType TextItemType { get; set; }
        public virtual ICollection<GuiPrepopItem> GuiPrepopItems { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextBtnAcceptCaptions { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextBtnBackCaptions { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextBtnCancelCaptions { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextFullInstructions { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextScreenTitleInstructions { get; set; }
        public virtual ICollection<GuiScreenText> GuiScreenTextScreenTitles { get; set; }
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextDepositorNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextDisclaimers { get; set; }
        public virtual ICollection<TransactionText> TransactionTextFullInstructions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextFundsSourceCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextIdNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextListItemCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextNarrationCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextPhoneNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReceiptTemplates { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextTerms { get; set; }
        public virtual ICollection<ValidationText> ValidationTextErrorMessages { get; set; }
        public virtual ICollection<ValidationText> ValidationTextSuccessMessages { get; set; }
    }

}
