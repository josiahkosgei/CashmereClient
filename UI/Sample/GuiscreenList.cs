using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIScreenList")]
    public partial class GuiscreenList
    {
        public GuiscreenList()
        {
            Devices = new HashSet<Device>();
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
            TransactionTypeListItems = new HashSet<TransactionTypeListItem>();
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
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(Device.GuiscreenListNavigation))]
        public virtual ICollection<Device> Devices { get; set; }
        [InverseProperty(nameof(GuiScreenListScreen.GuiScreenListNavigation))]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        [InverseProperty(nameof(TransactionTypeListItem.TxTypeGuiscreenlistNavigation))]
        public virtual ICollection<TransactionTypeListItem> TransactionTypeListItems { get; set; }
    }
}
