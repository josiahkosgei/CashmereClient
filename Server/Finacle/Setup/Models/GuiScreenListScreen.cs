﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    [Table("GuiScreenList_Screen")]
    [Index("GuiScreenList", "ScreenOrder", Name = "UX_GuiScreenList_Screen_Order", IsUnique = true)]
    [Index("GuiScreenList", "Screen", Name = "UX_GuiScreenList_Screen_ScreenItem", IsUnique = true)]
    [Index("GuiScreenList", Name = "igui_screen_list_GuiScreenList_Screen")]
    [Index("Screen", Name = "iscreen_GuiScreenList_Screen")]
    [Index("ValidationListId", Name = "ivalidation_list_id_GuiScreenList_Screen")]
    public partial class GuiScreenListScreen
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("screen")]
        public int Screen { get; set; }
        [Column("gui_screen_list")]
        public int GuiScreenList { get; set; }
        [Column("screen_order")]
        public int ScreenOrder { get; set; }
        [Column("required")]
        public bool Required { get; set; }
        [Column("validation_list_id")]
        public Guid? ValidationListId { get; set; }

        [ForeignKey("GuiScreenList")]
        [InverseProperty("GuiScreenListScreens")]
        public virtual GuiscreenList GuiScreenListNavigation { get; set; }
        [ForeignKey("Screen")]
        [InverseProperty("GuiScreenListScreens")]
        public virtual Guiscreen ScreenNavigation { get; set; }
        [ForeignKey("ValidationListId")]
        [InverseProperty("GuiScreenListScreens")]
        public virtual ValidationList ValidationList { get; set; }
    }
}