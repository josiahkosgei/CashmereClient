using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Cashmere.Library.CashmereDataAccess.Views;
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
            
            modelBuilder.UseCollation("Latin1_General_CI_AS");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<AlertEmail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.AlertEmails)
                    .HasForeignKey(d => d.AlertEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertEmail_AlertEmailEvent");
            });

            modelBuilder.Entity<AlertEmailAttachment>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Hash).IsFixedLength();

                entity.Property(e => e.Type).IsFixedLength();
            });

            modelBuilder.Entity<AlertEvent>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AlertEventNavigation)
                    .WithMany(p => p.InverseAlertEventNavigation)
                    .HasForeignKey(d => d.AlertEventId)
                    .HasConstraintName("FK_AlertEmailEvent_AlertEmailEvent");
            });

            modelBuilder.Entity<AlertMessageRegistry>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.EmailEnabled).HasDefaultValueSql("((1))");

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
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<AlertSMS>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AlertEvent)
                    .WithMany(p => p.AlertSMS)
                    .HasForeignKey(d => d.AlertEventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AlertSMS_AlertEvent");
            });

            modelBuilder.Entity<ApplicationException>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
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

                entity.Property(e => e.EmailEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.Password).IsFixedLength();

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

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CountryCode).IsFixedLength();

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

            modelBuilder.Entity<CIT>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CITDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.AuthUserNavigation)
                    .WithMany(p => p.CITAuthUsers)
                    .HasForeignKey(d => d.AuthUser)
                    .HasConstraintName("FK_CIT_ApplicationUser_AuthUser");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.CITs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_DeviceList");

                entity.HasOne(d => d.StartUserNavigation)
                    .WithMany(p => p.CITStartUsers)
                    .HasForeignKey(d => d.StartUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIT_ApplicationUser_StartUser");
            });

            modelBuilder.Entity<CITDenomination>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyId).IsFixedLength();

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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Datetime).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.CITPrintouts)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITPrintout_CIT");
            });

            modelBuilder.Entity<CITTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Currency).IsFixedLength();

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.CITTransactions)
                    .HasForeignKey(d => d.CITId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CITTransaction_CIT");
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Configs)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Config_ConfigCategory");
            });

            modelBuilder.Entity<ConfigCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<ConfigGroup>(entity =>
            {
                entity.HasOne(d => d.ParentGroupNavigation)
                    .WithMany(p => p.InverseParentGroupNavigation)
                    .HasForeignKey(d => d.ParentGroup)
                    .HasConstraintName("FK_ConfigGroup_ConfigGroup");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryCode)
                    .HasDefaultValueSql("('')")
                    .IsFixedLength();

                entity.Property(e => e.CountryName).HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<CrashEvent>(entity =>
            {
                entity.HasComment("contains a crash report");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Code).IsFixedLength();

                entity.Property(e => e.Flag).IsFixedLength();

                entity.Property(e => e.Iso3NumericCode).IsFixedLength();
            });

            modelBuilder.Entity<CurrencyList>(entity =>
            {
                entity.Property(e => e.DefaultCurrency).IsFixedLength();

                entity.HasOne(d => d.DefaultCurrencyNavigation)
                    .WithMany(p => p.CurrencyLists)
                    .HasForeignKey(d => d.DefaultCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CurrencyList_Currency");
            });

            modelBuilder.Entity<CurrencyListCurrency>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyItem).IsFixedLength();

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

            modelBuilder.Entity<DenominationDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.Tx)
                    .WithMany(p => p.DenominationDetails)
                    .HasForeignKey(d => d.TxId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DenominationDetail_Transaction");
            });

            modelBuilder.Entity<DepositorSession>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageCode).IsFixedLength();

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

                entity.Property(e => e.AppKey).IsFixedLength();

                entity.Property(e => e.GUIScreenList).HasDefaultValueSql("((1))");

                entity.Property(e => e.MacAddress).IsFixedLength();

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

                entity.HasOne(d => d.GUIScreenListNavigation)
                    .WithMany(p => p.Devices)
                    .HasForeignKey(d => d.GUIScreenList)
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

            modelBuilder.Entity<DeviceCITSuspenseAccount>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyCode).IsFixedLength();

                entity.HasOne(d => d.CurrencyCodeNavigation)
                    .WithMany(p => p.DeviceCITSuspenseAccounts)
                    .HasForeignKey(d => d.CurrencyCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceCITSuspenseAccount_Currency");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceCITSuspenseAccounts)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceCITSuspenseAccount_Device");
            });

            modelBuilder.Entity<DeviceConfig>(entity =>
            {
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

                entity.Property(e => e.IsInfront).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DevicePrinters)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DevicePrinter_DeviceList");
            });

            modelBuilder.Entity<DeviceStatus>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BaCurrency).IsFixedLength();

                entity.Property(e => e.BagNoteCapacity).IsFixedLength();
            });

            modelBuilder.Entity<DeviceSuspenseAccount>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyCode).IsFixedLength();

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

            modelBuilder.Entity<EscrowJam>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.AuthorisingUserNavigation)
                    .WithMany(p => p.EscrowJamAuthorisingUsers)
                    .HasForeignKey(d => d.AuthorisingUser)
                    .HasConstraintName("FK_EscrowJam_AppUser_Approver");

                entity.HasOne(d => d.InitialisingUserNavigation)
                    .WithMany(p => p.EscrowJamInitialisingUsers)
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

                entity.HasOne(d => d.GUIPrepopList)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.GUIPrepoplistId)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIPrepopList");

                entity.HasOne(d => d.GUIScreenNavigation)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.Screen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GuiScreenList_Screen_GUIScreen");

                entity.HasOne(d => d.ValidationList)
                    .WithMany(p => p.GuiScreenListScreens)
                    .HasForeignKey(d => d.ValidationListId)
                    .HasConstraintName("FK_GuiScreenList_Screen_ValidationList");
            });

            modelBuilder.Entity<GUIPrepopItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.ValueNavigation)
                    .WithMany(p => p.GUIPrepopItems)
                    .HasForeignKey(d => d.Value)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopItem_TextItem");
            });

            modelBuilder.Entity<GUIPrepopList>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.UseDefault).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GUIPrepopListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.GUIPrepopItem)
                    .WithMany(p => p.GUIPrepopListItems)
                    .HasForeignKey(d => d.Item)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopItem");

                entity.HasOne(d => d.ListNavigation)
                    .WithMany(p => p.GUIPrepopListItems)
                    .HasForeignKey(d => d.List)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIPrepopList_Item_GUIPrepopList");
            });

            modelBuilder.Entity<GUIScreen>(entity =>
            {
                entity.HasOne(d => d.GuiTextNavigation)
                    .WithMany(p => p.GUIScreens)
                    .HasForeignKey(d => d.GuiText)
                    .HasConstraintName("FK_GUIScreen_GUIScreenText");

                entity.HasOne(d => d.GUIScreenType)
                    .WithMany(p => p.GUIScreens)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreen_GUIScreenType");
            });

            modelBuilder.Entity<GUIScreenText>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.BtnAcceptCaptionNavigation)
                    .WithMany(p => p.GUIScreenTextBtnAcceptCaptions)
                    .HasForeignKey(d => d.BtnAcceptCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_accept_caption");

                entity.HasOne(d => d.BtnBackCaptionNavigation)
                    .WithMany(p => p.GUIScreenTextBtnBackCaptions)
                    .HasForeignKey(d => d.BtnBackCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_back_caption");

                entity.HasOne(d => d.BtnCancelCaptionNavigation)
                    .WithMany(p => p.GUIScreenTextBtnCancelCaptions)
                    .HasForeignKey(d => d.BtnCancelCaption)
                    .HasConstraintName("FK_GUIScreenText_btn_cancel_caption");

                entity.HasOne(d => d.FullInstructionsNavigation)
                    .WithMany(p => p.GUIScreenTextFullInstructionss)
                    .HasForeignKey(d => d.FullInstructions)
                    .HasConstraintName("FK_GUIScreenText_full_instructions");

                entity.HasOne(d => d.GUIScreen)
                    .WithOne(p => p.GUIScreenText)
                    .HasForeignKey<GUIScreenText>(d => d.GUIScreenId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_GUIScreen");

                entity.HasOne(d => d.ScreenTitleNavigation)
                    .WithMany(p => p.GUIScreenTextScreenTitles)
                    .HasForeignKey(d => d.ScreenTitle)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GUIScreenText_screen_title");

                entity.HasOne(d => d.ScreenTitleInstructionNavigation)
                    .WithMany(p => p.GUIScreenTextScreenTitleInstructions)
                    .HasForeignKey(d => d.ScreenTitleInstruction)
                    .HasConstraintName("FK_GUIScreenText_screen_title_instruction");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Languages");

                entity.Property(e => e.Code).IsFixedLength();
            });

            modelBuilder.Entity<LanguageList>(entity =>
            {
                entity.Property(e => e.DefaultLanguage).IsFixedLength();

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DefaultLanguageNavigation)
                    .WithMany(p => p.LanguageLists)
                    .HasForeignKey(d => d.DefaultLanguage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LanguageList_Language");
            });

            modelBuilder.Entity<LanguageListLanguage>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.LanguageItem).IsFixedLength();

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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.UseHistory).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
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

            modelBuilder.Entity<PrinterStatus>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Modified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Printout>(entity =>
            {
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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<SessionException>(entity =>
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

                entity.Property(e => e.LanguageCode).IsFixedLength();

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

                entity.Property(e => e.LanguageCode).IsFixedLength();

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
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.TxCurrency).IsFixedLength();

                entity.HasOne(d => d.AuthUserNavigation)
                    .WithMany(p => p.TransactionAuthUsers)
                    .HasForeignKey(d => d.AuthUser)
                    .HasConstraintName("FK_Transaction_auth_user");

                entity.HasOne(d => d.CIT)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CITId)
                    .HasConstraintName("FK_Transaction_CIT");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_DeviceList");

                entity.HasOne(d => d.InitUserNavigation)
                    .WithMany(p => p.TransactionInitUsers)
                    .HasForeignKey(d => d.InitUser)
                    .HasConstraintName("FK_Transaction_init_user");

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
                entity.HasComment("Exceptions encountered during execution");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<TransactionLimitList>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<TransactionLimitListItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CurrencyCode).IsFixedLength();

                entity.Property(e => e.PreventUnderdeposit).HasDefaultValueSql("((1))");

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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.FullInstructionsNavigation)
                    .WithMany(p => p.TransactionTextFullInstructionss)
                    .HasForeignKey(d => d.FullInstructions)
                    .HasConstraintName("FK_TransactionText_full_instructions");

                entity.HasOne(d => d.TxItemNavigation)
                    .WithOne(p => p.TransactionTextNav)
                    .HasForeignKey<TransactionText>(d => d.TxItem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_TransactionTypeListItem");

                entity.HasOne(d => d.AliasAccountNumberCaptionNavigation)
                    .WithMany(p => p.TransactionTextAliasAccountNumberCaptions)
                    .HasForeignKey(d => d.AliasAccountNumberCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Alias_Account_Number_Caption");

                entity.HasOne(d => d.AliasAccountNameCaptionNavigation)
                    .WithMany(p => p.TransactionTextAliasAccountNameCaptions)
                    .HasForeignKey(d => d.AliasAccountNameCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Alias_Account_Name_Caption");

                entity.HasOne(d => d.AccountNumberCaptionNavigation)
                    .WithMany(p => p.TransactionTextAccountNumberCaptions)
                    .HasForeignKey(d => d.AccountNumberCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Account_Number_Caption");

                entity.HasOne(d => d.AccountNameCaptionNavigation)
                    .WithMany(p => p.TransactionTextAccountNameCaptions)
                    .HasForeignKey(d => d.AccountNameCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Account_Name_Caption");

                entity.HasOne(d => d.DepositorNameCaptionNavigation)
                    .WithMany(p => p.TransactionTextDepositorNameCaptions)
                    .HasForeignKey(d => d.DepositorNameCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Depositor_Name_Caption");

                entity.HasOne(d => d.DisclaimerNavigation)
                    .WithMany(p => p.TransactionTextDisclaimers)
                    .HasForeignKey(d => d.Disclaimer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Disclaimers");

                entity.HasOne(d => d.FundsSourceCaptionNavigation)
                    .WithMany(p => p.TransactionTextFundsSourceCaptions)
                    .HasForeignKey(d => d.FundsSourceCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Funds_Source_Caption");

                entity.HasOne(d => d.IdNumberCaptionNavigation)
                    .WithMany(p => p.TransactionTextIdNumberCaptions)
                    .HasForeignKey(d => d.IdNumberCaption)
                    .HasConstraintName("FK_TransactionText_IdNumberCaption");

                entity.HasOne(d => d.ListItemCaptionNavigation)
                    .WithMany(p => p.TransactionTextListItemCaptions)
                    .HasForeignKey(d => d.ListItemCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ListItemCaption");

                entity.HasOne(d => d.NarrationCaptionNavigation)
                    .WithMany(p => p.TransactionTextNarrationCaptions)
                    .HasForeignKey(d => d.NarrationCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_NarrationCaption");

                entity.HasOne(d => d.ReceiptTemplateNavigation)
                    .WithMany(p => p.TransactionTextReceiptTemplates)
                    .HasForeignKey(d => d.ReceiptTemplate)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReceiptTemplate");

                entity.HasOne(d => d.PhoneNumberCaptionNavigation)
                    .WithMany(p => p.TransactionTextPhoneNumberCaptions)
                    .HasForeignKey(d => d.PhoneNumberCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_PhoneNumberCaption");

                entity.HasOne(d => d.ReferenceAccountNameCaptionNavigation)
                    .WithMany(p => p.TransactionTextReferenceAccountNameCaptions)
                    .HasForeignKey(d => d.ReferenceAccountNameCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReferenceAccountNameCaption");

                entity.HasOne(d => d.ReferenceAccountNumberCaptionNavigation)
                    .WithMany(p => p.TransactionTextReferenceAccountNumberCaptions)
                    .HasForeignKey(d => d.ReferenceAccountNumberCaption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ReferenceAccountNumberCaption");

                entity.HasOne(d => d.TermsNavigation)
                    .WithMany(p => p.TransactionTextTerms)
                    .HasForeignKey(d => d.Terms)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_Terms");

                entity.HasOne(d => d.ValidationTextErrorMessageNavigation)
                    .WithMany(p => p.ValidationTextErrorMessages)
                    .HasForeignKey(d => d.ValidationTextErrorMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ValidationTextErrorMessages");

                entity.HasOne(d => d.ValidationTextSuccessMessageNavigation)
                    .WithMany(p => p.ValidationTextSuccessMessages)
                    .HasForeignKey(d => d.ValidationTextSuccessMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionText_ValidationTextSuccessMessages");


            });

            modelBuilder.Entity<TransactionTypeListItem>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.DefaultAccountCurrency)
                    .HasDefaultValueSql("('KES')")
                    .IsFixedLength();

                entity.Property(e => e.Enabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.TxTypeGUIScreenlist).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.DefaultAccountCurrencyNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.DefaultAccountCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_Currency");

                entity.HasOne(d => d.TxLimitListNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxLimitList)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionLimitList");

                entity.HasOne(d => d.TxTextNavigationText)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxText)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_TransactionTypeListItem_TransactionText");

                entity.HasOne(d => d.TxTypeNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TransactionListItem_TransactionType");

                entity.HasOne(d => d.TxTypeGUIScreenlistNavigation)
                    .WithMany(p => p.TransactionTypeListItems)
                    .HasForeignKey(d => d.TxTypeGUIScreenlist)
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

            modelBuilder.Entity<UptimeComponentState>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UptimeMode>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Created).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasOne(d => d.ParentGroupNavigation)
                    .WithMany(p => p.InverseParentGroupNavigation)
                    .HasForeignKey(d => d.ParentGroup)
                    .HasConstraintName("FK_UserGroup_UserGroup");
            });

            modelBuilder.Entity<UserLock>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.InitiatingUserNavigation)
                    .WithMany(p => p.UserLocks)
                    .HasForeignKey(d => d.InitiatingUser)
                    .HasConstraintName("FK_UserLock_InitiatingUser");
            });

            modelBuilder.Entity<ValidationItem>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ValidationType)
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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ValidationItem)
                    .WithMany(p => p.ValidationItemValues)
                    .HasForeignKey(d => d.ValidationItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationItemValue_ValidationItem");
            });

            modelBuilder.Entity<ValidationList>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });

            modelBuilder.Entity<ValidationListValidationItem>(entity =>
            {
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
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.HasOne(d => d.ValidationItem)
                    .WithMany(p => p.ValidationTexts)
                    .HasForeignKey(d => d.ValidationItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ValidationText_ValidationItem");
            });

            modelBuilder.Entity<ValidationType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            });
        }
    }
}