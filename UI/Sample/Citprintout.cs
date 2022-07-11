using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores CIT receipts
    /// </summary>
    [Table("CITPrintout")]
    [Index(nameof(CITId), Name = "icit_id_CITPrintout")]
    public partial class CITprintout
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("datetime")]
        public DateTime Datetime { get; set; }

        /// <summary>
        /// The CIT this rceipt belongs to
        /// </summary>
        [Column("cit_id")]
        public Guid CITId { get; set; }

        /// <summary>
        /// Receipt SHA512 hash
        /// </summary>
        [Column("print_guid")]
        public Guid PrintGuid { get; set; }

        /// <summary>
        /// Text of the receipt
        /// </summary>
        [Column("print_content")]
        public string PrintContent { get; set; }

        /// <summary>
        /// Is this CIT Receipt a copy, used for marking duplicate receipts
        /// </summary>
        [Column("is_copy")]
        public bool IsCopy { get; set; }

        [ForeignKey(nameof(CITId))]
        [InverseProperty("CITprintouts")]
        public virtual CIT CIT { get; set; }
    }
}
