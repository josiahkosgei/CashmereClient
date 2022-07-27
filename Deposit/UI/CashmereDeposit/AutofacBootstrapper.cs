using Autofac;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autofac.Extensions.DependencyInjection;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Repositories;
using Cashmere.Library.Standard.Security;
using CashmereDeposit.DI;
using CashmereDeposit.ViewModels;
using CashmereDeposit.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using CashmereDeposit.Utils;

namespace CashmereDeposit
{
    public class AutofacBootstrapper<TRootViewModel> : BootstrapperBase
    {
        #region Properties
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }
        protected IContainer Container { get; private set; }

        protected IDictionary<string, object> RootViewDisplaySettings { get; set; }
        /// <summary>
        /// Should the namespace convention be enforced for type registration. The default is true.
        /// For views, this would require a views namespace to end with Views
        /// For view-models, this would require a view models namespace to end with ViewModels
        /// <remarks>Case is important as views would not match.</remarks>
        /// </summary>
        public bool EnforceNamespaceConvention { get; set; }

        /// <summary>
        /// Should the IoC automatically subscribe any types found that implement the
        /// IHandle interface at activation
        /// </summary>
        public bool AutoSubscribeEventAggegatorHandlers { get; set; }

        /// <summary>
        /// The base type required for a view model
        /// </summary>
        public Type ViewModelBaseType { get; set; }

        /// <summary>
        /// Method for creating the window manager
        /// </summary>
        public Func<IWindowManager> CreateWindowManager { get; set; }

        /// <summary>
        /// Method for creating the event aggregator
        /// </summary>
        public Func<IEventAggregator> CreateEventAggregator { get; set; }

        #endregion

        /// <summary>
        /// Do not override this method. This is where the IoC container is configured.
        /// <remarks>
        /// Will throw <see cref="System.ArgumentNullException"/> is either CreateWindowManager
        /// or CreateEventAggregator is null.
        /// </remarks>
        /// </summary>
        protected override void Configure()
        { //  allow base classes to change bootstrapper settings
            ConfigureBootstrapper();

            //  validate settings
            if (CreateWindowManager == null)
                throw new ArgumentNullException("CreateWindowManager");
            if (CreateEventAggregator == null)
                throw new ArgumentNullException("CreateEventAggregator");

            //  configure container
            var builder = new ContainerBuilder();

            //  register view models
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
              .Where(type => type.Name.EndsWith("ViewModel"))
              .Where(type => !EnforceNamespaceConvention || (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("ViewModels")))
              .Where(type => type.GetInterface(ViewModelBaseType.Name, false) != null)
              //.AsImplementedInterfaces()
              .AsSelf()
              .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
              .Where(type => type.Name.EndsWith("View"))
              .Where(type => !EnforceNamespaceConvention || (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("Views")))
              .AsSelf()
              .InstancePerDependency();

            //  register Models
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
              .Where(type => type.Name.EndsWith("Model"))
              .Where(type => !EnforceNamespaceConvention || (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("Model")))
              .AsSelf()
              .InstancePerDependency();

            //  register UserControls
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
              .Where(Func)
              .AsSelf()
              .InstancePerDependency();

            //  register userControls
            //builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            //    .Where(type => type.Name.EndsWith("View"))
            //    .Where(type => !EnforceNamespaceConvention || (!(string.IsNullOrWhiteSpace(type.Namespace)) && type.Namespace.EndsWith("Views")))
            //    .AsSelf()
            //    .InstancePerDependency();

            builder.Register(c => CreateWindowManager()).InstancePerLifetimeScope();
            builder.Register<IEventAggregator>(c => CreateEventAggregator()).InstancePerLifetimeScope();

            if (AutoSubscribeEventAggegatorHandlers)
                builder.RegisterModule<EventAggregationAutoSubscriptionModule>();

            var configurationBuilder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = configurationBuilder.Build();

            ConfigureContainer(builder);
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            ConfigureServices(serviceCollection);

            builder.Populate(serviceCollection);

            Container = builder.Build();
            ServiceProvider = new AutofacServiceProvider(Container);
        }

        private bool Func(Type type)
        {
            return !EnforceNamespaceConvention || (!string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace.EndsWith("UserControls"));
        }

        /// <summary>
        /// Do not override unless you plan to full replace the logic. This is how the framework
        /// retrieves services from the Autofac container.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>The located service.</returns>
        protected override object GetInstance(Type service, string key)
        {
            object instance;
            if (string.IsNullOrWhiteSpace(key))
            {
                if (Container.TryResolve(service, out instance))
                    return instance;
            }
            else
            {
                if (Container.TryResolveNamed(key, service, out instance))
                    return instance;
            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        /// <summary>
        /// Do not override unless you plan to full replace the logic. This is how the framework
        /// retrieves services from the Autofac container.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>The located services.</returns>
        protected override IEnumerable<object>? GetAllInstances(Type service)
        {
            return Container.Resolve(typeof(IEnumerable<>).MakeGenericType(new[] { service })) as IEnumerable<object>;
        }

        /// <summary>
        /// Do not override unless you plan to full replace the logic. This is how the framework
        /// retrieves services from the Autofac container.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            Container.InjectProperties(instance);
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
        protected virtual void ConfigureBootstrapper()
        { //  by default, enforce the namespace convention
            EnforceNamespaceConvention = true;
            // default is to auto subscribe known event aggregators
            AutoSubscribeEventAggegatorHandlers = false;
            //  the default view model base type
            ViewModelBaseType = typeof(System.ComponentModel.INotifyPropertyChanged);
            //  default window manager
            CreateWindowManager = () => new WindowManager();
            //  default event aggregator
            CreateEventAggregator = () => new EventAggregator();

            var config = new TypeMappingConfiguration
            {
                DefaultSubNamespaceForViews = typeof(StartupView).Namespace,
                DefaultSubNamespaceForViewModels = typeof(StartupViewModel).Namespace
            };
            ViewLocator.ConfigureTypeMappings(config);
            ViewModelLocator.ConfigureTypeMappings(config);
        }

        /// <summary>
        /// Override to include your own Autofac configuration after the framework has finished its configuration, but
        /// before the container is created.
        /// </summary>
        /// <param name="containerBuilder">The Autofac configuration builder.</param>
        protected virtual void ConfigureContainer(ContainerBuilder containerBuilder)
        {
        }

        protected virtual void RegisterComponents(ContainerBuilder builder) { }

        /// <summary>Override this to add custom behavior on exit.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        protected override void OnExit(object sender, EventArgs e)
        {
            Container.Dispose();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootViewForAsync<TRootViewModel>(RootViewDisplaySettings);
        }

        private void ConfigureServices(IServiceCollection services)
        {


            services.AddTransient<DepositorDBContext>();
            services.AddTransient<DepositorContextFactory>();
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            //string connectionString = @"Data Source=.\;Initial Catalog=DepositorProduction;Integrated Security=True";
            //services.AddDbContext<DepositorDBContext>(sqlServerOptionsAction =>
            //    sqlServerOptionsAction.UseSqlServer(connectionString));

            //services.AddDbContext<DepositorDBContext>(options =>
            //    {
            //        if (options is null)
            //        {
            //            throw new ArgumentNullException(nameof(options));
            //        }

            //        options.UseSqlServer(connectionString,
            //        sqlServerOptionsAction: sqlOptions =>
            //        {
            //            sqlOptions.EnableRetryOnFailure(
            //            maxRetryCount: 10,
            //            maxRetryDelay: TimeSpan.FromSeconds(30),
            //            errorNumbersToAdd: null);
            //        });
            //    });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddHttpClient("CashmereDepositHttpClient", client => { });
            //using (new DepositorDBContext())
            //{
            //    services.AddHttpClient("CashmereDepositHttpClient", client => { }).ConfigurePrimaryHttpMessageHandler(_ =>
            //    {
            //        Device device;
            //        using (DepositorDBContext _depositorDBContext = new DepositorDBContext())
            //        {
            //            device = GetDevice(_depositorDBContext);
            //        }

            //        var handler = new HMACDelegatingHandler(device.AppId, device.AppKey);
            //        return handler;
            //    });

            //}

            services.AddHttpClient("CDM_APIClient", client => { });
            // InitDatabase(services);
        }

        private Device GetDevice(DepositorDBContext dbContext)
        {
            return dbContext.Devices.FirstOrDefault(x => x.MachineName == Environment.MachineName) ?? throw new Exception("Device not set correctly in database. Device is null during start up.");
        }
        /// <summary>
        /// Run migrations against Environment set
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="app"></param>
        public void InitDatabase(IServiceCollection serviceCollection)
        {
            var provider = serviceCollection.BuildServiceProvider();

            using (var serviceScope = provider.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DepositorDBContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
        }

    }
}