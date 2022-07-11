using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.Extensions;
using Cashmere.Library.CashmereDataAccess.Views;
using Microsoft.EntityFrameworkCore;
using ApplicationException = Cashmere.Library.CashmereDataAccess.Entities.ApplicationException;

namespace Cashmere.Library.CashmereDataAccess.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.BaseType == null)
                {
                    entity.SetTableName(entity.DisplayName());
                }
            }

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

            modelBuilder.Entity<AlertEmail>(entity =>
            {
                entity.HasComment("Stores emails sent by the system");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AlertEventId).HasComment("Corresponding Alert that is tied to this email message");

                entity.Property(e => e.Attachments).HasComment("Pipe delimited List of filenames for files to attach when sending. Files must be accessible from the server");

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
                    .WithMany(p => p.AlertEmails)
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

                entity.HasOne(d => d.AlertMessageType)
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

            modelBuilder.Entity<AlertSMS>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AlertSMS>(entity =>
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

                entity.Property(e => e.To).HasComment("Pipe delimited List of phone numbers to receive SMSes");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.AlertSMSes)
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

            modelBuilder.Entity<ApplicationException>(entity =>
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

            modelBuilder.Entity<ApplicationLog>(entity =>
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

                entity.HasOne(d => d.ApplicationUserLoginDetail)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.ApplicationUserLoginDetailId)
                    .HasConstraintName("FK_ApplicationUser_ApplicationUserLoginDetail");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_Role");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.ApplicationUsers)
                    .HasForeignKey(d => d.UserGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApplicationUser_UserGroup");
            });

            modelBuilder.Entity<ApplicationUserChangePassword>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.PasswordPolicy)
                    .WithMany(p => p.ApplicationUserChangePasswords)
                    .HasForeignKey(d => d.PasswordPolicyId)
                    .HasConstraintName("FK_ApplicationUserChangePassword_PasswordPolicy");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApplicationUserChangePasswords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ApplicationUserChangePassword_User");
            });

            modelBuilder.Entity<ApplicationUserLoginDetail>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.LastLoginLogEntry)
                    .WithMany(p => p.ApplicationUserLoginDetails)
                    .HasForeignKey(d => d.LastLoginLogEntryId)
                    .HasConstraintName("FK_ApplicationUserLoginDetail_LastLoginLogEntry");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ApplicationUserLoginDetails)
                    .HasForeignKey(d => d.UserId)
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

                entity.HasOne(d => d.Country)
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

                entity.Property(e => e.AuthUserId).HasComment("Application User who authorised the CIT event");

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

                entity.Property(e => e.StartUserId).HasComment("ApplicationUser who initiated the CIT");

                entity.Property(e => e.ToDate).HasComment("The datetime until which the CIT calculations will be carrid out");

                entity.HasOne(d => d.AuthorisingUser)
                    .WithOne()
                    .HasForeignKey<ApplicationUser>(nameof(CIT.AuthUserId))
                    .IsRequired(false)
                    .HasConstraintName("FK_CIT_ApplicationUser_AuthUser");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CITs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_DeviceList");

                entity.HasOne(d => d.StartUser)
                    .WithMany(p => p.CITStartUsers)
                    .HasForeignKey(d => d.StartUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_ApplicationUser_StartUser");
            });

            modelBuilder.Entity<CITDenomination>(entity =>
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
                    .WithMany(p => p.CITDenominations)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITDenominations_CIT");

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.CITDenominations)
                    .HasForeignKey(d => d.CurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITDenominations_Currency");
            });

            modelBuilder.Entity<CITPrintout>(entity =>
            {
                entity.HasComment("Stores CIT receipts");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CITId).HasComment("The CIT this rceipt belongs to");

                entity.Property(e => e.Datetime).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsCopy).HasComment("Is this CIT Receipt a copy, used for marking duplicate receipts");

                entity.Property(e => e.PrintContent).HasComment("Text of the receipt");

                entity.Property(e => e.PrintGuid).HasComment("Receipt SHA512 hash");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.CITPrintouts)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITPrintout_CIT");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasComment("Configuration List");

                entity.HasOne(d => d.ConfigCategory)
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

                entity.HasOne(d => d.ParentGroup)
                    .WithMany(p => p.ParentGroups)
                    .HasForeignKey(d => d.ParentGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<CrashEvent>(entity =>
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
                entity.HasComment("Enumeration of allowed Currencies. A device can then associate with a currency List");

                entity.Property(e => e.DefaultCurrencyId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DefaultCurrency)
                    .WithMany(p => p.CurrencyLists)
                    .HasForeignKey(d => d.DefaultCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_Currency");
            });

            modelBuilder.Entity<CurrencyListCurrency>(entity =>
            {
                entity.HasComment("[m2m] Currency and CurrencyList");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyItemId)
                    .IsUnicode(false)
                    .IsFixedLength()
                    .HasComment("The currency in the List");

                entity.Property(e => e.CurrencyListId).HasComment("The Currency List to which the currency is associated");

                entity.Property(e => e.CurrencyOrder).HasComment("ASC Order of sorting for currencies in List.");

                entity.HasOne(d => d.CurrencyItem)
                    .WithMany(p => p.CurrencyListCurrencies)
                    .HasForeignKey(d => d.CurrencyItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Currency_CurrencyList_Currency");

                entity.HasOne(d => d.CurrencyList)
                    .WithMany(p => p.CurrencyListCurrencies)
                    .HasForeignKey(d => d.CurrencyListId)
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

                entity.HasOne(d => d.Transaction)
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

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.DepositorSessions)
                    .HasForeignKey(d => d.LanguageCode)
                    .HasConstraintName("FK_DepositorSession_Language");
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.GuiScreenListId).HasDefaultValueSql("((1))");

                entity.Property(e => e.LoginAttempts).HasComment("how many times in a row a login attempt has failed");

                entity.Property(e => e.LoginCycles).HasComment("how many cycles of failed logins have been detected. used to lock the machine in case of password guessing");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Secret)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TransactionTypeListId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_Branch");

                entity.HasOne(d => d.ConfigGroup)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.ConfigGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_ConfigGroup");

                entity.HasOne(d => d.CurrencyList)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.CurrencyListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_CurrencyList");

                entity.HasOne(d => d.GuiScreenList)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.GuiScreenListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_GUIScreenList");

                entity.HasOne(d => d.LanguageList)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.LanguageListId)
                    .HasConstraintName("FK_Device_LanguageList");

                entity.HasOne(d => d.TransactionTypeList)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.TransactionTypeListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_TransactionTypeList");

                entity.HasOne(d => d.DeviceType)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceList_DeviceType");

                entity.HasOne(d => d.UserGroup)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.UserGroupId)
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

                entity.HasOne(d => d.ConfigGroup)
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

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.DeviceLogins)
                    .HasForeignKey(d => d.UserId)
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

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.DeviceSuspenseAccounts)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceSuspenseAccount_Currency");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceCITSuspenseAccounts)
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

                entity.HasOne(d => d.AuthorisingUser)
                    .WithOne()
                    .HasForeignKey<ApplicationUser>(nameof(EscrowJam.AuthorisinguserId))
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EscrowJam_AppUser_Approver");

                entity.HasOne(d => d.InitialisingUser)
                    .WithOne()
                    .HasForeignKey<ApplicationUser>(nameof(EscrowJam.InitialisinguserId))
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict)
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

                entity.HasOne(d => d.GuiScreenList)
                    .WithOne()
                    .HasForeignKey<GuiScreenList>(nameof(GuiScreenListScreen.GuiScreenListId))
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict)
                    //.HasForeignKey(d => d.GuiScreenList)
                    //.OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIScreenList");

                entity.HasOne(d => d.GuiPrepopList)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.GuiPrepopListId)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIPrepopList");

                entity.HasOne(d => d.GuiScreen)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.GuiScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIScreen");

                entity.HasOne(d => d.ValidationList)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.ValidationListId)
                    .HasConstraintName("FK_GuiScreenList_Screen_ValidationList");
            });

            modelBuilder.Entity<GuiPrepopItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Value)
                    .WithMany(p => p.GuiPrepopItems)
                    .HasForeignKey(d => d.ValueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopItem_TextItem");
            });

            modelBuilder.Entity<GuiPrepopList>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.UseDefault).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GuiPrepopListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.GuiPrepopItem)
                    .WithMany(p => p.GuiPrepopListItems)
                    .HasForeignKey(d => d.GuiPrepopItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopItem");

                entity.HasOne(d => d.GuiPrepopList)
                    .WithMany(p => p.GuiPrepopListItems)
                    .HasForeignKey(d => d.GuiPrepopListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopList");
            });

            modelBuilder.Entity<GuiScreen>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.InputMask).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.PrefillText).HasComment("Text to prefil in the textbox");

                entity.HasOne(d => d.GuiScreenText)
                    .WithMany(p => p.GuiScreens)
                    .HasForeignKey(d => d.GuiScreenTextId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreen_GUIScreenText");

                entity.HasOne(d => d.GuiScreenType)
                    .WithMany(p => p.GuiScreens)
                    .HasForeignKey(d => d.GuiScreenTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreen_GUIScreenType");
            });

            modelBuilder.Entity<GuiScreenList>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<GuiScreenText>(entity =>
            {
                entity.HasComment("Stores the text for a screen for a language");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.GuiScreenId).HasComment("The GUIScreen this entry corresponds to");

                entity.HasOne(d => d.BtnAcceptCaption)
                    .WithMany(p => p.GuiScreenTextBtnAcceptCaptions)
                    .HasForeignKey(d => d.BtnAcceptCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_BtnAcceptCaption");

                entity.HasOne(d => d.BtnBackCaption)
                    .WithMany(p => p.GuiScreenTextBtnBackCaptions)
                    .HasForeignKey(d => d.BtnBackCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_BtnBackCaption");

                entity.HasOne(d => d.BtnCancelCaption)
                    .WithMany(p => p.GuiScreenTextBtnCancelCaptions)
                    .HasForeignKey(d => d.BtnCancelCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_BtnCancelCaption");

                entity.HasOne(d => d.FullInstructions)
                    .WithMany(p => p.GuiScreenTextFullInstructions)
                    .HasForeignKey(d => d.FullInstructionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_FullInstructions");

                entity.HasOne(d => d.GuiScreen)
                    .WithMany(p => p.GuiScreenTexts)
                    .HasForeignKey(d => d.GuiScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_GUIScreen");

                entity.HasOne(d => d.ScreenTitle)
                    .WithMany(p => p.GuiScreenTextScreenTitles)
                    .HasForeignKey(d => d.ScreenTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_ScreenTitle");

                entity.HasOne(d => d.ScreenTitleInstruction)
                    .WithMany(p => p.GuiScreenTextScreenTitleInstructions)
                    .HasForeignKey(d => d.ScreenTitleInstructionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_ScreenTitleInstruction");
            });

            modelBuilder.Entity<GuiScreenType>(entity =>
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
                entity.HasComment("A List of languages a device supports");

                entity.Property(e => e.DefaultLanguageId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).IsUnicode(false);

                entity.HasOne(d => d.DefaultLanguage)
                    .WithMany(p => p.LanguageLists)
                    .HasForeignKey(d => d.DefaultLanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language");
            });

            modelBuilder.Entity<LanguageListLanguage>(entity =>
            {
                entity.HasComment("[m2m] LanguageList and Language");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageItemId)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.LanguageItem)
                    .WithMany(p => p.LanguageListLanguages)
                    .HasForeignKey(d => d.LanguageItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language_Language");

                entity.HasOne(d => d.LanguageList)
                    .WithMany(p => p.LanguageListLanguages)
                    .HasForeignKey(d => d.LanguageListId)
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

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.ModelDifferenceAspects)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ModelDifferenceAspect_Owner");
            });

            modelBuilder.Entity<PasswordHistory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.PasswordHistories)
                    .HasForeignKey(d => d.ApplicationUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.TypePermissionObject)
                    .WithMany(p => p.PermissionPolicyMemberPermissionsObjects)
                    .HasForeignKey(d => d.TypePermissionObjectId)
                    .HasConstraintName("FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyNavigationPermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionPolicyNavigationPermissionsObjects)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_PermissionPolicyPermissionsObject_Role");
            });

            modelBuilder.Entity<PermissionPolicyObjectPermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.TypePermissionObject)
                    .WithMany(p => p.PermissionPolicyObjectPermissionsObjects)
                    .HasForeignKey(d => d.TypePermissionObjectId)
                    .HasConstraintName("FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyRole>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.ObjectType)
                    .WithMany(p => p.PermissionPolicyRoles)
                    .HasForeignKey(d => d.ObjectTypeId)
                    .HasConstraintName("FK_PermissionPolicyRole_ObjectType");
            });

            modelBuilder.Entity<PermissionPolicyTypePermissionsObject>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionPolicyTypePermissionsObjects)
                    .HasForeignKey(d => d.RoleId)
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

                entity.HasOne(d => d.Transaction)
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

            modelBuilder.Entity<SessionException>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<SysTextItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SysTextItems)
                    .HasForeignKey(d => d.CategoryId)
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

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Parents)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.Language)
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

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.TextItems)
                    .HasForeignKey(d => d.CategoryId)
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

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Parents)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.Language)
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

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.TxCurrency)
                    .HasConstraintName("FK_Transaction_Currency_Transaction");

                entity.HasOne(d => d.TransactionTypeListItem)
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

            modelBuilder.Entity<TransactionException>(entity =>
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

                entity.HasOne(d => d.Currency)
                    .WithMany(p => p.TransactionLimitListItems)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionLimitListItem_Currency");

                entity.HasOne(d => d.TransactionLimitList)
                    .WithMany(p => p.TransactionLimitListItems)
                    .HasForeignKey(d => d.TransactionLimitListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionLimitListItem_TransactionLimitList");
            });
            modelBuilder.Entity<TransactionPosting>(entity =>
            {

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.AuthorisingUser)
                    .WithMany(p => p.TransactionPostingAuthUsers)
                    .HasForeignKey(d => d.AuthorisingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionPosting_AuthorisingUser");

                entity.HasOne(d => d.InitialisingUser)
                    .WithMany(p => p.TransactionPostingInitUsers)
                    .HasForeignKey(d => d.InitialisingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionPosting_InitialisingUser");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.TransactionPostings)
                    .HasForeignKey(d => d.TxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionPosting_Transaction");
            });

            modelBuilder.Entity<TransactionText>(entity =>
            {
                entity.HasComment("Stores the multi language texts for a tx");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.FullInstructions)
                    .WithMany(p => p.TransactionTextFullInstructions)
                    .HasForeignKey(d => d.FullInstructionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_full_instructions");

                entity.HasOne(d => d.AliasAccountNumberCaption)
                    .WithMany(p => p.TransactionTextAliasAccountNumberCaptions)
                    .HasForeignKey(d => d.AliasAccountNumberCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Alias_Account_Number_Caption");

                entity.HasOne(d => d.AliasAccountNameCaption)
                    .WithMany(p => p.TransactionTextAliasAccountNameCaptions)
                    .HasForeignKey(d => d.AliasAccountNameCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Alias_Account_Name_Caption");

                entity.HasOne(d => d.AccountNumberCaption)
                    .WithMany(p => p.TransactionTextAccountNumberCaptions)
                    .HasForeignKey(d => d.AccountNumberCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Account_Number_Caption");

                entity.HasOne(d => d.AccountNameCaption)
                    .WithMany(p => p.TransactionTextAccountNameCaptions)
                    .HasForeignKey(d => d.AccountNameCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Account_Name_Caption");

                entity.HasOne(d => d.TxItem)
                    .WithOne(p => p.TransactionText)
                    .HasForeignKey<TransactionText>(d => d.TxItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_TransactionTypeListItem");

                entity.HasOne(d => d.DepositorNameCaption)
                    .WithMany(p => p.TransactionTextDepositorNameCaptions)
                    .HasForeignKey(d => d.DepositorNameCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Depositor_Name_Caption");

                entity.HasOne(d => d.Disclaimer)
                    .WithMany(p => p.TransactionTextDisclaimers)
                    .HasForeignKey(d => d.DisclaimerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Disclaimers");

                entity.HasOne(d => d.FundsSourceCaption)
                    .WithMany(p => p.TransactionTextFundsSourceCaptions)
                    .HasForeignKey(d => d.FundsSourceCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Funds_Source_Caption");

                entity.HasOne(d => d.IdNumberCaption)
                    .WithMany(p => p.TransactionTextIdNumberCaptions)
                    .HasForeignKey(d => d.IdNumberCaptionId)
                    .HasConstraintName("FK_TransactionText_IdNumberCaption");

                entity.HasOne(d => d.ListItemCaption)
                    .WithMany(p => p.TransactionTextListItemCaptions)
                    .HasForeignKey(d => d.ListItemCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ListItemCaption");

                entity.HasOne(d => d.NarrationCaption)
                    .WithMany(p => p.TransactionTextNarrationCaptions)
                    .HasForeignKey(d => d.NarrationCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_NarrationCaption");

                entity.HasOne(d => d.ReceiptTemplate)
                    .WithMany(p => p.TransactionTextReceiptTemplates)
                    .HasForeignKey(d => d.ReceiptTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReceiptTemplate");

                entity.HasOne(d => d.PhoneNumberCaption)
                    .WithMany(p => p.TransactionTextPhoneNumberCaptions)
                    .HasForeignKey(d => d.PhoneNumberCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_PhoneNumberCaption");

                entity.HasOne(d => d.ReferenceAccountNameCaption)
                    .WithMany(p => p.TransactionTextReferenceAccountNameCaptions)
                    .HasForeignKey(d => d.ReferenceAccountNameCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReferenceAccountNameCaption");

                entity.HasOne(d => d.ReferenceAccountNumberCaption)
                    .WithMany(p => p.TransactionTextReferenceAccountNumberCaptions)
                    .HasForeignKey(d => d.ReferenceAccountNumberCaptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReferenceAccountNumberCaption");

                entity.HasOne(d => d.Terms)
                    .WithMany(p => p.TransactionTextTerms)
                    .HasForeignKey(d => d.TermsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Terms");

                //entity.HasOne(d => d.TxItem)
                //    .WithMany(p => p.tx)
                //    .HasForeignKey(d => d.TxItemId)
                //    .HasConstraintName("FK_TransactionText_Terms");
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

                entity.Property(e => e.DefaultAccountCurrencyId)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('KES')")
                    .IsFixedLength();

                entity.Property(e => e.Description).HasComment("common description for the transaction type");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasComment("common name for the transaction e.g. Mpesa Deposit");

                entity.Property(e => e.TxTypeGuiScreenListId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DefaultAccountCurrency)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.DefaultAccountCurrencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_Currency");

                entity.HasOne(d => d.TxLimitList)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxLimitListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionLimitList");

                entity.HasOne(d => d.TxText)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxTextId)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionText");

                entity.HasOne(d => d.TxType)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxTypeId)
                    .HasConstraintName("FK_TransactionListItem_TransactionType");

                entity.HasOne(d => d.TxTypeGuiScreenList)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxTypeGuiScreenListId)
                    .HasConstraintName("FK_TransactionTypeListItem_GUIScreenList");
            });

            modelBuilder.Entity<TransactionTypeListTransactionTypeListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.TxTypeList)
                    .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.TxtypeListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList");

                entity.HasOne(d => d.TxTypeListItem)
                    .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.TxtypeListItemId)
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

                entity.HasOne(d => d.ParentGroup)
                    .WithMany(p => p.ParentGroups)
                    .HasForeignKey(d => d.ParentGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroup_UserGroup");
            });

            modelBuilder.Entity<UserLock>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ApplicationUserLoginDetail)
                    .WithMany(p => p.UserLocks)
                    .HasForeignKey(d => d.ApplicationUserLoginDetailId)
                    .HasConstraintName("FK_UserLock_ApplicationUserLoginDetail");

                entity.HasOne(d => d.InitiatingUser)
                    .WithMany(p => p.UserLocks)
                    .HasForeignKey(d => d.InitiatingUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.ValidationType)
                    .WithMany(p => p.ValidationItems)
                    .HasForeignKey(d => d.ValidationTypeId)
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

                entity.HasOne(d => d.ErrorMessage)
                    .WithMany(p => p.ValidationTextErrorMessages)
                    .HasForeignKey(d => d.ErrorMessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationText_ErrorMessage");

                entity.HasOne(d => d.SuccessMessage)
                    .WithMany(p => p.ValidationTextSuccessMessages)
                    .HasForeignKey(d => d.SuccessMessageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationText_SuccessMessage");
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

                entity.HasOne(d => d.ApplicationUserLoginDetail)
                    .WithMany(p => p.WebPortalLogins)
                    .HasForeignKey(d => d.ApplicationUserLoginDetailId)
                    .HasConstraintName("FK_WebPortalLogin_ApplicationUserLoginDetail");
            });

            modelBuilder.Entity<WebPortalRole>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.PermissionPolicyRole)
                    .WithOne(p => p.WebPortalRole)
                    .HasForeignKey<WebPortalRole>(d => d.Oid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebPortalRole_Oid");
            });

            modelBuilder.Entity<WebPortalRoleRolesApplicationUserApplicationUser>(entity =>
            {
                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.HasOne(d => d.ApplicationUsers)
                    .WithMany(p => p.WebPortalRoleRolesApplicationUserApplicationUsers)
                    .HasForeignKey(d => d.ApplicationUsersId)
                    .HasConstraintName("FK_WebPortalRoleRoles_ApplicationUserApplicationUsers_ApplicationUsers");

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.WebPortalRoleRolesApplicationUserApplicationUsers)
                    .HasForeignKey(d => d.RolesId)
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

        }
    }
}