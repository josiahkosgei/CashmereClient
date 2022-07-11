using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("GUIPrepopList_Item")]
    [Index(nameof(Item), nameof(List), Name = "UX_GUIPrepopList_Item", IsUnique = true)]
    [Index(nameof(ListOrder), nameof(List), Name = "UX_GUIPrepopList_ListOrder", IsUnique = true)]
    [Index(nameof(Item), Name = "iItem_GUIPrepopList_Item")]
    [Index(nameof(List), Name = "iList_GUIPrepopList_Item")]
    public partial class GuiprepopListItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        public Guid List { get; set; }
        public Guid Item { get; set; }
        [Column("List_Order")]
        public int ListOrder { get; set; }

        [ForeignKey(nameof(Item))]
        [InverseProperty(nameof(GuiprepopItem.GuiprepopListItems))]
        public virtual GuiprepopItem ItemNavigation { get; set; }
        [ForeignKey(nameof(List))]
        [InverseProperty(nameof(GuiprepopList.GuiprepopListItems))]
        public virtual GuiprepopList ListNavigation { get; set; }
    }
}
