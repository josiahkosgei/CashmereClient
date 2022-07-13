﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("UptimeComponentState")]
    public partial class UptimeComponentState
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device")]
        public Guid Device { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        [Column("end_date")]
        public DateTime? EndDate { get; set; }
        [Column("component_state")]
        public int ComponentState { get; set; }
    }
}