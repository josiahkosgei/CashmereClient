using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using Autofac.Features.ResolveAnything;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Repositories;
using Cashmere.Library.Standard.Logging;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Properties;
using CashmereDeposit.UserControls;
using CashmereDeposit.Utils;
using CashmereDeposit.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CashmereDeposit
{
    internal class MainBootstrapper : AutofacBootstrapper<StartupViewModel>
    {
        public MainBootstrapper()
          : base()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(UtilExtentionMethods.PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");


            RootViewDisplaySettings = new Dictionary<string, object>
            {

                { nameof(Window.Title), "Cashmere Deposit" },
                { nameof(Window.WindowStyle), WindowStyle.None },
                { nameof(Window.Topmost), Settings.Default.GUI_ALWAYS_ON_TOP },
                { nameof(Window.WindowState), WindowState.Maximized  },
                { nameof(Window.WindowStartupLocation), WindowStartupLocation.CenterScreen }
            };
            if (!Settings.Default.GUI_SHOW_MOUSE_CURSOR)
                RootViewDisplaySettings.Add("Cursor", Cursors.None);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ATMScreenCommandViewModel", "CashmereDeposit.Views.WaitForProcessScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*InputScreenViewModel", "CashmereDeposit.Views.CustomerInputScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ListScreenViewModel", "CashmereDeposit.Views.CustomerListScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*VerifyDetailsScreenViewModel", "CashmereDeposit.Views.CustomerVerifyDetailsScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*FormViewModel", "CashmereDeposit.Views.FormScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ATMViewModel", "CashmereDeposit.Views.ATMScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*DialogueBoxViewModel", "CashmereDeposit.Views.DialogueBoxView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*SearchScreenViewModel", "CashmereDeposit.Views.CustomerSearchScreenView");

        }

        /// <summary>
        /// Override to provide configuration prior to the Autofac configuration. You must call the base version BEFORE any 
        /// other statement or the behaviour is undefined.
        /// Current Defaults:
        ///   EnforceNamespaceConvention = true
        ///   ViewModelBaseType = <see cref="System.ComponentModel.INotifyPropertyChanged"/> 
        ///   CreateWindowManager = <see cref="WindowManager"/> 
        ///   CreateEventAggregator = <see cref="EventAggregator"/>
        /// </summary>
        protected override void ConfigureBootstrapper()
        {
            //  you must call the base version first!
            base.ConfigureBootstrapper();

            //  change our view model base type
            ViewModelBaseType = typeof(IShell);
        }

        /// <summary>
        /// Override to include your own Autofac configuration after the framework has finished its configuration, but 
        /// before the container is created.
        /// </summary>
        /// <param name="containerBuilder">The Autofac configuration builder.</param>
        protected override void ConfigureContainer(ContainerBuilder containerBuilder)
        {

            //containerBuilder.RegisterType<CashmereDeviceStatus>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<BusyIndicator>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<FullAlphanumericKeyboard>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<NumericKeypad>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<ScreenFooter>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<ScreenHeader>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<SummaryScreen>().AsSelf().InstancePerLifetimeScope();
            //containerBuilder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>().SingleInstance();
            // CashmereLogger : ICashmereLogger
            containerBuilder.RegisterType<CashmereLogger>().As<ICashmereLogger>().InstancePerLifetimeScope();

            containerBuilder.Register(c => c.Resolve<IHttpClientFactory>().CreateClient("CashmereDepositHttpClient")).As<HttpClient>().SingleInstance();
            containerBuilder.Register(c => c.Resolve<IHttpClientFactory>().CreateClient("CDM_APIClient")).As<HttpClient>().SingleInstance();

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            containerBuilder.RegisterType<DepositorDBContext>().As<DbContext>()
                .InstancePerLifetimeScope();
            containerBuilder
                .RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            //containerBuilder
            //    .RegisterType<DepositorDBContext>()
            //    .WithParameter("options", DepositorContextFactory.Get())
            //    .InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IAsyncRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceRepository)).As(typeof(IDeviceRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ApplicationLogRepository)).As(typeof(IApplicationLogRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(EscrowJamRepository)).As(typeof(IEscrowJamRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CITRepository)).As(typeof(ICITRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionRepository)).As(typeof(ITransactionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DepositorSessionRepository)).As(typeof(IDepositorSessionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIScreenRepository)).As(typeof(IGUIScreenRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GuiScreenListScreenRepository)).As(typeof(IGuiScreenListScreenRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceStatusRepository)).As(typeof(IDeviceStatusRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionTypeListItemRepository)).As(typeof(ITransactionTypeListItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceLockRepository)).As(typeof(IDeviceLockRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceLoginRepository)).As(typeof(IDeviceLoginRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(UserLockRepository)).As(typeof(IUserLockRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ApplicationUserRepository)).As(typeof(IApplicationUserRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ActivityRepository)).As(typeof(IActivityRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(PermissionRepository)).As(typeof(IPermissionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(RoleRepository)).As(typeof(IRoleRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(PasswordPolicyRepository)).As(typeof(IPasswordPolicyRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(PasswordHistoryRepository)).As(typeof(IPasswordHistoryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertAttachmentTypeRepository)).As(typeof(IAlertAttachmentTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertEmailRepository)).As(typeof(IAlertEmailRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertEmailAttachmentRepository)).As(typeof(IAlertEmailAttachmentRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertEventRepository)).As(typeof(IAlertEventRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertMessageRegistryRepository)).As(typeof(IAlertMessageRegistryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertMessageTypeRepository)).As(typeof(IAlertMessageTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(AlertSMSRepository)).As(typeof(IAlertSMSRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ApplicationExceptionRepository)).As(typeof(IApplicationExceptionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(BankRepository)).As(typeof(IBankRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(BranchRepository)).As(typeof(IBranchRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CITDenominationRepository)).As(typeof(ICITDenominationRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CITPrintoutRepository)).As(typeof(ICITPrintoutRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CITTransactionRepository)).As(typeof(ICITTransactionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ConfigRepository)).As(typeof(IConfigRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ConfigCategoryRepository)).As(typeof(IConfigCategoryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ConfigGroupRepository)).As(typeof(IConfigGroupRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CountryRepository)).As(typeof(ICountryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CrashEventRepository)).As(typeof(ICrashEventRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CurrencyRepository)).As(typeof(ICurrencyRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CurrencyListRepository)).As(typeof(ICurrencyListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(CurrencyListCurrencyRepository)).As(typeof(ICurrencyListCurrencyRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DenominationDetailRepository)).As(typeof(IDenominationDetailRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceCITSuspenseAccountRepository)).As(typeof(IDeviceCITSuspenseAccountRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceConfigRepository)).As(typeof(IDeviceConfigRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DevicePrinterRepository)).As(typeof(IDevicePrinterRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceSuspenseAccountRepository)).As(typeof(IDeviceSuspenseAccountRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(DeviceTypeRepository)).As(typeof(IDeviceTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIPrepopItemRepository)).As(typeof(IGUIPrepopItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIPrepopListRepository)).As(typeof(IGUIPrepopListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIPrepopListItemRepository)).As(typeof(IGUIPrepopListItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIScreenListRepository)).As(typeof(IGuiScreenListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIScreenTextRepository)).As(typeof(IGuiScreenTextRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(GUIScreenTypeRepository)).As(typeof(IGuiScreenTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(LanguageRepository)).As(typeof(ILanguageRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(LanguageListRepository)).As(typeof(ILanguageListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(LanguageListLanguageRepository)).As(typeof(ILanguageListLanguageRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(PrinterStatusRepository)).As(typeof(IPrinterStatusRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(PrintoutRepository)).As(typeof(IPrintoutRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(SessionExceptionRepository)).As(typeof(ISessionExceptionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(SysTextItemRepository)).As(typeof(ISysTextItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(SysTextItemCategoryRepository)).As(typeof(ISysTextItemCategoryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(SysTextItemTypeRepository)).As(typeof(ISysTextItemTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(SysTextTranslationRepository)).As(typeof(ISysTextTranslationRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TextItemRepository)).As(typeof(ITextItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TextItemCategoryRepository)).As(typeof(ITextItemCategoryRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TextItemTypeRepository)).As(typeof(ITextItemTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TextTranslationRepository)).As(typeof(ITextTranslationRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ThisDeviceRepository)).As(typeof(IThisDeviceRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionExceptionRepository)).As(typeof(ITransactionExceptionRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionLimitListRepository)).As(typeof(ITransactionLimitListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionLimitListItemRepository)).As(typeof(ITransactionLimitListItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionTextRepository)).As(typeof(ITransactionTextRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionTypeRepository)).As(typeof(ITransactionTypeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionTypeListRepository)).As(typeof(ITransactionTypeListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(TransactionTypeListTransactionTypeListItemRepository)).As(typeof(ITransactionTypeListTransactionTypeListItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(UptimeComponentStateRepository)).As(typeof(IUptimeComponentStateRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(UptimeModeRepository)).As(typeof(IUptimeModeRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(UserGroupRepository)).As(typeof(IUserGroupRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationItemRepository)).As(typeof(IValidationItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationItemValueRepository)).As(typeof(IValidationItemValueRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationListRepository)).As(typeof(IValidationListRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationListValidationItemRepository)).As(typeof(IValidationListValidationItemRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationTextRepository)).As(typeof(IValidationTextRepository)).InstancePerLifetimeScope();
            containerBuilder.RegisterType(typeof(ValidationTypeRepository)).As(typeof(IValidationTypeRepository)).InstancePerLifetimeScope();

            base.ConfigureContainer(containerBuilder);
        }
    }

}
