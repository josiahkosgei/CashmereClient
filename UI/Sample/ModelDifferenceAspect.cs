using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ModelDifferenceAspect")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_ModelDifferenceAspect")]
    [Index(nameof(Owner), Name = "iOwner_ModelDifferenceAspect")]
    public partial class ModelDifferenceAspect
    {
        [Key]
        public Guid Oid { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public string Xml { get; set; }
        public Guid? Owner { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }

        [ForeignKey(nameof(Owner))]
        [InverseProperty(nameof(ModelDifference.ModelDifferenceAspects))]
        public virtual ModelDifference OwnerNavigation { get; set; }
    }
}
