using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class WebUserPasswordHistory
    {
        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string User { get; set; }
        public DateTime? Datetime { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
    }
}
