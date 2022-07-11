
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
  public class Branch
  {
    public Branch()
    {
        Devices = new HashSet<Device>();
    }

    [Key]
    public Guid Id { get; set; }
    public string BranchCode { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Guid BankId { get; set; }
    [ForeignKey("BankId")]
    public virtual Bank Bank { get; set; }

    public virtual ICollection<Device> Devices { get; set; }
  }
}
