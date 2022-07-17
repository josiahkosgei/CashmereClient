
using Caliburn.Micro;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using CashmereDeposit.Interfaces;
using CashmereDeposit.Models;
using CashmereDeposit.Properties;
using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Hosting;
using Caliburn.Micro.ReactiveUI;
using CashmereDeposit.Startup;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.ViewModels
{
    public class StartupViewModel : Conductor<Screen>.Collection.OneActive, IDisposable
    {

        private DispatcherTimer _startupTimer = new DispatcherTimer(DispatcherPriority.Send);
        private DispatcherTimer _outOfOrderTimer = new DispatcherTimer(DispatcherPriority.Send);

        public CashmereLogger Log { get; set; }

        public AlertManager AlertManager { get; set; }

        private ApplicationViewModel ApplicationViewModel { get; set; }

        //

        public StartupViewModel()
        {
            //ApplicationViewModel = new ApplicationViewModel(this);

            using (new DepositorDBContext())
            {

                Log = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), "CashmereDepositLog", null);
                AppDomain.CurrentDomain.UnhandledException += CrashHandler;
                ApplicationViewModel.DeviceConfiguration = DeviceConfiguration.Initialise();
                ActivateItemAsync(new StartupImageViewModel());
                _startupTimer.Interval = TimeSpan.FromSeconds(10.0);
                _startupTimer.Tick += StartupTimer_Tick;
                _startupTimer.IsEnabled = true;
                _outOfOrderTimer.Interval = TimeSpan.FromSeconds(11.0);
                _outOfOrderTimer.Tick += OutOfOrderTimer_Tick;
                _outOfOrderTimer.IsEnabled = true;
                WebAPI_StartSelfHost();
            }
        }

        public sealed override Task ActivateItemAsync(Screen item, CancellationToken cancellationToken = new CancellationToken())
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        public async void WebAPI_StartSelfHost()
        {
            try
            {
                var url = Settings.Default.OWIN_BASE_ADDRESS ?? "http://localhost:9000/";

                Log.Info(nameof(StartupViewModel), "OWIN Start", nameof(WebAPI_StartSelfHost), "Starting server at {0}", new { url });
                DeviceConfiguration deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                //if ((deviceConfiguration.ALLOW_WEB_SERVER ? 1 : 0) == 0)
                //    return;
                //WebApp.Start<Startup>(url);
                await HostBuilder.Start();
            }
            catch (Exception ex)
            {
                Log.Error(nameof(StartupViewModel), "OWIN Start", nameof(WebAPI_StartSelfHost), "Error starting OWIN server: {0}", new { ex.Message });
            }
        }

        private static void TestCashmereOwin(string baseAddress)
        {
        }

        private void StartupTimer_Tick(object sender, EventArgs e)
        {
            _startupTimer.Stop();
            _startupTimer = null;
            ChangeActiveItemAsync(new OutOfOrderFatalScreenViewModel(), true);
        }

        private void OutOfOrderTimer_Tick(object sender, EventArgs e)
        {
            _outOfOrderTimer.Stop();
            _outOfOrderTimer = null;
            using DepositorDBContext dbContext = new DepositorDBContext();
            try
            {
                Device device = GetDevice(dbContext);
                try
                {
                    AlertManager = new AlertManager(new DepositorLogger(null), DeviceConfiguration.Initialise().API_COMMSERV_URI, device.AppId, device.AppKey, device.MachineName);
                    ApplicationViewModel = new ApplicationViewModel(this);
                    ChangeActiveItemAsync(ApplicationViewModel, true);
                }
                catch (Exception ex)
                {
                    ActivateItemAsync(new OutOfOrderFatalScreenViewModel());
                    AlertManager?.SendAlert(new AlertDeviceStartupFailed(ex?.Message, device, DateTime.Now));
                    CrashHandler(this, new UnhandledExceptionEventArgs(ex, false));
                    DeviceStatus deviceStatu;
                    if (dbContext == null)
                        deviceStatu = null;
                    else
                        deviceStatu = dbContext.DeviceStatus.FirstOrDefault(x => x.DeviceId == device.Id);
                    DeviceStatus entity = deviceStatu;
                    if (entity == null)
                    {
                        entity = CashmereDepositCommonClasses.GenerateDeviceStatus(device.Id, dbContext);
                        dbContext.DeviceStatus.Add(entity);
                    }
                    entity.CurrentStatus |= 1024;
                    entity.Modified = new DateTime?(DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                CrashHandler(this, new UnhandledExceptionEventArgs(ex, false));
            }
            SaveToDatabase(dbContext);
        }

        private void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception exceptionObject = (Exception)args.ExceptionObject;
            try
            {
                Console.WriteLine(string.Format("Crash Handler caught : {0}", exceptionObject.MessageString()));
                Log.Fatal(nameof(StartupViewModel), ApplicationErrorConst.ERROR_CRASH.ToString(), nameof(CrashHandler), "Crash Handler caught : {0}", new object[1]
                {
          exceptionObject.MessageString()
                });
                using DepositorDBContext dbContext = new DepositorDBContext();
                Device device = GetDevice(dbContext);
                SaveToDatabase(dbContext);
                AlertManager?.SendAlert(new AlertApplicationCrash(device, exceptionObject.Message, DateTime.Now, exceptionObject.StackTrace));
            }
            catch (Exception ex1)
            {
                try
                {
                    File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\[" + DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss.fff") + "] Crash.log", string.Format("Crash Handler caught : {0}>>{1}", ex1.MessageString(), exceptionObject.MessageString()));
                }
                catch (Exception ex2)
                {
                }
            }
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            Application.Current.Shutdown();
        }

        public void SaveToDatabase(DepositorDBContext dbContext)
        {
            try
            {
                ApplicationViewModel.SaveToDatabase(dbContext);
            }
            catch (ValidationException ex)
            {
                string errorDetail = string.Format("{0}>>{1}>>{2}>stack>{3}>Validation Errors: ", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message, ex.StackTrace);
                foreach (var entityValidationError in ex.ValidationResult.MemberNames)
                {
                    errorDetail += ">validation error>";
                    //foreach (ValidationError validationError in (IEnumerable<ValidationError>) entityValidationError)
                    errorDetail = errorDetail + "ErrorMessage=>" + entityValidationError;
                }
                Console.WriteLine(errorDetail);

                Log.Error(nameof(StartupViewModel), ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), nameof(SaveToDatabase), "Error Saving to Database: {0}>>{1}", new object[2]
                {
          (object) ex.MessageString(),
          errorDetail
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                Log.Error(nameof(StartupViewModel), ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), nameof(SaveToDatabase), "Error Saving to Database: {0}", new object[1]
                {
          ex.MessageString()
                });
            }
        }

        private static Device GetDevice(DepositorDBContext dbContext)
        {
            return dbContext.Devices.Include(x => x.Branch).Include(x => x.ConfigGroupNavigation)
                .Include(x => x.LanguageListNavigation)
                .ThenInclude(x => x.LanguageListLanguages)
                //.ThenInclude(x => x.LanguageListNavigation)
                //.ThenInclude(x => x.LanguageListLanguages)
                .Include(x => x.GUIScreenListNavigation)
                .Include(x => x.GUIScreenListNavigation.GuiScreenListScreens)
                .ThenInclude(x => x.ScreenNavigation)
                //.Include(x => x.GUIScreenListNavigation.GuiScreenListScreens.Select(q => q.ScreenNavigation))
                .Include(x => x.CurrencyListNavigation)
                .Include(x => x.CurrencyListNavigation.DefaultCurrencyNavigation)
                .Include(x => x.ConfigGroupNavigation)
                .Include(x => x.TransactionTypeListNavigation)
                .ThenInclude(x => x.TransactionTypeListTransactionTypeListItems)
                .ThenInclude(x => x.TxtypeListItemNavigation)
                .FirstOrDefault(x => x.MachineName == Environment.MachineName) ?? throw new Exception( "Device: "+Environment.MachineName+" not set correctly in database. Device is null during start up.");
        }
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private bool _isDisposed = false;
        // use this in derived class
        // protected override void Dispose(bool isDisposing)
        // use this in non-derived class
        protected virtual void Dispose(bool isDisposing)
        {
            if (this._isDisposed)
                return;

            if (isDisposing)
            {
                // free managed resources here
                this._disposables.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            // set large fields to null.

            this._isDisposed = true;
        }
        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
