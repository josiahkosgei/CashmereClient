using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sample
{
    public partial class DepositorProductionBackupContext : DbContext
    {
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<AlertEmail> AlertEmails { get; set; }
        public virtual DbSet<AlertEmail1> AlertEmails1 { get; set; }
        public virtual DbSet<AlertEmailResult> AlertEmailResults { get; set; }
        public virtual DbSet<AlertEvent> AlertEvents { get; set; }
        public virtual DbSet<AlertEvent1> AlertEvents1 { get; set; }
        public virtual DbSet<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        public virtual DbSet<AlertMessageType> AlertMessageTypes { get; set; }
        public virtual DbSet<AlertSm> AlertSms { get; set; }
        public virtual DbSet<AlertSm1> AlertSms1 { get; set; }
        public virtual DbSet<ApplicationException> ApplicationExceptions { get; set; }
        public virtual DbSet<ApplicationException1> ApplicationExceptions1 { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<ApplicationLog1> ApplicationLogs1 { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationUserChangePassword> ApplicationUserChangePasswords { get; set; }
        public virtual DbSet<ApplicationUserLoginDetail> ApplicationUserLoginDetails { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CashmereCommunicationServiceStatus> CashmereCommunicationServiceStatuses { get; set; }
        public virtual DbSet<CIT> CITs { get; set; }
        public virtual DbSet<CITdenomination> CITdenominations { get; set; }
        public virtual DbSet<CITprintout> CITprintouts { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<ConfigCategory> ConfigCategories { get; set; }
        public virtual DbSet<ConfigGroup> ConfigGroups { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CrashEvent> CrashEvents { get; set; }
        public virtual DbSet<CrashEvent1> CrashEvents1 { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        public virtual DbSet<DashboardDatum> DashboardData { get; set; }
        public virtual DbSet<DenominationDetail> DenominationDetails { get; set; }
        public virtual DbSet<DenominationView> DenominationViews { get; set; }
        public virtual DbSet<DepositorSession> DepositorSessions { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceConfig> DeviceConfigs { get; set; }
        public virtual DbSet<DeviceLock> DeviceLocks { get; set; }
        public virtual DbSet<DeviceLogin> DeviceLogins { get; set; }
        public virtual DbSet<DevicePrinter> DevicePrinters { get; set; }
        public virtual DbSet<DeviceStatus> DeviceStatuses { get; set; }
        public virtual DbSet<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<EscrowJam> EscrowJams { get; set; }
        public virtual DbSet<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        public virtual DbSet<GuiprepopItem> GuiprepopItems { get; set; }
        public virtual DbSet<GuiprepopList> GuiprepopLists { get; set; }
        public virtual DbSet<GuiprepopListItem> GuiprepopListItems { get; set; }
        public virtual DbSet<Guiscreen> Guiscreens { get; set; }
        public virtual DbSet<GuiscreenList> GuiscreenLists { get; set; }
        public virtual DbSet<GuiscreenText> GuiscreenTexts { get; set; }
        public virtual DbSet<GuiscreenType> GuiscreenTypes { get; set; }
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
        public virtual DbSet<SessionException1> SessionExceptions1 { get; set; }
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
        public virtual DbSet<TransactionException1> TransactionExceptions1 { get; set; }
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

        public DepositorProductionBackupContext(DbContextOptions<DepositorProductionBackupContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\;Initial Catalog=DepositorProductionBackup;Integrated Security=True;Trust Server Certificate=True;Command Timeout=300");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasComment("a task a user needs permission to perform");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).HasComment("Short description of the activity being performed");

                entity.Property(e => e.Name).HasComment("The name of the activity. will be used in lookups");
            });

            modelBuilder.Entity<AlertEmail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AlertEmail1>(entity =>
            {
                entity.HasComment("Stores emails sent by the system");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AlertEventId).HasComment("Corresponding Alert that is tied to this email message");

                entity.Property(e => e.Attachments).HasComment("Pipe delimited list of filenames for files to attach when sending. Files must be accessible from the server");

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Datetime when the email message was created");

                entity.Property(e => e.From).HasComment("Email address of the sender");

                entity.Property(e => e.HtmlMessage).HasComment("The HTML formatted message");

                entity.Property(e => e.RawTextMessage).HasComment("The raw ANSI text version of the email for clients that do not support HTML emails e.g. mobile phones etc");

                entity.Property(e => e.SendDate).HasComment("Datetime when the email message was processed by the server");

                entity.Property(e => e.SendError).HasComment("Was there a fatal error during processing this email message");

                entity.Property(e => e.SendErrorMessage).HasComment("Error message returned by the server when email sending failed");

                entity.Property(e => e.Sent).HasComment("Whether or not the email message has been processed by the server");

                entity.Property(e => e.Subject).HasComment("Subject of the email");

                entity.Property(e => e.To).HasComment("Fills the \"To:\" heading of the email");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.AlertEmail1s)
                    .HasForeignKey(d => d.AlertEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertEmail_AlertEmailEvent");
            });

            modelBuilder.Entity<AlertEmailResult>(entity =>
            {
                entity.HasComment("Result of sending an alert email");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("(N'NEW')");
            });

            modelBuilder.Entity<AlertEvent>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<AlertEvent1>(entity =>
            {
                entity.HasComment("An event that has raised an alert. Various messages can be sent based on the alert raised e.g. SMS EMail etc");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AlertEventId).HasComment("if this alert is paired with a previous alert, it is linked here");

                entity.Property(e => e.AlertTypeId).HasComment("the type of alert");

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("The exact moment the alert was raised");

                entity.Property(e => e.DateDetected).HasComment("When was the alert detected, in case it is different from the created date. e.g. may indicate the event occured some other time, possibly before it was created in the db");

                entity.Property(e => e.DateResolved).HasComment("If tied to another Alert, this is when the the paired Alert was resolved e.g. a door close alert may resolve a previous door open alert");

                entity.Property(e => e.DeviceId).HasComment("The Device that raised the alert");

                entity.Property(e => e.IsProcessed).HasComment("has this alert been processed and messages created accordingly");

                entity.Property(e => e.IsProcessing).HasComment("is this alert currently being processed, used for concurrency control");

                entity.Property(e => e.IsResolved).HasComment("whether the Alert in qustion has been resolved or is still open");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.InverseAlertEvent)
                    .HasForeignKey(d => d.AlertEventId)
                    .HasConstraintName("FK_AlertEmailEvent_AlertEmailEvent");

                entity.HasOne(d => d.AlertType)
                    .WithMany(p => p.AlertEvent1s)
                    .HasForeignKey(d => d.AlertTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertEvent_AlertMessageType");
            });

            modelBuilder.Entity<AlertMessageRegistry>(entity =>
            {
                entity.HasComment("Register a role to receive an alert");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AlertTypeId).HasComment("The type of alert the role can receive");

                entity.Property(e => e.EmailEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Can the role receive email");

                entity.Property(e => e.PhoneEnabled).HasComment("Can the role receive an SMS message for the alert message type");

                entity.Property(e => e.RoleId).HasComment("The role that will be given rights to the AlertMssage type");

                entity.HasOne(d => d.AlertType)
                    .WithMany(p => p.AlertMessageRegistries)
                    .HasForeignKey(d => d.AlertTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertMessageRegistry_AlertMessageType");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AlertMessageRegistries)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertMessageRegistry_Role");
            });

            modelBuilder.Entity<AlertMessageType>(entity =>
            {
                entity.HasComment("Types of messages for alerts sent via email or phone");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EmailContentTemplate).HasComment("The HTML template that will be merged into later");

                entity.Property(e => e.Enabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("whether or not the alert message type in enabled and can be instantiated");

                entity.Property(e => e.Name).HasComment("Name of the AlertMessage");

                entity.Property(e => e.PhoneContentTemplate).HasComment("The SMS template that will be merged into later");

                entity.Property(e => e.RawEmailContentTemplate).HasComment("The raw text template that will be merged into later");

                entity.Property(e => e.Title).HasComment("Title displayed in th eheader sction of messages");
            });

            modelBuilder.Entity<AlertSm>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AlertSm1>(entity =>
            {
                entity.HasComment("AlertSmses");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AlertEventId).HasComment("the associated AlertEvent for this SMS message");

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Datetime when the SMS alert message was created by the system");

                entity.Property(e => e.From).HasComment("the number from which the SMS originates");

                entity.Property(e => e.Message).HasComment("the SMS text message to deliver");

                entity.Property(e => e.SendDate).HasComment("the datetime when the SMS message was processed");

                entity.Property(e => e.SendError).HasComment("was there a fatal rror during processing?");

                entity.Property(e => e.SendErrorMessage).HasComment("error mssage returned by the system while processing the SMS message");

                entity.Property(e => e.Sent).HasComment("whether or not the SMS message was processed");

                entity.Property(e => e.To).HasComment("Pipe delimited list of phone numbers to receive SMSes");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.AlertSm1s)
                    .HasForeignKey(d => d.AlertEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertSMS_AlertEvent");
            });

            modelBuilder.Entity<ApplicationException>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ApplicationExceptions)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationException_Device");
            });

            modelBuilder.Entity<ApplicationException1>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<ApplicationLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Component).IsUnicode(false);

                entity.Property(e => e.EventName).IsUnicode(false);

                entity.Property(e => e.EventType).IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.ApplicationLogs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationLog_Device");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.ApplicationLogs)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK_ApplicationLog_DepositorSession");
            });

            modelBuilder.Entity<ApplicationLog1>(entity =>
            {
                entity.HasComment("Stores the general application log that the GUI and other local systems write to");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Component).HasComment("Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc");

                entity.Property(e => e.EventDetail).HasComment("the details of the log message");

                entity.Property(e => e.EventName).HasComment("The name of the log event");

                entity.Property(e => e.EventType).HasComment("the type of the log event used for grouping and sorting");

                entity.Property(e => e.LogDate).HasComment("Datetime the system deems for the log entry.");

                entity.Property(e => e.LogLevel).HasComment("the LogLevel");

                entity.Property(e => e.SessionId).HasComment("The session this log entry belongs to");
            });

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Email).HasComment("user email address, used to receive emails from the system");

                entity.Property(e => e.EmailEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("whether or not the user is allowed to receive emails");

                entity.Property(e => e.Fname).HasComment("First names");

                entity.Property(e => e.Lname).HasComment("Last name");

                entity.Property(e => e.LoginAttempts).HasComment("how many unsuccessful login attempts has the user mad in a row. used to lock the user automatically");

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("salted and hashed password utilising a password library");

                entity.Property(e => e.PasswordResetRequired).HasComment("should the user rset their password at their next login");

                entity.Property(e => e.Phone).HasComment("the phone number for the user to rceive SMSes from the system");

                entity.Property(e => e.PhoneEnabled).HasComment("can the user receive SMSes from the system");

                entity.Property(e => e.RoleId).HasComment("The role the user has e.g. Custodian, Branch Manager tc");

                entity.Property(e => e.Username).HasComment("username for logging into the system");

                entity.HasOne(d => d.ApplicationUserLoginDetailNavigation)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.ApplicationUserLoginDetail)
                    .HasConstraintName("FK_ApplicationUser_ApplicationUserLoginDetail");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_Role");

                entity.HasOne(d => d.UserGroupNavigation)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.UserGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_UserGroup");
            });

            modelBuilder.Entity<ApplicationUserChangePassword>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.PasswordPolicyNavigation)
                    .WithMany(p => p.ApplicationUserChangePasswords)
                    .HasForeignKey(d => d.PasswordPolicy)
                    .HasConstraintName("FK_ApplicationUserChangePassword_PasswordPolicy");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ApplicationUserChangePasswords)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_ApplicationUserChangePassword_User");
            });

            modelBuilder.Entity<ApplicationUserLoginDetail>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.LastLoginLogEntryNavigation)
                    .WithMany(p => p.ApplicationUserLoginDetails)
                    .HasForeignKey(d => d.LastLoginLogEntry)
                    .HasConstraintName("FK_ApplicationUserLoginDetail_LastLoginLogEntry");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.ApplicationUserLoginDetails)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_ApplicationUserLoginDetail_User");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasComment("The bank that owns the depositor");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CountryCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.CountryCodeNavigation)
                    .WithMany(p => p.Banks)
                    .HasForeignKey(d => d.CountryCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bank_Country");
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Bank)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.BankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branch_Bank");
            });

            modelBuilder.Entity<CashmereCommunicationServiceStatus>(entity =>
            {
                entity.HasComment("status of the communication service for email, sms etc");

                entity.Property(e => e.Modified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<CIT>(entity =>
            {
                entity.HasComment("store a CIT transaction");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AuthUser).HasComment("Application User who authorised the CIT event");

                entity.Property(e => e.CITCompleteDate).HasComment("Datetime when the CIT was completed");

                entity.Property(e => e.CITDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Datetime of the CIT");

                entity.Property(e => e.CITError).HasComment("The error code encountered during CIT");

                entity.Property(e => e.CITErrorMessage).HasComment("Error message encounterd during CIT");

                entity.Property(e => e.Complete).HasComment("Has the CIT been completed, used for calculating incomplete CITs");

                entity.Property(e => e.DeviceId).HasComment("Device that conducted the CIT");

                entity.Property(e => e.FromDate).HasComment("The datetime from which the CIT calculations will be carrid out");

                entity.Property(e => e.NewBagNumber).HasComment("The asset number of the empty bag that was inserted");

                entity.Property(e => e.OldBagNumber).HasComment("The asset number of the Bag that was removed i.e. the full bag");

                entity.Property(e => e.SealNumber).HasComment("The numbr on the tamper evident seal tag used to seal the bag");

                entity.Property(e => e.StartUser).HasComment("ApplicationUser who initiated the CIT");

                entity.Property(e => e.ToDate).HasComment("The datetime until which the CIT calculations will be carrid out");

                entity.HasOne(d => d.AuthUserNavigation)
                    .WithMany(p => p.CITAuthUserNavigations)
                    .HasForeignKey(d => d.AuthUser)
                    .HasConstraintName("FK_CIT_ApplicationUser_AuthUser");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CITs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_DeviceList");

                entity.HasOne(d => d.StartUserNavigation)
                    .WithMany(p => p.CITStartUserNavigations)
                    .HasForeignKey(d => d.StartUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_ApplicationUser_StartUser");
            });

            modelBuilder.Entity<CITdenomination>(entity =>
            {
                entity.HasComment("currency and deomination breakdown of the CIT bag");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CITId).HasComment("The CIT the record belongs to");

                entity.Property(e => e.Count).HasComment("How many of the denomination were counted");

                entity.Property(e => e.CurrencyId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The currency code");

                entity.Property(e => e.Datetime).HasComment("When this item was recorded");

                entity.Property(e => e.Denom).HasComment("denomination of note or coin in major currency");

                entity.Property(e => e.Subtotal).HasComment("The subtotal of the denomination calculated as denom*count");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.CITdenominations)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITDenominations_CIT");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.CITdenominations)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITDenominations_Currency");
            });

            modelBuilder.Entity<CITprintout>(entity =>
            {
                entity.HasComment("Stores CIT receipts");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CITId).HasComment("The CIT this rceipt belongs to");

                entity.Property(e => e.Datetime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsCopy).HasComment("Is this CIT Receipt a copy, used for marking duplicate receipts");

                entity.Property(e => e.PrintContent).HasComment("Text of the receipt");

                entity.Property(e => e.PrintGuid).HasComment("Receipt SHA512 hash");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.CITprintouts)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITPrintout_CIT");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasComment("Configuration list");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Configs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Config_ConfigCategory");
            });

            modelBuilder.Entity<ConfigCategory>(entity =>
            {
                entity.HasComment("Categorisation of configuration opions");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Name).HasComment("Name of the AlertMessage");
            });

            modelBuilder.Entity<ConfigGroup>(entity =>
            {
                entity.HasComment("Group together configurations so devices can share configs");

                entity.HasOne(d => d.ParentGroupNavigation)
                    .WithMany(p => p.InverseParentGroupNavigation)
                    .HasForeignKey(d => d.ParentGroup)
                    .HasConstraintName("FK_ConfigGroup_ConfigGroup");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryCode)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.CountryName)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CrashEvent>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CrashEvents)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CrashEvent_Device");
            });

            modelBuilder.Entity<CrashEvent1>(entity =>
            {
                entity.HasComment("contains a crash report");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasComment("Currency enumeration");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("ISO 4217 Three Character Currency Code");

                entity.Property(e => e.Enabled).HasComment("whether the system supports the language");

                entity.Property(e => e.Flag).HasComment("two character country code for the national flag to display for the language");

                entity.Property(e => e.Minor).HasComment("Expresses the relationship between a major currency unit and its corresponding minor currency unit. This mechanism is called the currency \"exponent\" and assumes a base of 10. Will be used with converters in the GUI");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("Name of the currency");
            });

            modelBuilder.Entity<CurrencyList>(entity =>
            {
                entity.HasComment("Enumeration of allowed Currencies. A device can then associate with a currency list");

                entity.Property(e => e.DefaultCurrency)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DefaultCurrencyNavigation)
                    .WithMany(p => p.CurrencyLists)
                    .HasForeignKey(d => d.DefaultCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_Currency");
            });

            modelBuilder.Entity<CurrencyListCurrency>(entity =>
            {
                entity.HasComment("[m2m] Currency and CurrencyList");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyItem)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The currency in the list");

                entity.Property(e => e.CurrencyList).HasComment("The Currency list to which the currency is associated");

                entity.Property(e => e.CurrencyOrder).HasComment("ASC Order of sorting for currencies in list.");

                entity.HasOne(d => d.CurrencyItemNavigation)
                    .WithMany(p => p.CurrencyListCurrencies)
                    .HasForeignKey(d => d.CurrencyItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Currency_CurrencyList_Currency");

                entity.HasOne(d => d.CurrencyListNavigation)
                    .WithMany(p => p.CurrencyListCurrencies)
                    .HasForeignKey(d => d.CurrencyList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Currency_CurrencyList_CurrencyList");
            });

            modelBuilder.Entity<DashboardDatum>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();
            });

            modelBuilder.Entity<DenominationDetail>(entity =>
            {
                entity.HasComment("Denomination enumeration for a Transaction");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Count).HasComment("How many of the denomination were counted");

                entity.Property(e => e.Denom).HasComment("denomination of note or coin in major currency");

                entity.Property(e => e.Subtotal).HasComment("The subtotal of the denomination calculated as denom*count");

                entity.HasOne(d => d.Tx)
                    .WithMany(p => p.DenominationDetails)
                    .HasForeignKey(d => d.TxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DenominationDetail_Transaction");
            });

            modelBuilder.Entity<DenominationView>(entity =>
            {
                entity.ToView("DenominationView");
            });

            modelBuilder.Entity<DepositorSession>(entity =>
            {
                entity.HasComment("Stores details of a customer deposit session. Asuccessful session ends in a successful transaction");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Salt).IsUnicode(false);

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DepositorSessions)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DepositorSession_DeviceList");

                entity.HasOne(d => d.LanguageCodeNavigation)
                    .WithMany(p => p.DepositorSessions)
                    .HasForeignKey(d => d.LanguageCode)
                    .HasConstraintName("FK_DepositorSession_Language");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.GuiscreenList).HasDefaultValueSql("((1))");

                entity.Property(e => e.LoginAttempts).HasComment("how many times in a row a login attempt has failed");

                entity.Property(e => e.LoginCycles).HasComment("how many cycles of failed logins have been detected. used to lock the machine in case of password guessing");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Secret)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TransactionTypeList).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_Branch");

                entity.HasOne(d => d.ConfigGroupNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.ConfigGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_ConfigGroup");

                entity.HasOne(d => d.CurrencyListNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.CurrencyList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_CurrencyList");

                entity.HasOne(d => d.GuiscreenListNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.GuiscreenList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_GUIScreenList");

                entity.HasOne(d => d.LanguageListNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.LanguageList)
                    .HasConstraintName("FK_Device_LanguageList");

                entity.HasOne(d => d.TransactionTypeListNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.TransactionTypeList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_TransactionTypeList");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_DeviceType");

                entity.HasOne(d => d.UserGroupNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.UserGroup)
                    .HasConstraintName("FK_DeviceList_UserGroup");
            });

            modelBuilder.Entity<DeviceConfig>(entity =>
            {
                entity.HasComment("Link a Device to its configuration");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Config)
                    .WithMany(p => p.DeviceConfigs)
                    .HasForeignKey(d => d.ConfigId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceConfig_Config");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.DeviceConfigs)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceConfig_ConfigGroup");
            });

            modelBuilder.Entity<DeviceLock>(entity =>
            {
                entity.HasComment("Record device locking and unlocking activity");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceLocks)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceLock_Device");
            });

            modelBuilder.Entity<DeviceLogin>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceLogins)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceLogin_Device");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.DeviceLogins)
                    .HasForeignKey(d => d.User)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceLogin_ApplicationUser");
            });

            modelBuilder.Entity<DevicePrinter>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.IsInfront)
                    .HasDefaultValueSql("((1))")
                    .HasComment("Is the printer in the front i.e. customer facing or in the rear i.e. custodian facing");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DevicePrinters)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DevicePrinter_DeviceList");
            });

            modelBuilder.Entity<DeviceStatus>(entity =>
            {
                entity.HasComment("Current State of the device");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BaCurrency)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BagNoteCapacity).IsFixedLength();
            });

            modelBuilder.Entity<DeviceSuspenseAccount>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.CurrencyCodeNavigation)
                    .WithMany(p => p.DeviceSuspenseAccounts)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSuspenseAccount_Currency");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceSuspenseAccounts)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSuspenseAccount_DeviceList");
            });

            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasComment("Describes the type of device");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<EscrowJam>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.AuthorisingUserNavigation)
                    .WithMany(p => p.EscrowJamAuthorisingUserNavigations)
                    .HasForeignKey(d => d.AuthorisingUser)
                    .HasConstraintName("FK_EscrowJam_AppUser_Approver");

                entity.HasOne(d => d.InitialisingUserNavigation)
                    .WithMany(p => p.EscrowJamInitialisingUserNavigations)
                    .HasForeignKey(d => d.InitialisingUser)
                    .HasConstraintName("FK_EscrowJam_AppUser_Initiator");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.EscrowJams)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EscrowJam_Transaction");
            });

            modelBuilder.Entity<GuiScreenListScreen>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.GuiScreenListNavigation)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.GuiScreenList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIScreenList");

                entity.HasOne(d => d.Guiprepoplist)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.GuiprepoplistId)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIPrepopList");

                entity.HasOne(d => d.ScreenNavigation)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.Screen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIScreen");

                entity.HasOne(d => d.ValidationList)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.ValidationListId)
                    .HasConstraintName("FK_GuiScreenList_Screen_ValidationList");
            });

            modelBuilder.Entity<GuiprepopItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ValueNavigation)
                    .WithMany(p => p.GuiprepopItems)
                    .HasForeignKey(d => d.Value)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopItem_TextItem");
            });

            modelBuilder.Entity<GuiprepopList>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.UseDefault).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GuiprepopListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.GuiprepopListItems)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopItem");

                entity.HasOne(d => d.ListNavigation)
                    .WithMany(p => p.GuiprepopListItems)
                    .HasForeignKey(d => d.List)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopList");
            });

            modelBuilder.Entity<Guiscreen>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.InputMask).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PrefillText).HasComment("Text to prefil in the textbox");

                entity.HasOne(d => d.GuiTextNavigation)
                    .WithMany(p => p.Guiscreens)
                    .HasForeignKey(d => d.GuiText)
                    .HasConstraintName("FK_GUIScreen_GUIScreenText");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Guiscreens)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreen_GUIScreenType");
            });

            modelBuilder.Entity<GuiscreenList>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<GuiscreenText>(entity =>
            {
                entity.HasComment("Stores the text for a screen for a language");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.GuiscreenId).HasComment("The GUIScreen this entry corresponds to");

                entity.HasOne(d => d.BtnAcceptCaptionNavigation)
                    .WithMany(p => p.GuiscreenTextBtnAcceptCaptionNavigations)
                    .HasForeignKey(d => d.BtnAcceptCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_accept_caption");

                entity.HasOne(d => d.BtnBackCaptionNavigation)
                    .WithMany(p => p.GuiscreenTextBtnBackCaptionNavigations)
                    .HasForeignKey(d => d.BtnBackCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_back_caption");

                entity.HasOne(d => d.BtnCancelCaptionNavigation)
                    .WithMany(p => p.GuiscreenTextBtnCancelCaptionNavigations)
                    .HasForeignKey(d => d.BtnCancelCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_cancel_caption");

                entity.HasOne(d => d.FullInstructionsNavigation)
                    .WithMany(p => p.GuiscreenTextFullInstructionsNavigations)
                    .HasForeignKey(d => d.FullInstructions)
                    .HasConstraintName("FK_GUIScreenText_full_instructions");

                entity.HasOne(d => d.Guiscreen)
                    .WithOne(p => p.GuiscreenText)
                    .HasForeignKey<GuiscreenText>(d => d.GuiscreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_GUIScreen");

                entity.HasOne(d => d.ScreenTitleNavigation)
                    .WithMany(p => p.GuiscreenTextScreenTitleNavigations)
                    .HasForeignKey(d => d.ScreenTitle)
                    .HasConstraintName("FK_GUIScreenText_screen_title");

                entity.HasOne(d => d.ScreenTitleInstructionNavigation)
                    .WithMany(p => p.GuiscreenTextScreenTitleInstructionNavigations)
                    .HasForeignKey(d => d.ScreenTitleInstruction)
                    .HasConstraintName("FK_GUIScreenText_screen_title_instruction");
            });

            modelBuilder.Entity<GuiscreenType>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Languages");

                entity.HasComment("Available languages in the system");

                entity.Property(e => e.Code)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Enabled).HasComment("whether the system supports the language");

                entity.Property(e => e.Flag).HasComment("two character country code for the national flag to display for the language");

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<LanguageList>(entity =>
            {
                entity.HasComment("A list of languages a device supports");

                entity.Property(e => e.DefaultLanguage)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DefaultLanguageNavigation)
                    .WithMany(p => p.LanguageLists)
                    .HasForeignKey(d => d.DefaultLanguage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language");
            });

            modelBuilder.Entity<LanguageListLanguage>(entity =>
            {
                entity.HasComment("[m2m] LanguageList and Language");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageItem)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.LanguageItemNavigation)
                    .WithMany(p => p.LanguageListLanguages)
                    .HasForeignKey(d => d.LanguageItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language_Language");

                entity.HasOne(d => d.LanguageListNavigation)
                    .WithMany(p => p.LanguageListLanguages)
                    .HasForeignKey(d => d.LanguageList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language_LanguageList");
            });

            modelBuilder.Entity<ModelDifference>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();
            });

            modelBuilder.Entity<ModelDifferenceAspect>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.ModelDifferenceAspects)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK_ModelDifferenceAspect_Owner");
            });

            modelBuilder.Entity<PasswordHistory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.UserNavigation)
                    .WithMany(p => p.PasswordHistories)
                    .HasForeignKey(d => d.User)
                    .HasConstraintName("FK_PasswordHistory_User");
            });

            modelBuilder.Entity<PasswordPolicy>(entity =>
            {
                entity.HasComment("The system password policy");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.UseHistory).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasComment("grant a role to perform an activity");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_Activity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Permission_Role");
            });

            modelBuilder.Entity<PermissionPolicyMemberPermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.TypePermissionObjectNavigation)
                    .WithMany(p => p.PermissionPolicyMemberPermissionsObjects)
                    .HasForeignKey(d => d.TypePermissionObject)
                    .HasConstraintName("FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyNavigationPermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.PermissionPolicyNavigationPermissionsObjects)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_PermissionPolicyNavigationPermissionsObject_Role");
            });

            modelBuilder.Entity<PermissionPolicyObjectPermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.TypePermissionObjectNavigation)
                    .WithMany(p => p.PermissionPolicyObjectPermissionsObjects)
                    .HasForeignKey(d => d.TypePermissionObject)
                    .HasConstraintName("FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyRole>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.ObjectTypeNavigation)
                    .WithMany(p => p.PermissionPolicyRoles)
                    .HasForeignKey(d => d.ObjectType)
                    .HasConstraintName("FK_PermissionPolicyRole_ObjectType");
            });

            modelBuilder.Entity<PermissionPolicyTypePermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.PermissionPolicyTypePermissionsObjects)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_PermissionPolicyTypePermissionsObject_Role");
            });

            modelBuilder.Entity<PingRequest>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<PrinterStatus>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Modified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Printout>(entity =>
            {
                entity.HasComment("Stores contents of a printout for a transaction");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Datetime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Tx)
                    .WithMany(p => p.Printouts)
                    .HasForeignKey(d => d.TxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Printout_Transaction");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasComment("a user's role storing all their permissions");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<SessionException>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.SessionExceptions)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SessionException_DepositorSession");
            });

            modelBuilder.Entity<SessionException1>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<SysTextItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.SysTextItems)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SysTextItem_SysTextItemCategory");

                entity.HasOne(d => d.TextItemType)
                    .WithMany(p => p.SysTextItems)
                    .HasForeignKey(d => d.TextItemTypeId)
                    .HasConstraintName("FK_sysTextItem_sysTextItemType");
            });

            modelBuilder.Entity<SysTextItemCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_TextItemCategory_TextItemCategory");
            });

            modelBuilder.Entity<SysTextItemType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<SysTextTranslation>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.LanguageCodeNavigation)
                    .WithMany(p => p.SysTextTranslations)
                    .HasForeignKey(d => d.LanguageCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_sysTextTranslation_Language");

                entity.HasOne(d => d.SysTextItem)
                    .WithMany(p => p.SysTextTranslations)
                    .HasForeignKey(d => d.SysTextItemId)
                    .HasConstraintName("FK_sysTextTranslation_sysTextItem");
            });

            modelBuilder.Entity<TextItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.TextItems)
                    .HasForeignKey(d => d.Category)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UI_TextItem_TextItemCategory");

                entity.HasOne(d => d.TextItemType)
                    .WithMany(p => p.TextItems)
                    .HasForeignKey(d => d.TextItemTypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UI_TextItem_TextItemType");
            });

            modelBuilder.Entity<TextItemCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ParentNavigation)
                    .WithMany(p => p.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .HasConstraintName("FK_UI_TextItemCategory_TextItemCategory");
            });

            modelBuilder.Entity<TextItemType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TextTranslation>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.LanguageCodeNavigation)
                    .WithMany(p => p.TextTranslations)
                    .HasForeignKey(d => d.LanguageCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UI_Translation_Language");

                entity.HasOne(d => d.TextItem)
                    .WithMany(p => p.TextTranslations)
                    .HasForeignKey(d => d.TextItemId)
                    .HasConstraintName("FK_UI_Translation_TextItem");
            });

            modelBuilder.Entity<ThisDevice>(entity =>
            {
                entity.ToView("ThisDevice");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasComment("Stores the summary of a transaction attempt. A transaction can have various stages of completion if an error is encountered.");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("(newsequentialid())")
                    .HasComment("Globally Unique Identifier for replication");

                entity.Property(e => e.CbAccountName)
                    .IsUnicode(false)
                    .HasComment("The account name returned by core banking.");

                entity.Property(e => e.CbDate).HasComment("Core banking returned transaction date and time");

                entity.Property(e => e.CbRefAccountName)
                    .IsUnicode(false)
                    .HasComment("Core banking returned Reference Account Name if any following a validation request for a Reference Account Number");

                entity.Property(e => e.CbStatusDetail).HasComment("Additional status details returned by core banking e.g. 'Amount must be less that MAX_AMOUNT'");

                entity.Property(e => e.CbTxNumber)
                    .IsUnicode(false)
                    .HasComment("Core banking returned transaction number");

                entity.Property(e => e.CbTxStatus)
                    .IsUnicode(false)
                    .HasComment("Core banking returned transaction status e.g. SUCCESS or FAILURE");

                entity.Property(e => e.SessionId).HasComment("The session this transaction fullfills");

                entity.Property(e => e.TxAccountNumber)
                    .IsUnicode(false)
                    .HasComment("Account Number for crediting. This can be a suspense account");

                entity.Property(e => e.TxCompleted).HasComment("Indicate if the transaction has completed or is in progress");

                entity.Property(e => e.TxCurrency)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("User selected currency. A transaction can only have one currency at a time");

                entity.Property(e => e.TxDepositorName)
                    .IsUnicode(false)
                    .HasComment("Customer's name");

                entity.Property(e => e.TxEndDate).HasComment("The date and time the transaction was recorded in the database. Can be different from core banking's transaction date");

                entity.Property(e => e.TxErrorCode).HasComment("Last error code encountered during the transaction");

                entity.Property(e => e.TxErrorMessage)
                    .IsUnicode(false)
                    .HasComment("Last error message encountered during the transaction");

                entity.Property(e => e.TxIdNumber)
                    .IsUnicode(false)
                    .HasComment("Customer's ID number");

                entity.Property(e => e.TxNarration)
                    .IsUnicode(false)
                    .HasComment("The narration from the deposit slip. Usually set to 16 characters in core banking");

                entity.Property(e => e.TxPhone)
                    .IsUnicode(false)
                    .HasComment("Customer entered phone number");

                entity.Property(e => e.TxRefAccount)
                    .IsUnicode(false)
                    .HasComment("Used for double validation transactions where the user enters a second account number. E.g Mpesa Agent Number");

                entity.Property(e => e.TxResult).HasComment("Boolean for if the transaction succeeded 100% without encountering a critical terminating error");

                entity.Property(e => e.TxStartDate).HasComment("The date and time the transaction was recorded in the database. Can be different from core banking's transaction date");

                entity.Property(e => e.TxType).HasComment("The transaction type chosen by the user from TransactionTypeListItem");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CITId)
                    .HasConstraintName("FK_Transaction_CIT");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_DeviceList");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_DepositorSession");

                entity.HasOne(d => d.TxCurrencyNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TxCurrency)
                    .HasConstraintName("FK_Transaction_Currency_Transaction");

                entity.HasOne(d => d.TxTypeNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TxType)
                    .HasConstraintName("FK_Transaction_TransactionTypeListItem");
            });

            modelBuilder.Entity<TransactionException>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionExceptions)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionException_Transaction");
            });

            modelBuilder.Entity<TransactionException1>(entity =>
            {
                entity.HasComment("Exceptions encountered during execution");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<TransactionLimitList>(entity =>
            {
                entity.HasComment("Sets the transaction limit amounts for each currency");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<TransactionLimitListItem>(entity =>
            {
                entity.HasComment("Limit values for each currency");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyCode)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("ISO 4217 Three Character Currency Code");

                entity.Property(e => e.FundsSourceAmount).HasComment("The amount after which the Source of Funds screen will be shown");

                entity.Property(e => e.OverdepositAmount).HasComment("The amount after which the CDM will disable the counter");

                entity.Property(e => e.PreventOverdeposit).HasComment("CDM will not accept further deposits past the maximum");

                entity.Property(e => e.PreventUnderdeposit).HasDefaultValueSql("((1))");

                entity.Property(e => e.ShowFundsSource).HasComment("Whether to show the source of funds screen after deposit limit is reached or passed");

                entity.HasOne(d => d.CurrencyCodeNavigation)
                    .WithMany(p => p.TransactionLimitListItems)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionLimitListItem_Currency");

                entity.HasOne(d => d.Transactionitemlist)
                    .WithMany(p => p.TransactionLimitListItems)
                    .HasForeignKey(d => d.TransactionitemlistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionLimitListItem_TransactionLimitList");
            });

            modelBuilder.Entity<TransactionText>(entity =>
            {
                entity.HasComment("Stores the multi language texts for a tx");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.FullInstructionsNavigation)
                    .WithMany(p => p.TransactionTextFullInstructionsNavigations)
                    .HasForeignKey(d => d.FullInstructions)
                    .HasConstraintName("FK_TransactionText_full_instructions");

                entity.HasOne(d => d.TxItemNavigation)
                    .WithOne(p => p.TransactionText)
                    .HasForeignKey<TransactionText>(d => d.TxItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_TransactionTypeListItem");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.HasComment("");

                entity.Property(e => e.Code).HasComment("Vendor supplied ScreenType GUID");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("common description for the transaction type");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("common name for the transaction e.g. Mpesa Deposit");
            });

            modelBuilder.Entity<TransactionTypeListItem>(entity =>
            {
                entity.HasComment("Transactions that the system can perform e.g. regular deposit, Mpesa deposit, etc");

                entity.Property(e => e.CbTxType).HasComment("A string passed to core banking with transaction details so core banking can route the deposit to the correct handler");

                entity.Property(e => e.DefaultAccount).HasComment("the default account that pre-polulates the AccountNumber of a transaction");

                entity.Property(e => e.DefaultAccountCurrency)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('KES')")
                    .IsFixedLength();

                entity.Property(e => e.Description).HasComment("common description for the transaction type");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasComment("common name for the transaction e.g. Mpesa Deposit");

                entity.Property(e => e.TxTypeGuiscreenlist).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DefaultAccountCurrencyNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.DefaultAccountCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_Currency");

                entity.HasOne(d => d.TxLimitListNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxLimitList)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionLimitList");

                entity.HasOne(d => d.TxTextNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxText)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionText");

                entity.HasOne(d => d.TxTypeNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionListItem_TransactionType");

                entity.HasOne(d => d.TxTypeGuiscreenlistNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxTypeGuiscreenlist)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_GUIScreenList");
            });

            modelBuilder.Entity<TransactionTypeListTransactionTypeListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.TxtypeListNavigation)
                    .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.TxtypeList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList");

                entity.HasOne(d => d.TxtypeListItemNavigation)
                    .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.TxtypeListItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeListItem");
            });

            modelBuilder.Entity<TransactionView>(entity =>
            {
                entity.ToView("TransactionView");

                entity.Property(e => e.AccountName).IsUnicode(false);

                entity.Property(e => e.AccountNumber).IsUnicode(false);

                entity.Property(e => e.CbStatus).IsUnicode(false);

                entity.Property(e => e.CbTransactionNumber).IsUnicode(false);

                entity.Property(e => e.Currency)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DepositorName).IsUnicode(false);

                entity.Property(e => e.DeviceName).IsUnicode(false);

                entity.Property(e => e.ErrorMessage).IsUnicode(false);

                entity.Property(e => e.IdNumber).IsUnicode(false);

                entity.Property(e => e.Narration).IsUnicode(false);

                entity.Property(e => e.PhoneNumber).IsUnicode(false);

                entity.Property(e => e.ReferenceAccountName).IsUnicode(false);

                entity.Property(e => e.ReferenceAccountNumber).IsUnicode(false);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasComment("groups together users ho have privileges on the same machine");

                entity.HasOne(d => d.ParentGroupNavigation)
                    .WithMany(p => p.InverseParentGroupNavigation)
                    .HasForeignKey(d => d.ParentGroup)
                    .HasConstraintName("FK_UserGroup_UserGroup");
            });

            modelBuilder.Entity<UserLock>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ApplicationUserLoginDetailNavigation)
                    .WithMany(p => p.UserLocks)
                    .HasForeignKey(d => d.ApplicationUserLoginDetail)
                    .HasConstraintName("FK_UserLock_ApplicationUserLoginDetail");

                entity.HasOne(d => d.InitiatingUserNavigation)
                    .WithMany(p => p.UserLocks)
                    .HasForeignKey(d => d.InitiatingUser)
                    .HasConstraintName("FK_UserLock_InitiatingUser");
            });

            modelBuilder.Entity<ValidationItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("common description for the transaction type");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("common name for the transaction e.g. Mpesa Deposit");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.ValidationItems)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationItem_ValidationType");

                entity.HasOne(d => d.ValidationText)
                    .WithMany(p => p.ValidationItems)
                    .HasForeignKey(d => d.ValidationTextId)
                    .HasConstraintName("FK_ValidationItem_ValidationText");
            });

            modelBuilder.Entity<ValidationItemValue>(entity =>
            {
                entity.HasComment("Individual values for the validation");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ValidationItem)
                    .WithMany(p => p.ValidationItemValues)
                    .HasForeignKey(d => d.ValidationItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationItemValue_ValidationItem");
            });

            modelBuilder.Entity<ValidationList>(entity =>
            {
                entity.HasComment("List of validations to be performed on a field");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("common description for the transaction type");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("common name for the transaction e.g. Mpesa Deposit");
            });

            modelBuilder.Entity<ValidationListValidationItem>(entity =>
            {
                entity.HasComment("Link a ValidationItem to a ValidationList");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ValidationItem)
                    .WithMany(p => p.ValidationListValidationItems)
                    .HasForeignKey(d => d.ValidationItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationList_ValidationItem_ValidationItem");

                entity.HasOne(d => d.ValidationList)
                    .WithMany(p => p.ValidationListValidationItems)
                    .HasForeignKey(d => d.ValidationListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationList_ValidationItem_ValidationList");
            });

            modelBuilder.Entity<ValidationText>(entity =>
            {
                entity.HasComment("Multilanguage validation result text");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ValidationItem)
                    .WithMany(p => p.ValidationTexts)
                    .HasForeignKey(d => d.ValidationItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationText_ValidationItem");
            });

            modelBuilder.Entity<ValidationType>(entity =>
            {
                entity.HasComment("The type of validation e.g. regex, etc");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasComment("common description for the transaction type");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasComment("common name for the transaction e.g. Mpesa Deposit");
            });

            modelBuilder.Entity<ViewConfig>(entity =>
            {
                entity.ToView("viewConfig");
            });

            modelBuilder.Entity<ViewPermission>(entity =>
            {
                entity.ToView("viewPermissions");
            });

            modelBuilder.Entity<WebPortalLogin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.ApplicationUserLoginDetailNavigation)
                    .WithMany(p => p.WebPortalLogins)
                    .HasForeignKey(d => d.ApplicationUserLoginDetail)
                    .HasConstraintName("FK_WebPortalLogin_ApplicationUserLoginDetail");
            });

            modelBuilder.Entity<WebPortalRole>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.OidNavigation)
                    .WithOne(p => p.WebPortalRole)
                    .HasForeignKey<WebPortalRole>(d => d.Oid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebPortalRole_Oid");
            });

            modelBuilder.Entity<WebPortalRoleRolesApplicationUserApplicationUser>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.ApplicationUsersNavigation)
                    .WithMany(p => p.WebPortalRoleRolesApplicationUserApplicationUsers)
                    .HasForeignKey(d => d.ApplicationUsers)
                    .HasConstraintName("FK_WebPortalRoleRoles_ApplicationUserApplicationUsers_ApplicationUsers");

                entity.HasOne(d => d.RolesNavigation)
                    .WithMany(p => p.WebPortalRoleRolesApplicationUserApplicationUsers)
                    .HasForeignKey(d => d.Roles)
                    .HasConstraintName("FK_WebPortalRoleRoles_ApplicationUserApplicationUsers_Roles");
            });

            modelBuilder.Entity<WebUserLoginCount>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();
            });

            modelBuilder.Entity<WebUserPasswordHistory>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();
            });

            OnModelCreatingGeneratedProcedures(modelBuilder);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
