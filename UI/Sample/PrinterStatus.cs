using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("PrinterStatus")]
    [Index(nameof(PrinterId), Name = "iprinter_id_PrinterStatus")]
    public partial class PrinterStatus
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("printer_id")]
        public Guid PrinterId { get; set; }
        [Column("is_error")]
        public bool IsError { get; set; }
        [Column("has_paper")]
        public bool HasPaper { get; set; }
        [Column("cover_open")]
        public bool CoverOpen { get; set; }
        [Column("error_code")]
        public int ErrorCode { get; set; }
        [Required]
        [Column("error_name")]
        [StringLength(50)]
        public string ErrorName { get; set; }
        [Column("error_message")]
        [StringLength(50)]
        public string ErrorMessage { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Required]
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; }
    }
}
