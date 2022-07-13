﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    // [Index("PrinterId", Name = "iprinter_id_PrinterStatus")]
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
        [Column("error_name")]
        [StringLength(50)]
        public string ErrorName { get; set; } = null!;
        [Column("error_message")]
        [StringLength(50)]
        public string? ErrorMessage { get; set; }
        [Column("modified")]
        public DateTime Modified { get; set; }
        [Column("machine_name")]
        [StringLength(50)]
        public string MachineName { get; set; } = null!;
    }
}