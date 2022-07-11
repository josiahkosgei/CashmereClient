
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class TransactionText
    {
       public TransactionText()
        {
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
        }

        [Key]
        public Guid Id { get; set; }
        public int TxItemId { get; set; }
        public Guid? DisclaimerId { get; set; }
        public Guid? TermsId { get; set; }
        public Guid? FullInstructionsId { get; set; }
        public Guid? ListItemCaptionId { get; set; }
        public Guid? AccountNumberCaptionId { get; set; }
        public Guid? AccountNameCaptionId { get; set; }
        public Guid? ReferenceAccountNumberCaptionId { get; set; }
        public Guid? ReferenceAccountNameCaptionId { get; set; }
        public Guid? NarrationCaptionId { get; set; }
        public Guid? AliasAccountNumberCaptionId { get; set; }
        public Guid? AliasAccountNameCaptionId { get; set; }
        public Guid? DepositorNameCaptionId { get; set; }
        public Guid? PhoneNumberCaptionId { get; set; }
        public Guid? IdNumberCaptionId { get; set; }
        public Guid? ReceiptTemplateId { get; set; }
        public Guid? FundsSourceCaptionId { get; set; }

        [ForeignKey(nameof(AccountNameCaptionId))]
        public virtual TextItem AccountNameCaption { get; set; }

        [ForeignKey(nameof(AccountNumberCaptionId))]
        public virtual TextItem AccountNumberCaption { get; set; }

        [ForeignKey(nameof(AliasAccountNameCaptionId))]
        public virtual TextItem AliasAccountNameCaption { get; set; }

        [ForeignKey(nameof(AliasAccountNumberCaptionId))]
        public virtual TextItem AliasAccountNumberCaption { get; set; }

        [ForeignKey(nameof(DepositorNameCaptionId))]
        public virtual TextItem DepositorNameCaption { get; set; }

        [ForeignKey(nameof(DisclaimerId))]
        public virtual TextItem Disclaimer { get; set; }

        [ForeignKey(nameof(FullInstructionsId))]
        public virtual TextItem FullInstructions { get; set; }
        
        [ForeignKey(nameof(FundsSourceCaptionId))]
        public virtual TextItem FundsSourceCaption { get; set; }

        [ForeignKey(nameof(IdNumberCaptionId))]
        public virtual TextItem IdNumberCaption { get; set; }

        [ForeignKey(nameof(ListItemCaptionId))]
        public virtual TextItem ListItemCaption { get; set; }

        [ForeignKey(nameof(NarrationCaptionId))]
        public virtual TextItem NarrationCaption { get; set; }

        [ForeignKey(nameof(PhoneNumberCaptionId))]
        public virtual TextItem PhoneNumberCaption { get; set; }

        [ForeignKey(nameof(ReceiptTemplateId))]
        public virtual TextItem ReceiptTemplate { get; set; }

        [ForeignKey(nameof(ReferenceAccountNameCaptionId))]
        public virtual TextItem ReferenceAccountNameCaption { get; set; }

        [ForeignKey(nameof(ReferenceAccountNumberCaptionId))]
        public virtual TextItem ReferenceAccountNumberCaption { get; set; }

        [ForeignKey(nameof(TermsId))]
        public virtual TextItem Terms { get; set; }

        [ForeignKey(nameof(TxItemId))]
        public virtual TransactionTypeListItem TxItem { get; set; }
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
