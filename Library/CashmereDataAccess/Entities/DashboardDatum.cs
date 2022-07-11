using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Index(nameof(Gcrecord), Name = "iGCRecord_DashboardData")]
    public class DashboardDatum
    {
        [Key]
        public Guid Oid { get; set; }
        public string Content { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        public bool? SynchronizeTitle { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
    }
}
