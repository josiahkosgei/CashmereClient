﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("AlertEmailAttachment")]
    public partial class AlertEmailAttachment
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("alert_email_id")]
        public Guid AlertEmailId { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("path")]
        [StringLength(255)]
        public string Path { get; set; } = null!;
        [Column("type")]
        [StringLength(6)]
        public string Type { get; set; } = null!;
        [Column("data")]
        public byte[] Data { get; set; } = null!;
        [Column("hash")]
        [MaxLength(64)]
        public byte[] Hash { get; set; } = null!;
    }
}