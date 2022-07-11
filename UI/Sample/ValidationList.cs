﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// List of validations to be performed on a field
    /// </summary>
    [Table("ValidationList")]
    public partial class ValidationList
    {
        public ValidationList()
        {
            GuiScreenListScreens = new HashSet<GuiScreenListScreen>();
            ValidationListValidationItems = new HashSet<ValidationListValidationItem>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// common name for the transaction e.g. Mpesa Deposit
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        /// <summary>
        /// common description for the transaction type
        /// </summary>
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("category")]
        [StringLength(25)]
        public string Category { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }

        [InverseProperty(nameof(GuiScreenListScreen.ValidationList))]
        public virtual ICollection<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        [InverseProperty(nameof(ValidationListValidationItem.ValidationList))]
        public virtual ICollection<ValidationListValidationItem> ValidationListValidationItems { get; set; }
    }
}
