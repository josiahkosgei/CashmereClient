using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PingRequest", Schema = "cb")]
    public partial class PingRequest
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime MessageDateTime { get; set; }
        [Required]
        [Column("RequestUUID")]
        [StringLength(50)]
        public string RequestUuid { get; set; }
        public bool Success { get; set; }
        public bool ServerOnline { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        public bool IsError { get; set; }
    }
}
