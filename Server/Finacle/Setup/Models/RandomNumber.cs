﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    [Index("Value", Name = "UX_RandomNumbers", IsUnique = true)]
    public partial class RandomNumber
    {
        [Key]
        [Column("RowID")]
        public int RowId { get; set; }
        public int Value { get; set; }
    }
}