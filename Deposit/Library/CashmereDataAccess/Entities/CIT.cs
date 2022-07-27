﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    [Table("CIT")]
    // [Index("AuthUser", Name = "iauth_user_CIT")]
    // [Index("DeviceId", Name = "idevice_id_CIT")]
    // [Index("StartUser", Name = "istart_user_CIT")]
    public partial class CIT
    {
        public CIT()
        {
            CITDenominations = new HashSet<CITDenomination>();
            CITPrintouts = new HashSet<CITPrintout>();
            CITTransactions = new HashSet<CITTransaction>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("device_id")]
        public Guid DeviceId { get; set; }
        [Column("cit_date")]
        public DateTime CITDate { get; set; }
        [Column("cit_complete_date")]
        public DateTime? CITCompleteDate { get; set; }
        [Column("start_user")]
        public Guid StartUser { get; set; }
        [Column("auth_user")]
        public Guid? AuthUser { get; set; }
        [Column("fromDate")]
        public DateTime? FromDate { get; set; }
        [Column("toDate")]
        public DateTime ToDate { get; set; }
        [Column("old_bag_number")]
        [StringLength(50)]
        public string? OldBagNumber { get; set; }
        [Column("new_bag_number")]
        [StringLength(50)]
        public string? NewBagNumber { get; set; }
        [Column("seal_number")]
        [StringLength(50)]
        public string? SealNumber { get; set; }
        [Column("complete")]
        public bool Complete { get; set; }
        [Column("cit_error")]
        public int CITError { get; set; }
        [Column("cit_error_message")]
        [StringLength(255)]
        public string? CITErrorMessage { get; set; }

        [ForeignKey("AuthUser")]
        //[InverseProperty("CITAuthUsers")]
        public virtual ApplicationUser? AuthUserNavigation { get; set; }
        [ForeignKey("DeviceId")]
        //[InverseProperty("CITs")]
        public virtual Device Device { get; set; } = null!;
        [ForeignKey("StartUser")]
        //[InverseProperty("CITStartUsers")]
        public virtual ApplicationUser StartUserNavigation { get; set; } = null!;
        //[InverseProperty("CIT")]
        public virtual ICollection<CITDenomination> CITDenominations { get; set; }
        //[InverseProperty("CIT")]
        public virtual ICollection<CITPrintout> CITPrintouts { get; set; }
        //[InverseProperty("CIT")]
        public virtual ICollection<CITTransaction> CITTransactions { get; set; }
        //[InverseProperty("CIT")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}