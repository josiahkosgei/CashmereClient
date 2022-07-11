using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("ApplicationException", Schema = "exp")]
    [Index(nameof(DeviceId), Name = "idevice_id_exp_ApplicationException_8807CB70")]
    public partial class ApplicationException1
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
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
