using System.Net.Http;
using System.Windows.Controls;
using Autofac;
using Autofac.Features.ResolveAnything;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Repositories;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Utils;
using CashmereDeposit.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit
{
    internal class MainBootstrapper : AutofacBootstrapper<IShell>
    {
        public MainBootstrapper()
          : base()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(UtilExtentionMethods.PasswordBoxHelper.BoundPasswordProperty, "Password", "PasswordChanged");
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
            base.ConfigureContainer(containerBuilder);
            containerBuilder.RegisterType<StartupViewModel>().As<IShell>();

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
        }
    }

}
