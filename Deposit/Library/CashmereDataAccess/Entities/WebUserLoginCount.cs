using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class WebUserLoginCount
    {
        [Key]
        public Guid Oid { get; set; }
        public DateTime? Modified { get; set; }
        public int? LoginCount { get; set; }
        [StringLength(100)]
        public string User { get; set; }
        public int? OptimisticLockField { get; set; }
        public int? Gcrecord { get; set; }
    }
}
