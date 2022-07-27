﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Setup.Models
{
    [Table("Device")]
    [Index("GuiscreenList", Name = "iGUIScreen_list_DeviceList")]
    [Index("BranchId", Name = "ibranch_id_DeviceList")]
    [Index("ConfigGroup", Name = "iconfig_group_DeviceList")]
    [Index("CurrencyList", Name = "icurrency_list_DeviceList")]
    [Index("LanguageList", Name = "ilanguage_list_Device")]
    [Index("TransactionTypeList", Name = "itransaction_type_list_DeviceList")]
    [Index("TypeId", Name = "itype_id_DeviceList")]
    [Index("UserGroup", Name = "iuser_group_DeviceList")]
    public partial class Device
    {
        public Device()
        {
            AlertEvents = new HashSet<AlertEvent>();
            ApplicationException1s = new HashSet<ApplicationException1>();
            ApplicationExceptions = new HashSet<ApplicationException>();
            ApplicationLog1s = new HashSet<ApplicationLog1>();
            ApplicationLogs = new HashSet<ApplicationLog>();
            Cits = new HashSet<Cit>();
            CrashEvents = new HashSet<CrashEvent>();
            DepositorSessions = new HashSet<DepositorSession>();
            DeviceLocks = new HashSet<DeviceLock>();
            DevicePrinters = new HashSet<DevicePrinter>();
            DeviceStatuses = new HashSet<DeviceStatus>();
            DeviceSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("device_number")]
        [StringLength(50)]
        public string DeviceNumber { get; set; }
        [Required]
        [Column("device_location")]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column("machine_name")]
        [StringLength(128)]
        public string MachineName { get; set; }
        [Column("branch_id")]
        public Guid BranchId { get; set; }
        [Column("description")]
        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        [Column("type_id")]
        public int TypeId { get; set; }
        [Column("enabled")]
        public bool Enabled { get; set; }
        [Column("config_group")]
        public int ConfigGroup { get; set; }
        [Column("user_group")]
        public int? UserGroup { get; set; }
        [Column("GUIScreen_list")]
        public int GuiscreenList { get; set; }
        [Column("language_list")]
        public int? LanguageList { get; set; }
        [Column("currency_list")]
        public int CurrencyList { get; set; }
        [Column("transaction_type_list")]
        public int TransactionTypeList { get; set; }
        /// <summary>
        /// how many cycles of failed logins have been detected. used to lock the machine in case of password guessing
        /// </summary>
        [Column("login_cycles")]
        public int LoginCycles { get; set; }
        /// <summary>
        /// how many times in a row a login attempt has failed
        /// </summary>
        [Column("login_attempts")]
        public int LoginAttempts { get; set; }
        [Required]
        [Column("username")]
        [StringLength(128)]
        [Unicode(false)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(128)]
        [Unicode(false)]
        public string Password { get; set; }

        [ForeignKey("BranchId")]
        [InverseProperty("Devices")]
        public virtual Branch Branch { get; set; }
        [ForeignKey("ConfigGroup")]
        [InverseProperty("Devices")]
        public virtual ConfigGroup ConfigGroupNavigation { get; set; }
        [ForeignKey("CurrencyList")]
        [InverseProperty("Devices")]
        public virtual CurrencyList CurrencyListNavigation { get; set; }
        [ForeignKey("GuiscreenList")]
        [InverseProperty("Devices")]
        public virtual GuiscreenList GuiscreenListNavigation { get; set; }
        [ForeignKey("LanguageList")]
        [InverseProperty("Devices")]
        public virtual LanguageList LanguageListNavigation { get; set; }
        [ForeignKey("TransactionTypeList")]
        [InverseProperty("Devices")]
        public virtual TransactionTypeList TransactionTypeListNavigation { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("Devices")]
        public virtual DeviceType Type { get; set; }
        [ForeignKey("UserGroup")]
        [InverseProperty("Devices")]
        public virtual UserGroup UserGroupNavigation { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<AlertEvent> AlertEvents { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<ApplicationException1> ApplicationException1s { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<ApplicationException> ApplicationExceptions { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<ApplicationLog1> ApplicationLog1s { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<Cit> Cits { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<CrashEvent> CrashEvents { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<DevicePrinter> DevicePrinters { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<DeviceStatus> DeviceStatuses { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        [InverseProperty("Device")]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}