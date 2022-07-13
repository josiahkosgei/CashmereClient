﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("CITPrintout")]
    [Index("CitId", Name = "iCIT_id_CITPrintout")]
    public partial class Citprintout
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }
        [Column("CIT_id")]
        public Guid CitId { get; set; }
        [Column("print_guid")]
        public Guid PrintGuid { get; set; }
        [Column("print_content")]
        public string? PrintContent { get; set; }
        [Column("is_copy")]
        public bool IsCopy { get; set; }

        [ForeignKey("CitId")]
        [InverseProperty("Citprintouts")]
        public virtual Cit Cit { get; set; } = null!;
    }
}