using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TextItem", Schema = "xlns")]
    [Index(nameof(Name), nameof(Category), Name = "UX_UI_TextItem_name", IsUnique = true)]
    [Index(nameof(Category), Name = "iCategory_xlns_TextItem_CDE8C5ED")]
    [Index(nameof(TextItemTypeId), Name = "iTextItemTypeID_xlns_TextItem_2A2E0516")]
    public partial class TextItem
    {
        public TextItem()
        {
            GuiprepopItems = new HashSet<GuiprepopItem>();
            GuiscreenTextBtnAcceptCaptionNavigations = new HashSet<GuiscreenText>();
            GuiscreenTextBtnBackCaptionNavigations = new HashSet<GuiscreenText>();
            GuiscreenTextBtnCancelCaptionNavigations = new HashSet<GuiscreenText>();
            GuiscreenTextFullInstructionsNavigations = new HashSet<GuiscreenText>();
            GuiscreenTextScreenTitleInstructionNavigations = new HashSet<GuiscreenText>();
            GuiscreenTextScreenTitleNavigations = new HashSet<GuiscreenText>();
            TextTranslations = new HashSet<TextTranslation>();
            TransactionTextAccountNameCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextAccountNumberCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextAliasAccountNameCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextAliasAccountNumberCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextDepositorNameCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextDisclaimerNavigations = new HashSet<TransactionText>();
            TransactionTextFullInstructionsNavigations = new HashSet<TransactionText>();
            TransactionTextFundsSourceCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextIdNumberCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextListItemCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextNarrationCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextPhoneNumberCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextReceiptTemplateNavigations = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNameCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNumberCaptionNavigations = new HashSet<TransactionText>();
            TransactionTextTermsNavigations = new HashSet<TransactionText>();
            ValidationTextErrorMessageNavigations = new HashSet<ValidationText>();
            ValidationTextSuccessMessageNavigations = new HashSet<ValidationText>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        public string DefaultTranslation { get; set; }
        public Guid Category { get; set; }
        [Column("TextItemTypeID")]
        public Guid? TextItemTypeId { get; set; }

        [ForeignKey(nameof(Category))]
        [InverseProperty(nameof(TextItemCategory.TextItems))]
        public virtual TextItemCategory CategoryNavigation { get; set; }
        [ForeignKey(nameof(TextItemTypeId))]
        [InverseProperty("TextItems")]
        public virtual TextItemType TextItemType { get; set; }
        [InverseProperty(nameof(GuiprepopItem.ValueNavigation))]
        public virtual ICollection<GuiprepopItem> GuiprepopItems { get; set; }
        [InverseProperty(nameof(GuiscreenText.BtnAcceptCaptionNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextBtnAcceptCaptionNavigations { get; set; }
        [InverseProperty(nameof(GuiscreenText.BtnBackCaptionNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextBtnBackCaptionNavigations { get; set; }
        [InverseProperty(nameof(GuiscreenText.BtnCancelCaptionNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextBtnCancelCaptionNavigations { get; set; }
        [InverseProperty(nameof(GuiscreenText.FullInstructionsNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextFullInstructionsNavigations { get; set; }
        [InverseProperty(nameof(GuiscreenText.ScreenTitleInstructionNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextScreenTitleInstructionNavigations { get; set; }
        [InverseProperty(nameof(GuiscreenText.ScreenTitleNavigation))]
        public virtual ICollection<GuiscreenText> GuiscreenTextScreenTitleNavigations { get; set; }
        [InverseProperty(nameof(TextTranslation.TextItem))]
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        [InverseProperty(nameof(TransactionText.AccountNameCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextAccountNameCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.AccountNumberCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextAccountNumberCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.AliasAccountNameCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNameCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.AliasAccountNumberCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNumberCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.DepositorNameCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextDepositorNameCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.DisclaimerNavigation))]
        public virtual ICollection<TransactionText> TransactionTextDisclaimerNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.FullInstructionsNavigation))]
        public virtual ICollection<TransactionText> TransactionTextFullInstructionsNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.FundsSourceCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextFundsSourceCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.IdNumberCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextIdNumberCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.ListItemCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextListItemCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.NarrationCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextNarrationCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.PhoneNumberCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextPhoneNumberCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.ReceiptTemplateNavigation))]
        public virtual ICollection<TransactionText> TransactionTextReceiptTemplateNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.ReferenceAccountNameCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNameCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.ReferenceAccountNumberCaptionNavigation))]
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNumberCaptionNavigations { get; set; }
        [InverseProperty(nameof(TransactionText.TermsNavigation))]
        public virtual ICollection<TransactionText> TransactionTextTermsNavigations { get; set; }
        [InverseProperty(nameof(ValidationText.ErrorMessageNavigation))]
        public virtual ICollection<ValidationText> ValidationTextErrorMessageNavigations { get; set; }
        [InverseProperty(nameof(ValidationText.SuccessMessageNavigation))]
        public virtual ICollection<ValidationText> ValidationTextSuccessMessageNavigations { get; set; }
    }
}
