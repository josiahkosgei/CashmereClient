using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("EscrowJam", Schema = "exp")]
    public partial class EscrowJam
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("transaction_id")]
        public Guid TransactionId { get; set; }
        [Column("date_detected")]
        public DateTime DateDetected { get; set; }
        [Column("dropped_amount")]
        public long DroppedAmount { get; set; }
        [Column("escrow_amount")]
        public long EscrowAmount { get; set; }
        [Column("posted_amount")]
        public long PostedAmount { get; set; }
        [Column("retreived_amount")]
        public long RetreivedAmount { get; set; }
        [Column("recovery_date")]
        public DateTime? RecoveryDate { get; set; }
        [Column("initialising_user")]
        public Guid? InitialisingUser { get; set; }
        [Column("authorising_user")]
        public Guid? AuthorisingUser { get; set; }
        [Column("additional_info")]
        [StringLength(100)]
        public string AdditionalInfo { get; set; }

        [ForeignKey(nameof(AuthorisingUser))]
        [InverseProperty(nameof(ApplicationUser.EscrowJamAuthorisingUserNavigations))]
        public virtual ApplicationUser AuthorisingUserNavigation { get; set; }
        [ForeignKey(nameof(InitialisingUser))]
        [InverseProperty(nameof(ApplicationUser.EscrowJamInitialisingUserNavigations))]
        public virtual ApplicationUser InitialisingUserNavigation { get; set; }
        [ForeignKey(nameof(TransactionId))]
        [InverseProperty("EscrowJams")]
        public virtual Transaction Transaction { get; set; }
    }
}
