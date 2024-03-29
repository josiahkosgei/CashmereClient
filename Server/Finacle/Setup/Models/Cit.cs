﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    /// <summary>
    /// store a CIT transaction
    /// </summary>
    [Table("CIT")]
    [Index("AuthUser", Name = "iauth_user_CIT")]
    [Index("DeviceId", Name = "idevice_id_CIT")]
    [Index("StartUser", Name = "istart_user_CIT")]
    public partial class Cit
    {
        public Cit()
        {
            Citdenominations = new HashSet<Citdenomination>();
            Citprintouts = new HashSet<Citprintout>();
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
        public DateTime CitDate { get; set; }
        /// <summary>
        /// Datetime when the CIT was completed
        /// </summary>
        [Column("cit_complete_date")]
        public DateTime? CitCompleteDate { get; set; }
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
        public int CitError { get; set; }
        /// <summary>
        /// Error message encounterd during CIT
        /// </summary>
        [Column("cit_error_message")]
        [StringLength(255)]
        public string CitErrorMessage { get; set; }

        [ForeignKey("AuthUser")]
        [InverseProperty("CitAuthUserNavigations")]
        public virtual ApplicationUser AuthUserNavigation { get; set; }
        [ForeignKey("DeviceId")]
        [InverseProperty("Cits")]
        public virtual Device Device { get; set; }
        [ForeignKey("StartUser")]
        [InverseProperty("CitStartUserNavigations")]
        public virtual ApplicationUser StartUserNavigation { get; set; }
        [InverseProperty("Cit")]
        public virtual ICollection<Citdenomination> Citdenominations { get; set; }
        [InverseProperty("Cit")]
        public virtual ICollection<Citprintout> Citprintouts { get; set; }
        [InverseProperty("Cit")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}