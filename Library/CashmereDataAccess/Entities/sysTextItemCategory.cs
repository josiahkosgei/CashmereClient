
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextItemCategory", Schema = "xlns")]
    public class SysTextItemCategory
    {
        public SysTextItemCategory()
        {
            SysTextItems = new HashSet<SysTextItem>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual SysTextItemCategory Parent { get; set; }
        public virtual ICollection<SysTextItemCategory> Parents { get; set; }
        public virtual ICollection<SysTextItem> SysTextItems { get; set; }

    }
}
