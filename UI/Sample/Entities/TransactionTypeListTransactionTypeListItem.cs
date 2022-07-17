﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample.Entities
{
    [Table("TransactionTypeList_TransactionTypeListItem")]
    [Index("TxtypeList", "TxtypeListItem", Name = "UX_TransactionTypeList_TransactionTypeListItem_Item", IsUnique = true)]
    [Index("TxtypeList", "ListOrder", Name = "UX_TransactionTypeList_TransactionTypeListItem_Order", IsUnique = true)]
    [Index("TxtypeList", Name = "itxtype_list_TransactionTypeList_TransactionTypeListItem")]
    [Index("TxtypeListItem", Name = "itxtype_list_item_TransactionTypeList_TransactionTypeListItem")]
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

        [ForeignKey("TxtypeListItem")]
        [InverseProperty("TransactionTypeListTransactionTypeListItems")]
        public virtual TransactionTypeListItem TxtypeListItemNavigation { get; set; } = null!;
        [ForeignKey("TxtypeList")]
        [InverseProperty("TransactionTypeListTransactionTypeListItems")]
        public virtual TransactionTypeList TxtypeListNavigation { get; set; } = null!;
    }
}