using System.ComponentModel.DataAnnotations.Schema;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.Extensions;
using Cashmere.Library.CashmereDataAccess.Views;
using Microsoft.EntityFrameworkCore;
using ApplicationException = Cashmere.Library.CashmereDataAccess.Entities.ApplicationException;

namespace Cashmere.Library.CashmereDataAccess
{
    public partial class DepositorDBContext : DbContext
    {

        public DepositorDBContext()
        {
        }

        public DepositorDBContext(DbContextOptions<DepositorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; } = null!;
        public virtual DbSet<AlertAttachmentType> AlertAttachmentTypes { get; set; } = null!;
        public virtual DbSet<AlertEmail> AlertEmails { get; set; } = null!;
        public virtual DbSet<AlertEmailAttachment> AlertEmailAttachments { get; set; } = null!;
        public virtual DbSet<AlertEvent> AlertEvents { get; set; } = null!;
        public virtual DbSet<AlertMessageRegistry> AlertMessageRegistries { get; set; } = null!;
        public virtual DbSet<AlertMessageType> AlertMessageTypes { get; set; } = null!;
        public virtual DbSet<AlertSMS> AlertSMS { get; set; } = null!;
        public virtual DbSet<ApplicationException> ApplicationExceptions { get; set; } = null!;
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; } = null!;
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public virtual DbSet<Bank> Banks { get; set; } = null!;
        public virtual DbSet<Branch> Branches { get; set; } = null!;
        public virtual DbSet<CIT> CITs { get; set; } = null!;
        public virtual DbSet<CITDenomination> CITDenominations { get; set; } = null!;
        public virtual DbSet<CITPrintout> CITPrintouts { get; set; } = null!;
        public virtual DbSet<CITTransaction> CITTransactions { get; set; } = null!;
        public virtual DbSet<Config> Configs { get; set; } = null!;
        public virtual DbSet<ConfigCategory> ConfigCategories { get; set; } = null!;
        public virtual DbSet<ConfigGroup> ConfigGroups { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<CrashEvent> CrashEvents { get; set; } = null!;
        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; } = null!;
        public virtual DbSet<CurrencyListCurrency> CurrencyListCurrencies { get; set; } = null!;
        public virtual DbSet<DenominationDetail> DenominationDetails { get; set; } = null!;
        public virtual DbSet<DepositorSession> DepositorSessions { get; set; } = null!;
        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; } = null!;
        public virtual DbSet<DeviceConfig> DeviceConfigs { get; set; } = null!;
        public virtual DbSet<DeviceLock> DeviceLocks { get; set; } = null!;
        public virtual DbSet<DeviceLogin> DeviceLogins { get; set; } = null!;
        public virtual DbSet<DevicePrinter> DevicePrinters { get; set; } = null!;
        public virtual DbSet<DeviceStatus> DeviceStatus { get; set; } = null!;
        public virtual DbSet<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; } = null!;
        public virtual DbSet<DeviceType> DeviceTypes { get; set; } = null!;
        public virtual DbSet<EscrowJam> EscrowJams { get; set; } = null!;
        public virtual DbSet<GuiScreenListScreen> GuiScreenListScreens { get; set; } = null!;
        public virtual DbSet<GUIPrepopItem> GUIPrepopItems { get; set; } = null!;
        public virtual DbSet<GUIPrepopList> GUIPrepopLists { get; set; } = null!;
        public virtual DbSet<GUIPrepopListItem> GUIPrepopListItems { get; set; } = null!;
        public virtual DbSet<GUIScreen> GuiScreens { get; set; } = null!;
        public virtual DbSet<GUIScreenList> GuiScreenLists { get; set; } = null!;
        public virtual DbSet<GUIScreenText> GuiScreenTexts { get; set; } = null!;
        public virtual DbSet<GUIScreenType> GuiScreenTypes { get; set; } = null!;
        public virtual DbSet<Language> Languages { get; set; } = null!;
        public virtual DbSet<LanguageList> LanguageLists { get; set; } = null!;
        public virtual DbSet<LanguageListLanguage> LanguageListLanguages { get; set; } = null!;
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; } = null!;
        public virtual DbSet<PasswordPolicy> PasswordPolicies { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<PrinterStatus> PrinterStatus { get; set; } = null!;
        public virtual DbSet<Printout> Printouts { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<SessionException> SessionExceptions { get; set; } = null!;
        public virtual DbSet<SysTextItem> SysTextItems { get; set; } = null!;
        public virtual DbSet<SysTextItemCategory> SysTextItemCategories { get; set; } = null!;
        public virtual DbSet<SysTextItemType> SysTextItemTypes { get; set; } = null!;
        public virtual DbSet<SysTextTranslation> SysTextTranslations { get; set; } = null!;
        public virtual DbSet<TextItem> TextItems { get; set; } = null!;
        public virtual DbSet<TextItemCategory> TextItemCategories { get; set; } = null!;
        public virtual DbSet<TextItemType> TextItemTypes { get; set; } = null!;
        public virtual DbSet<TextTranslation> TextTranslations { get; set; } = null!;
        public virtual DbSet<ThisDevice> ThisDevices { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<TransactionException> TransactionExceptions { get; set; } = null!;
        public virtual DbSet<TransactionLimitList> TransactionLimitLists { get; set; } = null!;
        public virtual DbSet<TransactionLimitListItem> TransactionLimitListItems { get; set; } = null!;
        public virtual DbSet<TransactionText> TransactionTexts { get; set; } = null!;
        public virtual DbSet<TransactionType> TransactionTypes { get; set; } = null!;
        public virtual DbSet<TransactionTypeList> TransactionTypeLists { get; set; } = null!;
        public virtual DbSet<TransactionTypeListItem> TransactionTypeListItems { get; set; } = null!;
        public virtual DbSet<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; } = null!;
        public virtual DbSet<UptimeComponentState> UptimeComponentStates { get; set; } = null!;
        public virtual DbSet<UptimeMode> UptimeModes { get; set; } = null!;
        public virtual DbSet<UserGroup> UserGroups { get; set; } = null!;
        public virtual DbSet<UserLock> UserLocks { get; set; } = null!;
        public virtual DbSet<ValidationItem> ValidationItems { get; set; } = null!;
        public virtual DbSet<ValidationItemValue> ValidationItemValues { get; set; } = null!;
        public virtual DbSet<ValidationList> ValidationLists { get; set; } = null!;
        public virtual DbSet<ValidationListValidationItem> ValidationListValidationItems { get; set; } = null!;
        public virtual DbSet<ValidationText> ValidationTexts { get; set; } = null!;
        public virtual DbSet<ValidationType> ValidationTypes { get; set; } = null!;
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.UseModel(DepositorDBContextModel.Instance)
            .UseSqlServer(@"Data Source=.\;Initial Catalog=DepositorProduction;Integrated Security=True",
            options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingGeneratedFunctions(modelBuilder);
        }

    }
}
