using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Types of messages for alerts sent via email or phone
    /// </summary>
    [Table("AlertMessageType")]
    public partial class AlertMessageType
    {
        public AlertMessageType()
        {
            AlertEvent1s = new HashSet<AlertEvent1>();
            AlertMessageRegistries = new HashSet<AlertMessageRegistry>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the AlertMessage
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Title displayed in th eheader sction of messages
        /// </summary>
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// The HTML template that will be merged into later
        /// </summary>
        [Column("email_content_template")]
        public string EmailContentTemplate { get; set; }

        /// <summary>
        /// The raw text template that will be merged into later
        /// </summary>
        [Column("raw_email_content_template")]
        public string RawEmailContentTemplate { get; set; }

        /// <summary>
        /// The SMS template that will be merged into later
        /// </summary>
        [Column("phone_content_template")]
        [StringLength(255)]
        public string PhoneContentTemplate { get; set; }

        /// <summary>
        /// whether or not the alert message type in enabled and can be instantiated
        /// </summary>
        [Required]
        [Column("enabled")]
        public bool? Enabled { get; set; }

        [InverseProperty(nameof(AlertEvent1.AlertType))]
        public virtual ICollection<AlertEvent1> AlertEvent1s { get; set; }
        [InverseProperty(nameof(AlertMessageRegistry.AlertType))]
        public virtual ICollection<AlertMessageRegistry> AlertMessageRegistries { get; set; }
    }
}
