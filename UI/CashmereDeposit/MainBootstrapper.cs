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
using CashmereDeposit.Interfaces;
using CashmereDeposit.Properties;
using CashmereDeposit.Utils;
using CashmereDeposit.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit
{
    internal class MainBootstrapper : AutofacBootstrapper<StartupViewModel>
    {
        public MainBootstrapper()
          : base()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(UtilExtentionMethods.PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");
            

            this.RootViewDisplaySettings = new Dictionary<string, object>
            {
                { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                { "Title", "Cashmere Deposit" },
                { "WindowState", WindowState.Maximized },
                { "WindowStyle", WindowStyle.None },
                { "Topmost", Settings.Default.GUI_ALWAYS_ON_TOP }
            };
            if (!Settings.Default.GUI_SHOW_MOUSE_CURSOR)
                this.RootViewDisplaySettings.Add("Cursor", Cursors.Arrow);
        }
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ATMScreenCommandViewModel", "CashmereDeposit.Views.WaitForProcessScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*InputScreenViewModel", "CashmereDeposit.Views.CustomerInputScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ListScreenViewModel", "CashmereDeposit.Views.CustomerListScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*VerifyDetailsScreenViewModel", "CashmereDeposit.Views.CustomerVerifyDetailsScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*FormViewModel", "CashmereDeposit.Views.FormScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*ATMViewModel", "CashmereDeposit.Views.ATMScreenView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*DialogueBoxViewModel", "CashmereDeposit.Views.DialogueBoxView");
            ViewLocator.NameTransformer.AddRule("^CashmereDeposit.ViewModels.*SearchScreenViewModel", "CashmereDeposit.Views.CustomerSearchScreenView");

            base.OnStartup(sender, e);
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
            //containerBuilder.RegisterType<ApplicationViewModel>().As<IShell>();
            //containerBuilder.RegisterType<StartupViewModel>().As<IShell>();
            
            containerBuilder.RegisterType<StartupViewModel>().AsSelf().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ApplicationViewModel>().AsSelf().InstancePerLifetimeScope();
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

            containerBuilder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IAsyncRepository<>)).SingleInstance();

            base.ConfigureContainer(containerBuilder);
        }
    }

}
