
// Type: CashmereDeposit.sysTextItemType

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("sysTextItemType", Schema = "xlns")]
    public class SysTextItemType
    {
        public SysTextItemType()
        {
            SysTextItems = new HashSet<SysTextItem>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SysTextItem> SysTextItems { get; set; }
    }
}
