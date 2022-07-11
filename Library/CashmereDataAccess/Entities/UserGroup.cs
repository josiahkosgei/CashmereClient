using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class UserGroup
    {
        public UserGroup()
        {
            ApplicationUsers = new HashSet<ApplicationUser>();
            Devices = new HashSet<Device>();
            ParentGroups = new HashSet<UserGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(512)]
        public string Description { get; set; }
        public int? ParentGroupId { get; set; }

        [ForeignKey(nameof(ParentGroupId))]
        public virtual UserGroup ParentGroup { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<UserGroup> ParentGroups { get; set; }
    }
}
