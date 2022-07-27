using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class ModelDifferenceAspect
    {
        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Xml { get; set; }
        public Guid? OwnerId { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual ModelDifference Owner { get; set; }
    }
}
