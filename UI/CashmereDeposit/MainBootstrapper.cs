﻿using System.Collections.Generic;
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
        ///   CreateWindowManager = <see cref="Caliburn.Micro.WindowManager"/> 
        ///   CreateEventAggregator = <see cref="Caliburn.Micro.EventAggregator"/>
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

            containerBuilder
                .RegisterType<DepositorDBContext>()
                .WithParameter("options", DepositorContextFactory.Get())
                .InstancePerLifetimeScope();

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

            base.ConfigureContainer(containerBuilder);
        }
    }

}
