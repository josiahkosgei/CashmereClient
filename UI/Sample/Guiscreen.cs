using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIScreen")]
    [Index(nameof(GuiText), Name = "igui_text_GUIScreen")]
    [Index(nameof(Type), Name = "itype_GUIScreen")]
    public partial class Guiscreen
    {
        public Guiscreen()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
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
        [Column("gui_text")]
        public Guid? GuiText { get; set; }

        [ForeignKey(nameof(GuiText))]
        [InverseProperty("Guiscreens")]
        public virtual GuiscreenText GuiTextNavigation { get; set; }
        [ForeignKey(nameof(Type))]
        [InverseProperty(nameof(GuiscreenType.Guiscreens))]
        public virtual GuiscreenType TypeNavigation { get; set; }
        [InverseProperty("Guiscreen")]
        public virtual GuiscreenText GuiscreenText { get; set; }
        [InverseProperty(nameof(GuiScreenListScreen.ScreenNavigation))]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
    }
}
