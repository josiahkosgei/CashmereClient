﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("TextItem", Schema = "xlns")]
    public partial class TextItem
    {
        public TextItem()
        {
            GUIPrepopItems = new HashSet<GUIPrepopItem>();
            GUIScreenTextBtnAcceptCaptions = new HashSet<GUIScreenText>();
            GUIScreenTextBtnBackCaptions = new HashSet<GUIScreenText>();
            GUIScreenTextBtnCancelCaptions = new HashSet<GUIScreenText>();
            GUIScreenTextFullInstructionss = new HashSet<GUIScreenText>();
            GUIScreenTextScreenTitleInstructions = new HashSet<GUIScreenText>();
            GUIScreenTextScreenTitles = new HashSet<GUIScreenText>();
            TextTranslations = new HashSet<TextTranslation>();
            TransactionTextAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextAliasAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextAliasAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextDepositorNameCaptions = new HashSet<TransactionText>();
            TransactionTextDisclaimers = new HashSet<TransactionText>();
            TransactionTextFullInstructionss = new HashSet<TransactionText>();
            TransactionTextFundsSourceCaptions = new HashSet<TransactionText>();
            TransactionTextIdNumberCaptions = new HashSet<TransactionText>();
            TransactionTextListItemCaptions = new HashSet<TransactionText>();
            TransactionTextNarrationCaptions = new HashSet<TransactionText>();
            TransactionTextPhoneNumberCaptions = new HashSet<TransactionText>();
            TransactionTextReceiptTemplates = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNameCaptions = new HashSet<TransactionText>();
            TransactionTextReferenceAccountNumberCaptions = new HashSet<TransactionText>();
            TransactionTextTerms = new HashSet<TransactionText>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(255)]
        public string? Description { get; set; }
        public string DefaultTranslation { get; set; } = null!;
        public Guid Category { get; set; }
        [Column("TextItemTypeID")]
        public Guid? TextItemTypeId { get; set; }

        [ForeignKey("Category")]
        public virtual TextItemCategory CategoryNavigation { get; set; } = null!;
        [ForeignKey("TextItemTypeId")]
        public virtual TextItemType? TextItemType { get; set; }
        public virtual ICollection<GUIPrepopItem> GUIPrepopItems { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextBtnAcceptCaptions { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextBtnBackCaptions { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextBtnCancelCaptions { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextFullInstructionss { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextScreenTitleInstructions { get; set; }
        public virtual ICollection<GUIScreenText> GUIScreenTextScreenTitles { get; set; }
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextAliasAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextDepositorNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextDisclaimers { get; set; }
        public virtual ICollection<TransactionText> TransactionTextFullInstructionss { get; set; }
        public virtual ICollection<TransactionText> TransactionTextFundsSourceCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextIdNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextListItemCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextNarrationCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextPhoneNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReceiptTemplates { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNameCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextReferenceAccountNumberCaptions { get; set; }
        public virtual ICollection<TransactionText> TransactionTextTerms { get; set; }
    }
}