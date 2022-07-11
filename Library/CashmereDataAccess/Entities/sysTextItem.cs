
// Type: CashmereDeposit.sysTextItem

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextItem", Schema = "xlns")]
    public class SysTextItem
    {
        public SysTextItem()
        {
            SysTextTranslations = new HashSet<SysTextTranslation>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string DefaultTranslation { get; set; }

        public Guid CategoryId { get; set; }

        public Guid? TextItemTypeId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual SysTextItemCategory Category { get; set; }

        [ForeignKey(nameof(TextItemTypeId))]
        public virtual SysTextItemType TextItemType { get; set; }

        public virtual ICollection<SysTextTranslation> SysTextTranslations { get; set; }
    }
}
