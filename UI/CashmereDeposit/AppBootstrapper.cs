using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Autofac;
using CashmereDeposit.Interfaces;
using CashmereDeposit.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CashmereDeposit
{
    public class AppBootstrapper : AutofacBootstrapper<IShell>
	{
		public AppBootstrapper()
		{
			Initialize();
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

		protected override void Configure()
		{
			base.Configure();
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
			//builder.RegisterType<ErrorHandler>().As<IErrorHandler>();
			//builder.RegisterType<MainController>().AsSelf().SingleInstance();
			//builder.RegisterType<ProfileService>().As<IProfileService>();
			//builder.RegisterType<ProfileFactory>().As<IProfileFactory>();
			//builder.RegisterType<StorageService>().As<IStorageService>();
			//builder.RegisterType<RegExService>().As<IRegExService>().SingleInstance();
			containerBuilder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>().SingleInstance();
			//builder.RegisterType<BusyStateManager>().As<IBusyStateManager>().SingleInstance();

		}

		protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs ex)
		{
			base.OnUnhandledException(sender, ex);
			//MessageBox.Show($"Unhandled exception: {ex.Exception.Message}");
		}
	}
}
