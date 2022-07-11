﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// Stores details of a customer deposit session. Asuccessful session ends in a successful transaction
    /// </summary>
    [Table("DepositorSession")]
    [Index(nameof(DeviceId), Name = "idevice_id_DepositorSession")]
    [Index(nameof(LanguageCode), Name = "ilanguage_code_DepositorSession")]
    public partial class DepositorSession
    {
        public DepositorSession()
        {
            ApplicationLogs = new HashSet<ApplicationLog>();
            SessionExceptions = new HashSet<SessionException>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("session_start")]
        public DateTime SessionStart { get; set; }
        [Column("session_end")]
        public DateTime? SessionEnd { get; set; }
        [Column("language_code")]
        [StringLength(5)]
        [Unicode(false)]
        public string LanguageCode { get; set; }
        [Column("complete")]
        public bool Complete { get; set; }
        [Column("complete_success")]
        public bool CompleteSuccess { get; set; }
        [Column("error_code")]
        public int? ErrorCode { get; set; }
        [Column("error_message")]
        [StringLength(255)]
        public string ErrorMessage { get; set; }
        [Column("terms_accepted")]
        public bool TermsAccepted { get; set; }
        [Column("account_verified")]
        public bool AccountVerified { get; set; }
        [Column("reference_account_verified")]
        public bool ReferenceAccountVerified { get; set; }
        [Column("salt")]
        [StringLength(64)]
        [Unicode(false)]
        public string Salt { get; set; }

        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("DepositorSessions")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(LanguageCode))]
        [InverseProperty(nameof(Language.DepositorSessions))]
        public virtual Language LanguageCodeNavigation { get; set; }
        [InverseProperty(nameof(ApplicationLog.Session))]
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        [InverseProperty(nameof(SessionException.Session))]
        public virtual ICollection<SessionException> SessionExceptions { get; set; }
        [InverseProperty(nameof(Transaction.Session))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
