using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Entities
{
    public partial class Device
    {
        public DeviceSuspenseAccount? GetCITSuspenseAccount(string currency)
        {
            return this.DeviceCITSuspenseAccounts.FirstOrDefault(x =>
                x.Currency.Code.Equals(currency, StringComparison.OrdinalIgnoreCase));
        }

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
            DeviceCITSuspenseAccounts = new HashSet<DeviceSuspenseAccount>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string DeviceLocation { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(128)]
        public string MachineName { get; set; }
        public Guid BranchId { get; set; }
        public Guid AppId { get; set; }
        public byte[] AppKey { get; set; }

        [StringLength(255)]
        [Unicode(false)]
        public string Description { get; set; }
        public int TypeId { get; set; }
        public bool Enabled { get; set; }
        public int ConfigGroupId { get; set; }
        public int? UserGroupId { get; set; }
        public int GuiScreenListId { get; set; }
        public int? LanguageListId { get; set; }
        public int CurrencyListId { get; set; }
        public int TransactionTypeListId { get; set; }

        /// <summary>
        /// how many cycles of failed logins have been detected. used to lock the machine in case of password guessing
        /// </summary>
        public int LoginCycles { get; set; }

        /// <summary>
        /// how many times in a row a login attempt has failed
        /// </summary>
        public int LoginAttempts { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string Secret { get; set; }
        [StringLength(128)]
        [Unicode(false)]
        public string Password { get; set; }
        public string MacAddress { get; set; }

        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(ConfigGroupId))]
        public virtual ConfigGroup ConfigGroup { get; set; }
        [ForeignKey(nameof(CurrencyListId))]
        public virtual CurrencyList CurrencyList { get; set; }
        [ForeignKey(nameof(GuiScreenListId))]
        public virtual GuiScreenList GuiScreenList { get; set; }
        [ForeignKey(nameof(LanguageListId))]
        public virtual LanguageList LanguageList { get; set; }
        [ForeignKey(nameof(TransactionTypeListId))]
        public virtual TransactionTypeList TransactionTypeList { get; set; }
        [ForeignKey(nameof(TypeId))]
        [InverseProperty(nameof(Entities.DeviceType.Devices))]
        public virtual DeviceType DeviceType { get; set; }
        [ForeignKey(nameof(UserGroupId))]
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<ApplicationException> ApplicationExceptions { get; set; }
        public virtual ICollection<ApplicationLog> ApplicationLogs { get; set; }
        public virtual ICollection<CIT> CITs { get; set; }
        public virtual ICollection<CrashEvent> CrashEvents { get; set; }
        public virtual ICollection<DepositorSession> DepositorSessions { get; set; }
        public virtual ICollection<DeviceLock> DeviceLocks { get; set; }
        public virtual ICollection<DeviceLogin> DeviceLogins { get; set; }
        public virtual ICollection<DevicePrinter> DevicePrinters { get; set; }
        public virtual ICollection<DeviceSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
