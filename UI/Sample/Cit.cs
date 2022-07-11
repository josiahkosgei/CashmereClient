using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    /// <summary>
    /// store a CIT transaction
    /// </summary>
    [Table("CIT")]
    [Index(nameof(AuthUser), Name = "iauth_user_CIT")]
    [Index(nameof(DeviceId), Name = "idevice_id_CIT")]
    [Index(nameof(StartUser), Name = "istart_user_CIT")]
    public partial class CIT
    {
        public CIT()
        {
            CITdenominations = new HashSet<CITdenomination>();
            CITprintouts = new HashSet<CITprintout>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Device that conducted the CIT
        /// </summary>
        [Column("device_id")]
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Datetime of the CIT
        /// </summary>
        [Column("cit_date")]
        public DateTime CITDate { get; set; }

        /// <summary>
        /// Datetime when the CIT was completed
        /// </summary>
        [Column("cit_complete_date")]
        public DateTime? CITCompleteDate { get; set; }

        /// <summary>
        /// ApplicationUser who initiated the CIT
        /// </summary>
        [Column("start_user")]
        public Guid StartUser { get; set; }

        /// <summary>
        /// Application User who authorised the CIT event
        /// </summary>
        [Column("auth_user")]
        public Guid? AuthUser { get; set; }

        /// <summary>
        /// The datetime from which the CIT calculations will be carrid out
        /// </summary>
        [Column("fromDate")]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// The datetime until which the CIT calculations will be carrid out
        /// </summary>
        [Column("toDate")]
        public DateTime ToDate { get; set; }

        /// <summary>
        /// The asset number of the Bag that was removed i.e. the full bag
        /// </summary>
        [Column("old_bag_number")]
        [StringLength(50)]
        public string OldBagNumber { get; set; }

        /// <summary>
        /// The asset number of the empty bag that was inserted
        /// </summary>
        [Column("new_bag_number")]
        [StringLength(50)]
        public string NewBagNumber { get; set; }

        /// <summary>
        /// The numbr on the tamper evident seal tag used to seal the bag
        /// </summary>
        [Column("seal_number")]
        [StringLength(50)]
        public string SealNumber { get; set; }

        /// <summary>
        /// Has the CIT been completed, used for calculating incomplete CITs
        /// </summary>
        [Column("complete")]
        public bool Complete { get; set; }

        /// <summary>
        /// The error code encountered during CIT
        /// </summary>
        [Column("cit_error")]
        public int CITError { get; set; }

        /// <summary>
        /// Error message encounterd during CIT
        /// </summary>
        [Column("cit_error_message")]
        [StringLength(255)]
        public string CITErrorMessage { get; set; }

        [ForeignKey(nameof(AuthUser))]
        [InverseProperty(nameof(ApplicationUser.CITAuthUserNavigations))]
        public virtual ApplicationUser AuthUserNavigation { get; set; }
        [ForeignKey(nameof(DeviceId))]
        [InverseProperty("CITs")]
        public virtual Device Device { get; set; }
        [ForeignKey(nameof(StartUser))]
        [InverseProperty(nameof(ApplicationUser.CITStartUserNavigations))]
        public virtual ApplicationUser StartUserNavigation { get; set; }
        [InverseProperty(nameof(CITdenomination.CIT))]
        public virtual ICollection<CITdenomination> CITdenominations { get; set; }
        [InverseProperty(nameof(CITprintout.CIT))]
        public virtual ICollection<CITprintout> CITprintouts { get; set; }
        [InverseProperty(nameof(Transaction.CIT))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
