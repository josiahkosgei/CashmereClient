using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Register a role to receive an alert
    /// </summary>
    [Table("AlertMessageRegistry")]
    [Index(nameof(AlertTypeId), nameof(RoleId), Name = "UX_AlertMessageRegistry", IsUnique = true)]
    [Index(nameof(AlertTypeId), Name = "ialert_type_id_AlertMessageRegistry")]
    [Index(nameof(RoleId), Name = "irole_id_AlertMessageRegistry")]
    public partial class AlertMessageRegistry
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// The type of alert the role can receive
        /// </summary>
        [Column("alert_type_id")]
        public int AlertTypeId { get; set; }

        /// <summary>
        /// The role that will be given rights to the AlertMssage type
        /// </summary>
        [Column("role_id")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Can the role receive email
        /// </summary>
        [Required]
        [Column("email_enabled")]
        public bool? EmailEnabled { get; set; }

        /// <summary>
        /// Can the role receive an SMS message for the alert message type
        /// </summary>
        [Column("phone_enabled")]
        public bool PhoneEnabled { get; set; }

        [ForeignKey(nameof(AlertTypeId))]
        [InverseProperty(nameof(AlertMessageType.AlertMessageRegistries))]
        public virtual AlertMessageType AlertType { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty("AlertMessageRegistries")]
        public virtual Role Role { get; set; }
    }
}
