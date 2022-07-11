using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("TransactionTypeList_TransactionTypeListItem")]
    [Index(nameof(TxtypeList), nameof(TxtypeListItem), Name = "UX_TransactionTypeList_TransactionTypeListItem_Item", IsUnique = true)]
    [Index(nameof(TxtypeList), nameof(ListOrder), Name = "UX_TransactionTypeList_TransactionTypeListItem_Order", IsUnique = true)]
    [Index(nameof(TxtypeList), Name = "itxtype_list_TransactionTypeList_TransactionTypeListItem")]
    [Index(nameof(TxtypeListItem), Name = "itxtype_list_item_TransactionTypeList_TransactionTypeListItem")]
    public partial class TransactionTypeListTransactionTypeListItem
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("txtype_list_item")]
        public int TxtypeListItem { get; set; }
        [Column("txtype_list")]
        public int TxtypeList { get; set; }
        [Column("list_order")]
        public int ListOrder { get; set; }

        [ForeignKey(nameof(TxtypeListItem))]
        [InverseProperty(nameof(TransactionTypeListItem.TransactionTypeListTransactionTypeListItems))]
        public virtual TransactionTypeListItem TxtypeListItemNavigation { get; set; }
        [ForeignKey(nameof(TxtypeList))]
        [InverseProperty(nameof(TransactionTypeList.TransactionTypeListTransactionTypeListItems))]
        public virtual TransactionTypeList TxtypeListNavigation { get; set; }
    }
}
