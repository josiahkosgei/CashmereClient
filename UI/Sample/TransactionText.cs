using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores the multi language texts for a tx
    /// </summary>
    [Table("TransactionText")]
    [Index(nameof(TxItem), Name = "UX_TransactionText_tx_item", IsUnique = true)]
    [Index(nameof(FundsSourceCaption), Name = "iFundsSource_caption_TransactionText")]
    [Index(nameof(AccountNameCaption), Name = "iaccount_name_caption_TransactionText")]
    [Index(nameof(AccountNumberCaption), Name = "iaccount_number_caption_TransactionText")]
    [Index(nameof(AliasAccountNameCaption), Name = "ialias_account_name_caption_TransactionText")]
    [Index(nameof(AliasAccountNumberCaption), Name = "ialias_account_number_caption_TransactionText")]
    [Index(nameof(DepositorNameCaption), Name = "idepositor_name_caption_TransactionText")]
    [Index(nameof(Disclaimer), Name = "idisclaimer_TransactionText")]
    [Index(nameof(FullInstructions), Name = "ifull_instructions_TransactionText")]
    [Index(nameof(IdNumberCaption), Name = "iid_number_caption_TransactionText")]
    [Index(nameof(ListItemCaption), Name = "ilistItem_caption_TransactionText")]
    [Index(nameof(NarrationCaption), Name = "inarration_caption_TransactionText")]
    [Index(nameof(PhoneNumberCaption), Name = "iphone_number_caption_TransactionText")]
    [Index(nameof(ReceiptTemplate), Name = "ireceipt_template_TransactionText")]
    [Index(nameof(ReferenceAccountNameCaption), Name = "ireference_account_name_caption_TransactionText")]
    [Index(nameof(ReferenceAccountNumberCaption), Name = "ireference_account_number_caption_TransactionText")]
    [Index(nameof(Terms), Name = "iterms_TransactionText")]
    [Index(nameof(TxItem), Name = "itx_item_TransactionText")]
    public partial class TransactionText
    {
        public TransactionText()
        {
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("tx_item")]
        public int TxItem { get; set; }
        [Column("disclaimer")]
        public Guid? Disclaimer { get; set; }
        [Column("terms")]
        public Guid? Terms { get; set; }
        [Column("full_instructions")]
        public Guid? FullInstructions { get; set; }
        [Column("listItem_caption")]
        public Guid? ListItemCaption { get; set; }
        [Column("account_number_caption")]
        public Guid? AccountNumberCaption { get; set; }
        [Column("account_name_caption")]
        public Guid? AccountNameCaption { get; set; }
        [Column("reference_account_number_caption")]
        public Guid? ReferenceAccountNumberCaption { get; set; }
        [Column("reference_account_name_caption")]
        public Guid? ReferenceAccountNameCaption { get; set; }
        [Column("narration_caption")]
        public Guid? NarrationCaption { get; set; }
        [Column("alias_account_number_caption")]
        public Guid? AliasAccountNumberCaption { get; set; }
        [Column("alias_account_name_caption")]
        public Guid? AliasAccountNameCaption { get; set; }
        [Column("depositor_name_caption")]
        public Guid? DepositorNameCaption { get; set; }
        [Column("phone_number_caption")]
        public Guid? PhoneNumberCaption { get; set; }
        [Column("id_number_caption")]
        public Guid? IdNumberCaption { get; set; }
        [Column("receipt_template")]
        public Guid? ReceiptTemplate { get; set; }
        [Column("FundsSource_caption")]
        public Guid? FundsSourceCaption { get; set; }

        [ForeignKey(nameof(AccountNameCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextAccountNameCaptionNavigations))]
        public virtual TextItem AccountNameCaptionNavigation { get; set; }
        [ForeignKey(nameof(AccountNumberCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextAccountNumberCaptionNavigations))]
        public virtual TextItem AccountNumberCaptionNavigation { get; set; }
        [ForeignKey(nameof(AliasAccountNameCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextAliasAccountNameCaptionNavigations))]
        public virtual TextItem AliasAccountNameCaptionNavigation { get; set; }
        [ForeignKey(nameof(AliasAccountNumberCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextAliasAccountNumberCaptionNavigations))]
        public virtual TextItem AliasAccountNumberCaptionNavigation { get; set; }
        [ForeignKey(nameof(DepositorNameCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextDepositorNameCaptionNavigations))]
        public virtual TextItem DepositorNameCaptionNavigation { get; set; }
        [ForeignKey(nameof(Disclaimer))]
        [InverseProperty(nameof(TextItem.TransactionTextDisclaimerNavigations))]
        public virtual TextItem DisclaimerNavigation { get; set; }
        [ForeignKey(nameof(FullInstructions))]
        [InverseProperty(nameof(TextItem.TransactionTextFullInstructionsNavigations))]
        public virtual TextItem FullInstructionsNavigation { get; set; }
        [ForeignKey(nameof(FundsSourceCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextFundsSourceCaptionNavigations))]
        public virtual TextItem FundsSourceCaptionNavigation { get; set; }
        [ForeignKey(nameof(IdNumberCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextIdNumberCaptionNavigations))]
        public virtual TextItem IdNumberCaptionNavigation { get; set; }
        [ForeignKey(nameof(ListItemCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextListItemCaptionNavigations))]
        public virtual TextItem ListItemCaptionNavigation { get; set; }
        [ForeignKey(nameof(NarrationCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextNarrationCaptionNavigations))]
        public virtual TextItem NarrationCaptionNavigation { get; set; }
        [ForeignKey(nameof(PhoneNumberCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextPhoneNumberCaptionNavigations))]
        public virtual TextItem PhoneNumberCaptionNavigation { get; set; }
        [ForeignKey(nameof(ReceiptTemplate))]
        [InverseProperty(nameof(TextItem.TransactionTextReceiptTemplateNavigations))]
        public virtual TextItem ReceiptTemplateNavigation { get; set; }
        [ForeignKey(nameof(ReferenceAccountNameCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextReferenceAccountNameCaptionNavigations))]
        public virtual TextItem ReferenceAccountNameCaptionNavigation { get; set; }
        [ForeignKey(nameof(ReferenceAccountNumberCaption))]
        [InverseProperty(nameof(TextItem.TransactionTextReferenceAccountNumberCaptionNavigations))]
        public virtual TextItem ReferenceAccountNumberCaptionNavigation { get; set; }
        [ForeignKey(nameof(Terms))]
        [InverseProperty(nameof(TextItem.TransactionTextTermsNavigations))]
        public virtual TextItem TermsNavigation { get; set; }
        [ForeignKey(nameof(TxItem))]
        [InverseProperty(nameof(TransactionTypeListItem.TransactionText))]
        public virtual TransactionTypeListItem TxItemNavigation { get; set; }
        [InverseProperty(nameof(TransactionTypeListItem.TxTextNavigation))]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
