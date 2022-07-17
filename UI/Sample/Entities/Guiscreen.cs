﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("GUIScreen")]
    // [Index("GuiText", Name = "igui_text_GUIScreen")]
    // [Index("Type", Name = "itype_GUIScreen")]
    public partial class GUIScreen
    {
        public GUIScreen()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("description")]
        [StringLength(255)]
        public string? Description { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("keyboard")]
        public int? Keyboard { get; set; }
        [Column("is_masked")]
        public bool? IsMasked { get; set; }
        [Column("prefill_text")]
        [StringLength(50)]
        public string? PrefillText { get; set; }
        [Column("input_mask")]
        [StringLength(50)]
        [Unicode(false)]
        public string? InputMask { get; set; }
        [Column("gui_text")]
        public Guid? GuiText { get; set; }

        [ForeignKey("GuiText")]
        //[InverseProperty("GUIScreens")]
        public virtual GUIScreenText? GuiTextNavigation { get; set; }
        [ForeignKey("Type")]
        //[InverseProperty("GUIScreens")]
        public virtual GUIScreenType TypeNavigation { get; set; } = null!;
        //[InverseProperty("GUIScreen")]
        public virtual GUIScreenText GUIScreenText { get; set; } = null!;
        //[InverseProperty("ScreenNavigation")]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}