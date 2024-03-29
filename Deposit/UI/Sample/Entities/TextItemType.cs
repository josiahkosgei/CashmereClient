﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("TextItemType", Schema = "xlns")]
    [Index("Token", Name = "UX_UI_TextItemType", IsUnique = true)]
    public partial class TextItemType
    {
        public TextItemType()
        {
            TextItems = new HashSet<TextItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("token")]
        [StringLength(100)]
        public string Token { get; set; } = null!;
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }

        [InverseProperty("TextItemType")]
        public virtual ICollection<TextItem> TextItems { get; set; }
    }
}