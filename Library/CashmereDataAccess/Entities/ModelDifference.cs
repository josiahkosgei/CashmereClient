using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public class ModelDifference
    {
        public ModelDifference()
        {
            ModelDifferenceAspects = new HashSet<ModelDifferenceAspect>();
        }

        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string UserId { get; set; }
        [StringLength(100)]
        public string ContextId { get; set; }
        public int? Version { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
        
        public virtual ICollection<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
    }
}
