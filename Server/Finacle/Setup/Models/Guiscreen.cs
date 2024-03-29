﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    [Table("GUIScreen")]
    [Index("Type", Name = "itype_GUIScreen")]
    public partial class Guiscreen
    {
        public Guiscreen()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
            GuiscreenTexts = new HashSet<GuiscreenText>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("keyboard")]
        public int? Keyboard { get; set; }
        [Column("is_masked")]
        public bool? IsMasked { get; set; }
        /// <summary>
        /// Text to prefil in the textbox
        /// </summary>
        [Column("prefill_text")]
        [StringLength(50)]
        public string PrefillText { get; set; }
        [Column("input_mask")]
        [StringLength(50)]
        [Unicode(false)]
        public string InputMask { get; set; }

        [ForeignKey("Type")]
        [InverseProperty("Guiscreens")]
        public virtual GuiscreenType TypeNavigation { get; set; }
        [InverseProperty("ScreenNavigation")]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        [InverseProperty("Guiscreen")]
        public virtual ICollection<GuiscreenText> GuiscreenTexts { get; set; }
    }
}