using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("WebUserLoginCount")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_WebUserLoginCount")]
    public partial class WebUserLoginCount
    {
        [Key]
        public Guid Oid { get; set; }
        [Column("modified", TypeName = "datetime")]
        public DateTime? Modified { get; set; }
        [Column("loginCount")]
        public int? LoginCount { get; set; }
        [StringLength(100)]
        public string User { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
    }
}
