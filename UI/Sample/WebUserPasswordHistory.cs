﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("WebUserPasswordHistory")]
    [Index(nameof(Gcrecord), Name = "iGCRecord_WebUserPasswordHistory")]
    public partial class WebUserPasswordHistory
    {
        [Key]
        public Guid Oid { get; set; }
        [Column("user")]
        [StringLength(100)]
        public string User { get; set; }
        [Column("datetime", TypeName = "datetime")]
        public DateTime? Datetime { get; set; }
        [Column("password")]
        [StringLength(100)]
        public string Password { get; set; }
        public int? OptimisticLockField { get; set; }
        [Column("GCRecord")]
        public int? Gcrecord { get; set; }
    }
}
