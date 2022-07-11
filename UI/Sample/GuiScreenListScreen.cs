using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GuiScreenList_Screen")]
    [Index(nameof(GuiScreenList), nameof(ScreenOrder), Name = "UX_GuiScreenList_Screen_Order", IsUnique = true)]
    [Index(nameof(GuiScreenList), nameof(Screen), Name = "UX_GuiScreenList_Screen_ScreenItem", IsUnique = true)]
    [Index(nameof(GuiScreenList), Name = "igui_screen_list_GuiScreenList_Screen")]
    [Index(nameof(GuiprepoplistId), Name = "iguiprepoplist_id_GuiScreenList_Screen")]
    [Index(nameof(Screen), Name = "iscreen_GuiScreenList_Screen")]
    [Index(nameof(ValidationListId), Name = "ivalidation_list_id_GuiScreenList_Screen")]
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
        [Column("guiprepoplist_id")]
        public Guid? GuiprepoplistId { get; set; }

        [ForeignKey(nameof(GuiScreenList))]
        [InverseProperty(nameof(GuiscreenList.GuiScreenListScreens))]
        public virtual GuiscreenList GuiScreenListNavigation { get; set; }
        [ForeignKey(nameof(GuiprepoplistId))]
        [InverseProperty(nameof(GuiprepopList.GuiScreenListScreens))]
        public virtual GuiprepopList Guiprepoplist { get; set; }
        [ForeignKey(nameof(Screen))]
        [InverseProperty(nameof(Guiscreen.GuiScreenListScreens))]
        public virtual Guiscreen ScreenNavigation { get; set; }
        [ForeignKey(nameof(ValidationListId))]
        [InverseProperty("GuiScreenListScreens")]
        public virtual ValidationList ValidationList { get; set; }
    }
}
