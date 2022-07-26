
using Caliburn.Micro;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Models;
using CashmereDeposit.Properties;
using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;
using CashmereDeposit.Startup;
using Microsoft.EntityFrameworkCore;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.ViewModels
{
    public class StartupViewModel : Conductor<Screen>, IShell, IDisposable
    {

        private DispatcherTimer _startupTimer = new(DispatcherPriority.Send);
        private DispatcherTimer _outOfOrderTimer = new(DispatcherPriority.Send);

        public CashmereLogger Log { get; set; }

        public AlertManager AlertManager { get; set; }

        private ApplicationViewModel ApplicationViewModel { get; set; }
        private static IDeviceRepository _iDeviceRepository { get; set; }
        //private static DepositorDBContext _depositorDBContext { get; set; }
        private readonly IDeviceStatusRepository _deviceStatusRepository;
        public StartupViewModel()
        {

            _iDeviceRepository = IoC.Get<IDeviceRepository>();
            //  _depositorDBContext = IoC.Get<DepositorDBContext>();
            _deviceStatusRepository = IoC.Get<IDeviceStatusRepository>();
            Log = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version?.ToString(), "CashmereDepositLog", null);
            AppDomain.CurrentDomain.UnhandledException += CrashHandler;
            ApplicationViewModel.DeviceConfiguration = DeviceConfiguration.Initialise();
            ActivateItemAsync(new StartupImageViewModel());
            _startupTimer.Interval = TimeSpan.FromSeconds(10.0);
            _startupTimer.Tick += StartupTimer_Tick;
            _startupTimer.IsEnabled = true;
            _outOfOrderTimer.Interval = TimeSpan.FromSeconds(11.0);
            _outOfOrderTimer.Tick += OutOfOrderTimer_TickAsync;
            _outOfOrderTimer.IsEnabled = true;
            WebAPI_StartSelfHost();
        }

        public sealed override Task ActivateItemAsync(Screen item, CancellationToken cancellationToken = new())
        {
            return base.ActivateItemAsync(item, cancellationToken);
        }

        public async void WebAPI_StartSelfHost()
        {
            try
            {
                var url = Settings.Default.OWIN_BASE_ADDRESS ?? "http://localhost:9000/";

                Log.Info(nameof(StartupViewModel), "OWIN Start", nameof(WebAPI_StartSelfHost), "Starting server at {0}", new { url });
                var deviceConfiguration = ApplicationViewModel.DeviceConfiguration;
                if ((deviceConfiguration.ALLOW_WEB_SERVER ? 1 : 0) == 0)
                    return;
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

        private async void OutOfOrderTimer_TickAsync(object sender, EventArgs e)
        {
            _outOfOrderTimer.Stop();
            _outOfOrderTimer = null;
            try
            {
                var device = GetDeviceAsync();
                try
                {
                    AlertManager = new AlertManager(new DepositorLogger(null), DeviceConfiguration.Initialise().API_COMMSERV_URI, device.AppId, device.AppKey, device.MachineName);
                    ApplicationViewModel = new ApplicationViewModel();
                    ChangeActiveItemAsync(ApplicationViewModel, true);
                }
                catch (Exception ex)
                {
                    ActivateItemAsync(new OutOfOrderFatalScreenViewModel());
                    AlertManager?.SendAlert(new AlertDeviceStartupFailed(ex?.Message, device, DateTime.Now));
                    CrashHandler(this, new UnhandledExceptionEventArgs(ex, false));
                    var deviceStatus = await _deviceStatusRepository.GetByDeviceId(device.Id);
                    if (deviceStatus == null)
                    {
                        deviceStatus = CashmereDepositCommonClasses.GenerateDeviceStatus(deviceID: device.Id);
                        await _deviceStatusRepository.AddAsync(deviceStatus);
                    }
                    deviceStatus.CurrentStatus |= 1024;
                    deviceStatus.Modified = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                CrashHandler(this, new UnhandledExceptionEventArgs(ex, false));
            }
            // SaveToDatabase();
        }

        private void CrashHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var exceptionObject = (Exception)args.ExceptionObject;
            try
            {
                Console.WriteLine(string.Format("Crash Handler caught : {0}", exceptionObject.MessageString()));
                Log.Fatal(nameof(StartupViewModel), ApplicationErrorConst.ERROR_CRASH.ToString(), nameof(CrashHandler), "Crash Handler caught : {0}", new object[1]
                {
          exceptionObject.MessageString()
                });
                var device = GetDeviceAsync();

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


        private static Device GetDeviceAsync()
        {
            return _iDeviceRepository.GetDevice(Environment.MachineName).ContinueWith(x => x.Result).Result ?? throw new Exception("Device: " + Environment.MachineName + " not set correctly in database. Device is null during start up.");
            //return device ?? throw new Exception("Device: " + Environment.MachineName + " not set correctly in database. Device is null during start up.");
        }
        private readonly CompositeDisposable _disposables = new();
        private bool _isDisposed = false;
        // use this in derived class
        // protected override void Dispose(bool isDisposing)
        // use this in non-derived class
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (isDisposing)
            {
                // free managed resources here
                _disposables.Dispose();
            }

            // free unmanaged resources (unmanaged objects) and override a finalizer below.
            // set large fields to null.

            _isDisposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
