using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sample
{
    [Table("Device")]
    [Index(nameof(GuiscreenList), Name = "iGUIScreen_list_DeviceList")]
    [Index(nameof(BranchId), Name = "ibranch_id_DeviceList")]
    [Index(nameof(ConfigGroup), Name = "iconfig_group_DeviceList")]
    [Index(nameof(CurrencyList), Name = "icurrency_list_DeviceList")]
    [Index(nameof(LanguageList), Name = "ilanguage_list_Device")]
    [Index(nameof(TransactionTypeList), Name = "itransaction_type_list_DeviceList")]
    [Index(nameof(TypeId), Name = "itype_id_DeviceList")]
    [Index(nameof(UserGroup), Name = "iuser_group_DeviceList")]
    public partial class Device
    {
        public Device()
        {
            ApplicationExceptions = new HashSet<ApplicationException>();
            ApplicationLogs = new HashSet<ApplicationLog>();
            CITs = new HashSet<CIT>();
            CrashEvents = new HashSet<CrashEvent>();
            DepositorSessions = new HashSet<DepositorSession>();
            DeviceLocks = new HashSet<DeviceLock>();
            DeviceLogins = new HashSet<DeviceLogin>();
            DevicePrinters = new HashSet<DevicePrinter>();
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
        [Column("secret")]
        [StringLength(128)]
        [Unicode(false)]
        public string Secret { get; set; }
        [Column("password")]
        [StringLength(128)]
        [Unicode(false)]
        public string Password { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("Devices")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(ConfigGroup))]
        [InverseProperty("Devices")]
        public virtual ConfigGroup ConfigGroupNavigation { get; set; }
        [ForeignKey(nameof(CurrencyList))]
        [InverseProperty("Devices")]
        public virtual CurrencyList CurrencyListNavigation { get; set; }
        [ForeignKey(nameof(GuiscreenList))]
        [InverseProperty("Devices")]
        public virtual GuiscreenList GuiscreenListNavigation { get; set; }
        [ForeignKey(nameof(LanguageList))]
        [InverseProperty("Devices")]
        public virtual LanguageList LanguageListNavigation { get; set; }
        [ForeignKey(nameof(TransactionTypeList))]
        [InverseProperty("Devices")]
        public virtual TransactionTypeList TransactionTypeListNavigation { get; set; }
        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(DeviceType.Devices))]
        public virtual DeviceType Type { get; set; }
        [ForeignKey(nameof(UserGroup))]
        [InverseProperty("Devices")]
        public virtual UserGroup UserGroupNavigation { get; set; }
        [InverseProperty(nameof(ApplicationException.Device))]
        public virtual ICollection<ApplicationException> ApplicationExceptions { get; set; }
        [InverseProperty(nameof(ApplicationLog.Device))]
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        [InverseProperty(nameof(CIT.Device))]
        public virtual ICollection<CIT> CITs { get; set; }
        [InverseProperty(nameof(CrashEvent.Device))]
        public virtual ICollection<CrashEvent> CrashEvents { get; set; }
        [InverseProperty(nameof(DepositorSession.Device))]
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        [InverseProperty(nameof(DeviceLock.Device))]
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        [InverseProperty(nameof(DeviceLogin.Device))]
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        [InverseProperty(nameof(DevicePrinter.Device))]
        public virtual ICollection<DevicePrinter> DevicePrinters { get; set; }
        [InverseProperty(nameof(DeviceSuspenseAccount.Device))]
        public virtual ICollection<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        [InverseProperty(nameof(Transaction.Device))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
