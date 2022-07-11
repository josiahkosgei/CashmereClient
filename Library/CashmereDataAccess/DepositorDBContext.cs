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
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<AlertEmail> AlertEmails { get; set; }
        public virtual DbSet<AlertEmailResult> AlertEmailResults { get; set; }
        public virtual DbSet<AlertEmailAttachment> AlertEmailAttachments { get; set; }
        public virtual DbSet<AlertAttachmentType> AlertAttachmentTypes { get; set; }
        public virtual DbSet<AlertEvent> AlertEvents { get; set; }
        public virtual DbSet<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        public virtual DbSet<AlertMessageType> AlertMessageTypes { get; set; }
        public virtual DbSet<AlertSMS> AlertSMS { get; set; }
        public virtual DbSet<ApplicationException> ApplicationExceptions { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
        public virtual DbSet<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CashmereCommunicationServiceStatus> CashmereCommunicationServiceStatuses { get; set; }
        public virtual DbSet<CIT> CITs { get; set; }
        public virtual DbSet<CITDenomination> CITDenominations { get; set; }
        public virtual DbSet<CITPrintout> CITPrintouts { get; set; }
        public virtual DbSet<CITTransaction> CITTransactions { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<ConfigCategory> ConfigCategories { get; set; }
        public virtual DbSet<ConfigGroup> ConfigGroups { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CrashEvent> CrashEvents { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        public virtual DbSet<DashboardDatum> DashboardData { get; set; }
        public virtual DbSet<DenominationDetail> DenominationDetails { get; set; }
        public virtual DbSet<DenominationView> DenominationViews { get; set; }
        public virtual DbSet<DepositorSession> DepositorSessions { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }
        public virtual DbSet<DeviceConfig> DeviceConfigs { get; set; }
        public virtual DbSet<DeviceLock> DeviceLocks { get; set; }
        public virtual DbSet<DeviceLogin> DeviceLogins { get; set; }
        public virtual DbSet<DevicePrinter> DevicePrinters { get; set; }
        public virtual DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public virtual DbSet<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<EscrowJam> EscrowJams { get; set; }
        public virtual DbSet<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        public virtual DbSet<GuiPrepopItem> GuiPrepopItems { get; set; }
        public virtual DbSet<GuiPrepopList> GuiPrepopLists { get; set; }
        public virtual DbSet<GuiPrepopListItem> GuiPrepopListItems { get; set; }
        public virtual DbSet<GuiScreen> GuiScreens { get; set; }
        public virtual DbSet<GuiScreenList> GuiScreenLists { get; set; }
        public virtual DbSet<GuiScreenText> GuiScreenTexts { get; set; }
        public virtual DbSet<GuiScreenType> GuiScreenTypes { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageList> LanguageLists { get; set; }
        public virtual DbSet<LanguageListLanguage> LanguageListLanguages { get; set; }
        public virtual DbSet<ModelDifference> ModelDifferences { get; set; }
        public virtual DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<PasswordPolicy> PasswordPolicies { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PermissionPolicyMemberPermissionsObject> PermissionPolicyMemberPermissionsObjects { get; set; }
        public virtual DbSet<PermissionPolicyNavigationPermissionsObject> PermissionPolicyNavigationPermissionsObjects { get; set; }
        public virtual DbSet<PermissionPolicyObjectPermissionsObject> PermissionPolicyObjectPermissionsObjects { get; set; }
        public virtual DbSet<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
        public virtual DbSet<PermissionPolicyTypePermissionsObject> PermissionPolicyTypePermissionsObjects { get; set; }
        public virtual DbSet<PingRequest> PingRequests { get; set; }
        public virtual DbSet<PrinterStatus> PrinterStatuses { get; set; }
        public virtual DbSet<Printout> Printouts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SessionException> SessionExceptions { get; set; }
        public virtual DbSet<SysTextItem> SysTextItems { get; set; }
        public virtual DbSet<SysTextItemCategory> SysTextItemCategories { get; set; }
        public virtual DbSet<SysTextItemType> SysTextItemTypes { get; set; }
        public virtual DbSet<SysTextTranslation> SysTextTranslations { get; set; }
        public virtual DbSet<TextItem> TextItems { get; set; }
        public virtual DbSet<TextItemCategory> TextItemCategories { get; set; }
        public virtual DbSet<TextItemType> TextItemTypes { get; set; }
        public virtual DbSet<TextTranslation> TextTranslations { get; set; }
        public virtual DbSet<ThisDevice> ThisDevices { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionException> TransactionExceptions { get; set; }
        public virtual DbSet<TransactionLimitList> TransactionLimitLists { get; set; }
        public virtual DbSet<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        public virtual DbSet<TransactionText> TransactionTexts { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<TransactionTypeList> TransactionTypeLists { get; set; }
        public virtual DbSet<TransactionTypeListItem> TransactionTypeListItems { get; set; }
        public virtual DbSet<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        public virtual DbSet<TransactionView> TransactionViews { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserLock> UserLocks { get; set; }
        public virtual DbSet<UptimeComponentState> UptimeComponentStates { get; set; }
        public virtual DbSet<UptimeMode> UptimeModes { get; set; }
        public virtual DbSet<ValidationItem> ValidationItems { get; set; }
        public virtual DbSet<ValidationItemValue> ValidationItemValues { get; set; }
        public virtual DbSet<ValidationList> ValidationLists { get; set; }
        public virtual DbSet<ValidationListValidationItem> ValidationListValidationItems { get; set; }
        public virtual DbSet<ValidationText> ValidationTexts { get; set; }
        public virtual DbSet<ValidationType> ValidationTypes { get; set; }
        public virtual DbSet<ViewConfig> ViewConfigs { get; set; }
        public virtual DbSet<ViewPermission> ViewPermissions { get; set; }
        public virtual DbSet<WebPortalLogin> WebPortalLogins { get; set; }
        public virtual DbSet<WebPortalRole> WebPortalRoles { get; set; }
        public virtual DbSet<WebPortalRoleRolesApplicationUserApplicationUser> WebPortalRoleRolesApplicationUserApplicationUsers { get; set; }
        public virtual DbSet<WebUserLoginCount> WebUserLoginCounts { get; set; }
        public virtual DbSet<WebUserPasswordHistory> WebUserPasswordHistories { get; set; }
        public virtual DbSet<XpobjectType> XpobjectTypes { get; set; }

        public DepositorDBContext() : base()
        {
        }

        //public DepositorDBContext(DbContextOptions<DepositorDBContext> optionsBuilderOptions): base(optionsBuilderOptions)
        //{
        //}

        //public DepositorDBContext(DbContextOptions options) : base(options) { }
        public DepositorDBContext(DbContextOptions<DepositorDBContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\;Initial Catalog=DepositorDatabase;Integrated Security=True",
                options => options.EnableRetryOnFailure());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.Entity<Config>().Property(p => p.Id).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<YourEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            //modelBuilder.Entity<CIT>()
            //    .HasOne(g => g.AuthorisingUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(CIT.AuthUserId))
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<CIT>()
            //    .HasOne(g => g.StartUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(CIT.StartUserId))
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<CIT>()
            //    .HasIndex(q => q.AuthUserId)
            //    .IsUnique(false);

            //modelBuilder.Entity<CIT>()
            //    .HasIndex(q => q.StartUserId)
            //    .IsUnique(false);

            ///*-----------------------*/

            //modelBuilder.Entity<EscrowJam>()
            //    .HasOne(g => g.AuthorisingUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(EscrowJam.AuthorisinguserId))
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<EscrowJam>()
            //    .HasOne(g => g.InitialisingUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(EscrowJam.InitialisinguserId))
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<EscrowJam>()
            //    .HasIndex(q => q.AuthorisinguserId)
            //    .IsUnique(false);

            //modelBuilder.Entity<EscrowJam>()
            //    .HasIndex(q => q.InitialisinguserId)
            //    .IsUnique(false);

            ///*-----------------------*/

            //modelBuilder.Entity<TransactionPosting>()
            //    .HasOne(g => g.AuthorisingUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(TransactionPosting.AuthorisingUserId))
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<TransactionPosting>()
            //    .HasOne(g => g.InitialisingUser)
            //    .WithOne()
            //    .HasForeignKey<ApplicationUser>(nameof(TransactionPosting.InitialisingUserId))
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<TransactionPosting>()
            //    .HasIndex(q => q.AuthorisingUserId)
            //    .IsUnique(false);

            //modelBuilder.Entity<TransactionPosting>()
            //    .HasIndex(q => q.InitialisingUserId)
            //    .IsUnique(false);

            ///*-----------------------*/

            //modelBuilder.Entity<GuiScreen>()
            //    .HasMany(g => g.GuiScreenTexts)
            //    .WithOne()
            //    //.HasForeignKey<GUIScreenText>(nameof(GUIScreen))
            //    //.IsRequired(false) 
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<GuiScreen>()
            //    .HasOne(g => g.GuiScreenText)
            //    .WithOne()
            //    .HasForeignKey<GuiScreenText>(nameof(GuiScreen.GuiScreenTextId))
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<GuiScreen>()
            //    .HasIndex(q => q.GuiScreenTextId)
            //    .IsUnique(false);

            ////modelBuilder.Entity<GUIScreen>()
            ////    .HasIndex(q => q.initialising_user)
            ////    .IsUnique(false);

            ///*-----------------------*/
            //modelBuilder.Entity<DeviceLock>()
            //    .HasOne(e => e.Device)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            ///*-----------------------*/
            //modelBuilder.Entity<DeviceLogin>()
            //    .HasOne(e => e.Device)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            ///*-----------------------*/
            //modelBuilder.Entity<DevicePrinter>()
            //    .HasOne(e => e.Device)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            ///*-----------------------*/
            //modelBuilder.Entity<DeviceSuspenseAccount>()
            //    .HasOne(e => e.Device)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);

            ///*-----------------------*/
            //modelBuilder.Entity<Transaction>()
            //    .HasOne(e => e.Device)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);

            ///*-----------------------*/

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(e => e.Session)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            ///*-----------------------*/

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(e => e.TransactionTypeListItem)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            ///*-----------------------*/

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(e => e.DepositorSession)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            /*-----------------------*/

            //modelBuilder.Entity<Transaction>()
            //    .HasOne(e => e.DepositorSession)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Restrict);
            /*-----------------------*/

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
            //base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
