﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationException", Schema = "bak")]
    public partial class ApplicationException
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("code")]
        public int Code { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("level")]
        public int Level { get; set; }
        [Column("message")]
        [StringLength(255)]
        public string Message { get; set; }
        [Column("additional_info")]
        [StringLength(255)]
        public string AdditionalInfo { get; set; }
        [Column("stack")]
        public string Stack { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("ApplicationExceptions")]
        public virtual Device Device { get; set; }
    }
}
