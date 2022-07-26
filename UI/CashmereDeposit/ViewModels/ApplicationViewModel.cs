using Caliburn.Micro;
using CashAccSysDeviceManager;
using Cashmere.API.Messaging.CDM.GUIControl.AccountsLists;
using Cashmere.API.Messaging.CDM.GUIControl.Clients;
using Cashmere.API.Messaging.Integration;
using Cashmere.API.Messaging.Integration.Controllers;
using Cashmere.API.Messaging.Integration.ServerPing;
using Cashmere.API.Messaging.Integration.Transactions;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Models;
using CashmereDeposit.Models.Submodule;
using CashmereDeposit.UserControls;
using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;
using CashmereUtil.Licensing;
using DeviceManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Activity = Cashmere.Library.CashmereDataAccess.Entities.Activity;

namespace CashmereDeposit.ViewModels
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public class ApplicationViewModel : Conductor<Screen>, IShell
    {
        private bool _adminMode;
        private ApplicationUser _currentUser;
        private ApplicationUser _validatingUser;
        private CashAccSysDeviceManager.CashAccSysDeviceManager _deviceManager;
        private ApplicationState _currentApplicationState = ApplicationState.STARTUP;
        private DispatcherTimer statusTimer = new(DispatcherPriority.Send);

        public bool debugNoDevice { get; } = true;

        public bool debugNoCoreBanking { get; } = true;

        public bool debugDisableSafeSensor { get; } = true;

        public bool debugDisableBagSensor { get; } = true;

        public bool debugDisablePrinter { get; } = true;

        public static CashmereTranslationService CashmereTranslationService { get; set; }

        public object NavigationLock { get; set; } = new object();

        public object BagOpenLock { get; private set; } = new object();

        public object BagReplacedLock { get; private set; } = new object();

        public object DoorClosedLock { get; private set; } = new object();

        internal EscrowJam EscrowJam { get; set; }

        public bool AdminMode
        {
            get => _adminMode;
            set
            {
                _adminMode = value;
                if (!value)
                    return;
                UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.ADMIN);
            }
        }

        private DepositorPrinter Printer { get; set; }

        public static DepositorLogger Log { get; set; }

        public static DepositorLogger AlertLog { get; set; }

        public Random Rand { get; }

        public string CurrentLanguage { get; private set; } = "en-GB";

        public ApplicationUser CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                NotifyOfPropertyChange((Expression<Func<ApplicationUser>>)(() => CurrentUser));
            }
        }

        public ApplicationUser ValidatingUser
        {
            get => _validatingUser;
            set
            {
                _validatingUser = value;
                NotifyOfPropertyChange((Expression<Func<ApplicationUser>>)(() => ValidatingUser));
            }
        }

        public ApplicationModel ApplicationModel { get; }

        public AppSession CurrentSession { get; set; }

        public AppTransaction CurrentTransaction => CurrentSession?.Transaction;

        public CashAccSysDeviceManager.CashAccSysDeviceManager DeviceManager
        {
            get => _deviceManager;
            set => _deviceManager = value;
        }

        public List<Language> LanguagesAvailable => ApplicationModel.LanguageList;

        public List<Currency> CurrenciesAvailable => ApplicationModel.CurrencyList;

        public List<TransactionTypeListItem> TransactionTypesAvailable => ApplicationModel.TransactionList;

        public ApplicationState CurrentApplicationState
        {
            get => _currentApplicationState;
            set
            {
                if (DeviceManager != null)
                {
                    switch (DeviceManager.DeviceManagerMode)
                    {
                        case DeviceManagerMode.NONE:
                            if (value.ToString().StartsWith("CIT"))
                            {
                                DeviceManager.DeviceManagerMode = DeviceManagerMode.CIT;
                                break;
                            }
                            break;
                        case DeviceManagerMode.CIT:
                            if (!value.ToString().StartsWith("CIT"))
                            {
                                DeviceManager.DeviceManagerMode = DeviceManagerMode.NONE;
                                break;
                            }
                            break;
                    }
                }
                Log.DebugFormat(GetType().Name, "ApplicationStateChanged", "ApplicationState", "Changed from {0} to {1}", _currentApplicationState, value);
                _currentApplicationState = value;
                NotifyOfPropertyChange((Expression<Func<ApplicationState>>)(() => CurrentApplicationState));
                switch (value)
                {
                    case ApplicationState.SPLASH:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.ACTIVE);
                        break;
                    case ApplicationState.CIT_START:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.CIT);
                        break;
                    case ApplicationState.CIT_END:
                        UptimeMonitor.SetCurrentUptimeMode(UptimeModeType.OUT_OF_ORDER);
                        break;
                }
            }
        }

        public CashmereDeviceStatus ApplicationStatus { get; set; } = new CashmereDeviceStatus();

        public CIT lastCIT
        {
            get
            {

                Device device = ApplicationModel.GetDeviceAsync();
                return _citRepository.GetByDeviceId(device.Id).ContinueWith(r => r.Result).Result;

            }
        }

        public AlertManager AlertManager { get; set; }

        public static DeviceConfiguration DeviceConfiguration { get; set; }

        public LicenseMechanism License { get; private set; }

        private StartupViewModel StartupViewModel { get; }

        private static IDeviceRepository _deviceRepository;
        private readonly ICITRepository _citRepository;
        private readonly IEscrowJamRepository _escrowJamRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IDepositorSessionRepository _depositorSessionRepository;
        private readonly IGuiScreenListScreenRepository _guiScreenListScreenRepository;
        private readonly IDeviceStatusRepository _deviceStatusRepository;
        private readonly IGUIScreenRepository _gUIScreenRepository;
        private readonly ITransactionTypeListItemRepository _transactionTypeListItemRepository;
        private readonly IDeviceLockRepository _deviceLockRepository;
        private readonly IDeviceLoginRepository _deviceLoginRepository;
        private readonly IUserLockRepository _userLockRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IPermissionRepository _permissionRepository;

        //  private static ;

        private readonly StartupViewModel startupModel;

        public ApplicationViewModel()
        {

            _deviceRepository = IoC.Get<IDeviceRepository>();
            _citRepository = IoC.Get<ICITRepository>();
            _escrowJamRepository = IoC.Get<IEscrowJamRepository>();
            _transactionRepository = IoC.Get<ITransactionRepository>();
            _depositorSessionRepository = IoC.Get<IDepositorSessionRepository>();
            _guiScreenListScreenRepository = IoC.Get<IGuiScreenListScreenRepository>();
            _gUIScreenRepository = IoC.Get<IGUIScreenRepository>();
            _deviceStatusRepository = IoC.Get<IDeviceStatusRepository>();
            _transactionTypeListItemRepository = IoC.Get<ITransactionTypeListItemRepository>();
            _deviceLoginRepository = IoC.Get<IDeviceLoginRepository>();
            _userLockRepository = IoC.Get<IUserLockRepository>();
            _applicationUserRepository = IoC.Get<IApplicationUserRepository>();
            _activityRepository = IoC.Get<IActivityRepository>();
            _permissionRepository = IoC.Get<IPermissionRepository>();
            startupModel = IoC.Get<StartupViewModel>();

            ApplicationStatus.PropertyChanged += new PropertyChangedEventHandler(ApplicationStatus_PropertyChanged);
            StartupViewModel = startupModel;
            Log = new DepositorLogger(this);
            DeviceConfiguration = DeviceConfiguration.Initialise();
            AlertLog = new DepositorLogger(this, "DepositorCommunicationService");
            AlertManager = startupModel.AlertManager;
            AlertManager.Log = Log;
            Log.Info(GetType().Name, "Application Startup", "Constructor", "Initialising Application");

            //_deviceRepository.GetDevice(Environment.MachineName).ContinueWith(r=>r.Result).Result;
            InitialiseLicense();
            statusTimer.Interval = TimeSpan.FromSeconds(DeviceConfiguration.SERVER_POLL_INTERVAL);
            statusTimer.Tick += new EventHandler(statusTimer_Tick);
            statusTimer.IsEnabled = true;
            ApplicationModel = new ApplicationModel(this);
            ApplicationModel.DatabaseStorageErrorEvent += new EventHandler<EventArgs>(ApplicationModel_DatabaseStorageErrorEvent);
            SetCashmereDeviceState(CashmereDeviceState.DEVICE_MANAGER);
            InitialiseApp();
            Rand = new Random();
            CurrentApplicationState = ApplicationState.STARTUP_COMPLETE;
            if (CurrentApplicationState != ApplicationState.STARTUP_COMPLETE || ApplicationStatus.CashmereDeviceState != CashmereDeviceState.NONE)
                return;
            Log.Info(GetType().Name, "Application Startup", "ApplicationState.STARTUP_COMPLETE", "Application started successfully");
            AlertManager.SendAlert(new AlertDeviceStartupSuccess(ApplicationModel.GetDeviceAsync(), DateTime.Now));

        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new()
            {
                WorkerReportsProgress = false
            };
            backgroundWorker.DoWork += new DoWorkEventHandler(statusWorker_DoWork);
            backgroundWorker.RunWorkerAsync();
        }

        private void InitialiseLicense()
        {
            CashmereTranslationService = new CashmereTranslationService(this, License?.License);
            try
            {
                License = new LicenseMechanism();
                CashmereTranslationService = new CashmereTranslationService(this, License?.License);
            }
            catch (Exception ex)
            {
                SetCashmereDeviceState(CashmereDeviceState.LICENSE);
                NotifyOfPropertyChange((Expression<Func<CashmereDeviceStatus>>)(() => ApplicationStatus));
                NotifyOfPropertyChange((Expression<Func<ApplicationState>>)(() => CurrentApplicationState));
            }

        }

        public void InitialiseApp()
        {
            Log.Info(GetType().Name, nameof(InitialiseApp), "Initialisation", "Initialising Application");
            CheckHDDSpace();
            InitialiseDevice();
            if (CurrentApplicationState == ApplicationState.STARTUP)
            {
                SystemStartupChecks();
                InitialiseEmailManager();
                InitialisePrinter();
            }
            AlertManager?.InitialiseAlertManager();
            InitialiseUsersAndPermissions();
            InitialiseFolders();
            CurrentScreenIndex = 0;
            InitialiseCurrencyList();
            InitialiseScreenList();
            InitialiseLanguageList();
            InitialiseTransactionTypeList();
            InitialiseCoreBankingAsync().AsResult();
            SetLanguage(DeviceConfiguration.UI_CULTURE);
            ConnectToDevice();
            HandleIncompleteSession();
            NavigateFirstScreen();
        }

        private void statusWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            InitialiseLicense();
            InitialiseCoreBankingAsync().AsResult();
            CheckDeviceLockStatus();
            CheckHDDSpace();
        }

        private void CheckHDDSpace()
        {
            DriveInfo driveInfo = DriveInfo.GetDrives().FirstOrDefault(x => x.RootDirectory.FullName.Equals("c:\\", StringComparison.InvariantCultureIgnoreCase));
            if (driveInfo == null)
                return;
            Console.WriteLine(driveInfo.Name);
            if (!driveInfo.IsReady)
                return;
            long minHddSpace = DeviceConfiguration.MIN_HDD_SPACE;
            if (driveInfo.AvailableFreeSpace < minHddSpace)
            {
                if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.HDD_FULL))
                    return;
                SetCashmereDeviceState(CashmereDeviceState.HDD_FULL);
                double availableSpace = driveInfo.AvailableFreeSpace / 1073741824.0;
                double minimumSpace = minHddSpace / 1073741824.0;
                Log.InfoFormat(GetType().Name, "Device", "HDD FULL", "Setting CashmereDeviceState.HDD_FULL as HDD space of {0:0.##} GB < {1:0.##} GB", availableSpace, minimumSpace);
                AlertManager.SendAlert(new AlertHDDFull(availableSpace, minimumSpace, ApplicationModel.GetDeviceAsync(), DateTime.Now));
            }
            else
            {
                if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.HDD_FULL))
                    return;
                UnSetCashmereDeviceState(CashmereDeviceState.HDD_FULL);
            }

        }

        private void UnSetCashmereDeviceState(CashmereDeviceState state)
        {
            Log.Debug(GetType().Name, "Unset", "CashmereDeviceState", "{0}", new object[1]
            {
         state.ToString()
            });
            ApplicationStatus.CashmereDeviceState &= ~state;
            UptimeMonitor.UnSetCurrentUptimeComponentState(state);
        }

        private void SetCashmereDeviceState(CashmereDeviceState state)
        {
            Log.Debug(GetType().Name, "Set", "CashmereDeviceState", "{0}", new object[1]
            {
         state.ToString()
            });
            ApplicationStatus.CashmereDeviceState |= state;
            UptimeMonitor.SetCurrentUptimeComponentState(state);
        }

        private void CheckDeviceLockStatus()
        {
            Device device = ApplicationModel.GetDeviceAsync();
            if (device == null)
                return;
            DeviceManager.Enabled = device.Enabled;

        }

        private async Task InitialiseCoreBankingAsync()
        {
            ApplicationViewModel applicationViewModel = this;
            try
            {
                if (applicationViewModel.debugNoCoreBanking)
                {
                    if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                }
                else
                {
                    AppSession currentSession = applicationViewModel.CurrentSession;
                    bool allowConnectionError = currentSession == null || !currentSession.CountingStarted;
                    try
                    {
                        Device device = applicationViewModel.ApplicationModel.GetDeviceAsync();
                        Guid Id = device.Id;
                        Guid appId = applicationViewModel.ApplicationModel.GetDeviceAsync().AppId;
                        IntegrationServiceClient integrationServiceClient = new(DeviceConfiguration.API_INTEGRATION_URI, appId, device.AppKey, null);
                        IntegrationServerPingRequest request = new();
                        Guid guid = Guid.NewGuid();
                        request.SessionID = guid.ToString();
                        guid = Guid.NewGuid();
                        request.MessageID = guid.ToString();
                        request.AppID = appId;
                        request.AppName = device.MachineName;
                        request.Language = applicationViewModel.CurrentLanguage;
                        request.MessageDateTime = DateTime.Now;
                        IntegrationServerPingResponse serverPingResponse = await integrationServiceClient.ServerPingAsync(request);
                        applicationViewModel.CheckIntegrationResponseMessageDateTime(serverPingResponse.MessageDateTime);
                        applicationViewModel.ApplicationStatus.CoreBankingStatus = new CoreBankingStatus()
                        {
                            ServerOnline = serverPingResponse.ServerOnline
                        };
                        if (allowConnectionError && !serverPingResponse.ServerOnline)
                        {
                            CashmereDeviceStatus applicationStatus = applicationViewModel.ApplicationStatus;
                            if ((applicationStatus != null ? (applicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION) ? 1 : 0) : 0) == 0)
                            {
                                Log.ErrorFormat(applicationViewModel.GetType().Name, 92, ApplicationErrorConst.ERROR_CORE_BANKING.ToString(), "Could not connect to core banking with error: {0}>Server Error>{1}", serverPingResponse.PublicErrorMessage, serverPingResponse.ServerErrorMessage);
                                if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                                {
                                    Log.Debug(applicationViewModel.GetType().Name, "Device", "Device State Changed", "Setting CashmereDeviceState.SERVER_CONNECTION");

                                }

                            }
                        }
                        if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                            applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(applicationViewModel.GetType().Name, 92, ApplicationErrorConst.ERROR_CORE_BANKING.ToString(), "Could not connect to core banking with exception: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                        if (allowConnectionError)
                        {
                            if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                                applicationViewModel.SetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                        }
                        else
                            Log.DebugFormat(applicationViewModel.GetType().Name, "Device", "Device State Changed", "{0} = {1}. Ignoring Setting CashmereDeviceState.SERVER_CONNECTION", "allowConnectionError", allowConnectionError);
                    }
                }
            }
            finally
            {
                // _depositorDBContext?.Dispose();
            }
        }

        public void InitialiseUsersAndPermissions() => LogoffUsers();

        private void InitialiseEmailManager()
        {
        }

        private void InitialiseFolders() => Directory.CreateDirectory(DeviceConfiguration.TRANSACTION_LOG_FOLDER);

        private void InitialiseApplicationModel() => ApplicationModel.InitialiseApplicationModel();

        private void SystemStartupChecks()
        {
            Log.Debug(GetType().Name, nameof(SystemStartupChecks), "Initialisation", "Performing startup checks");
            Log.Debug(GetType().Name, "SystemStartupDatabaseCheck", "Initialisation", "Performing database check");
            if (!ApplicationModel.TestConnection())
            {
                Log.Fatal(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Could not connect to database during system startup, terminating...");
                OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_DATABASE_OFFLINE, "Could not connect to database during system startup, terminating...");
            }
            else if (_deviceRepository.GetDevice(Environment.MachineName).ContinueWith(r => r.Result).Result == null)
            {
                Log.Fatal(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "This Device does not exist in the system, terminating...");
                OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_DEVICE_DOES_NOT_EXIST, "This Device does not exist in the system, terminating...");
            }
            InitialiseApplicationModel();
            Log.Info(GetType().Name, "SystemStartupChecks Result", "Initialisation", "SUCCESS");

        }

        private void InitialisePrinter()
        {
            Log.Debug(GetType().Name, nameof(InitialisePrinter), "Initialisation", "Initialising Printer");
            if (CurrentApplicationState != ApplicationState.STARTUP)
                return;
            try
            {
                Printer = new DepositorPrinter(this, Log, DeviceConfiguration.RECEIPT_PRINTERPORT);
                Printer.PrinterStateChangedEvent += new EventHandler<DepositorPrinter.PrinterStateChangedEventArgs>(Printer_StatusChangedEvent);
                Log.Info(GetType().Name, "InitialisePrinter Result", "Initialisation", "SUCCESS");
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 75, ApplicationErrorConst.ERROR_PRINTER_ERROR.ToString(), "Could not connect to database during system startup: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
                Printer = null;
            }
        }

        private void InitialiseDevice()
        {

            Device device = _deviceRepository.GetDevice(Environment.MachineName).ContinueWith(r => r.Result).Result;
            if (device == null)
            {
                Log.FatalFormat(GetType().Name, 97, ApplicationErrorConst.ERROR_DEVICE_DOES_NOT_EXIST.ToString(), "Device with machine name = {0} does not exists in the local database.", Environment.MachineName);
                SetCashmereDeviceState(CashmereDeviceState.DATABASE);
            }
            DeviceConfiguration = DeviceConfiguration.Initialise();
            Log.Debug(GetType().Name, nameof(InitialiseDevice), "Initialisation", "Initialising Depositor Device");
            try
            {
                if (CurrentApplicationState == ApplicationState.STARTUP)
                {
                    if (string.IsNullOrWhiteSpace(device.MacAddress))
                    {
                        device.MacAddress = ExtentionMethods.GetDefaultMacAddress();

                    }
                    _deviceManager = new CashAccSysDeviceManager.CashAccSysDeviceManager(DeviceConfiguration.DEVICECONTROLLER_HOST, DeviceConfiguration.DEVICECONTROLLER_PORT, device?.MacAddress, 1234, DeviceConfiguration.FIX_DEVICE_PORT, DeviceConfiguration.FIX_CONTROLLER_PORT, DeviceConfiguration.BAGFULL_WARN_PERCENT, DeviceConfiguration.SENSOR_INVERT_DOOR, DeviceConfiguration.CONTROLLER_LOG_DIRECTORY);
                    if (DeviceManager == null)
                        throw new Exception("Error creating DeviceManager: _deviceManager is null");
                    DeviceManager.ConnectionEvent += new EventHandler<StringResult>(DeviceManager_ConnectionEvent);
                    DeviceManager.RaiseControllerStateChangedEvent += new EventHandler<ControllerStateChangedEventArgs>(DeviceManager_RaiseControllerStateChangedEvent);
                    DeviceManager.RaiseDeviceStateChangedEvent += new EventHandler<DeviceStateChangedEventArgs>(DeviceManager_RaiseDeviceStateChangedEvent);
                    DeviceManager.StatusReportEvent += new EventHandler<DeviceStatusChangedEventArgs>(DeviceManager_StatusReportEvent);
                    DeviceManager.NotifyCurrentTransactionStatusChangedEvent += new EventHandler<EventArgs>(DeviceManager_NotifyCurrentTransactionStatusChangedEvent);
                    DeviceManager.TransactionStartedEvent += new EventHandler<DeviceTransaction>(DeviceManager_TransactionStartedEvent);
                    DeviceManager.CashInStartedEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CashInStartedEvent);
                    DeviceManager.CountStartedEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountStartedEvent);
                    DeviceManager.CountPauseEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountPauseEvent);
                    DeviceManager.CountEndEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_CountEndEvent);
                    DeviceManager.TransactionStatusEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionStatusEvent);
                    DeviceManager.TransactionEndEvent += new EventHandler<DeviceTransactionResult>(DeviceManager_TransactionEndEvent);
                    DeviceManager.CITResultEvent += new EventHandler<CITResult>(DeviceManager_CITResultEvent);
                    DeviceManager.BagClosedEvent += new EventHandler<EventArgs>(DeviceManager_BagClosedEvent);
                    DeviceManager.BagOpenedEvent += new EventHandler<EventArgs>(DeviceManager_BagOpenedEvent);
                    DeviceManager.BagRemovedEvent += new EventHandler<EventArgs>(DeviceManager_BagRemovedEvent);
                    DeviceManager.BagPresentEvent += new EventHandler<EventArgs>(DeviceManager_BagPresentEvent);
                    DeviceManager.DoorClosedEvent += new EventHandler<EventArgs>(DeviceManager_DoorClosedEvent);
                    DeviceManager.DoorOpenEvent += new EventHandler<EventArgs>(DeviceManager_DoorOpenEvent);
                    DeviceManager.BagFullAlertEvent += new EventHandler<ControllerStatus>(DeviceManager_BagFullAlertEvent);
                    DeviceManager.BagFullWarningEvent += new EventHandler<ControllerStatus>(DeviceManager_BagFullWarningEvent);
                    DeviceManager.DeviceLockedEvent += new EventHandler<EventArgs>(DeviceManager_DeviceLockedEvent);
                    DeviceManager.DeviceUnlockedEvent += new EventHandler<EventArgs>(DeviceManager_DeviceUnlockedEvent);
                    DeviceManager.EscrowJamStartEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamStartEvent);
                    DeviceManager.EscrowJamClearWaitEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamClearWaitEvent);
                    DeviceManager.EscrowJamEndRequestEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndRequestEvent);
                    DeviceManager.EscrowJamEndEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndEvent);
                }
                DeviceManager.Initialise();
                EscrowJam escrowJam = _escrowJamRepository.GetFirst().ContinueWith(r => r.Result).Result;
                if (escrowJam == null || escrowJam.RecoveryDate.HasValue || DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
                    return;
                DeviceManager_EscrowJamStartEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Failed to initiaise depositor: {0}", ex.MessageString());
                OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_APPLICATION_STARTUP_FAILED, ex.MessageString());
            }

        }

        private void DeviceManager_EscrowJamStartEvent(object sender, EventArgs e)
        {
            Log?.Error(GetType().Name, 107, "DeviceManager_StorageJamEvent", "Escrow Jam Detected");
            if (CurrentSession != null)
                CurrentSession.CountingEnded = true;
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.Transaction != null)
                {
                    CurrentTransaction.Transaction.EscrowJam = true;
                    EscrowJam = new EscrowJam()
                    {
                        Id = Guid.NewGuid(),
                        DateDetected = DateTime.Now,
                        DroppedAmount = CurrentTransaction.DroppedAmountCents,
                        EscrowAmount = CurrentTransaction.CountedAmountCents
                    };
                    CurrentTransaction.Transaction.EscrowJams.Add(EscrowJam);
                    CurrentTransaction.DroppedAmountCents = CurrentTransaction.TotalAmountCents;
                    CurrentTransaction.DroppedDenomination += CurrentTransaction.CountedDenomination;
                    ///ApplicationViewModel.SaveToDatabaseAsync();
                }
                AlertManager.SendAlert(new AlertEscrowJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                CurrentTransaction.NoteJamDetected = true;
                EndTransaction(ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM, ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM.ToString() + ": Escrow Jam detected. DO NOT Post until after CIT");
            }
            if (sender == this && DeviceManager.DeviceManagerMode != DeviceManagerMode.ESCROW_JAM)
                DeviceManager.OnEscrowJamStartEvent(this, EventArgs.Empty);
            else if (EscrowJam == null)
            {
                EscrowJam escrowJam = _escrowJamRepository.GetFirst().ContinueWith(r => r.Result).Result;
                if (escrowJam != null && !escrowJam.RecoveryDate.HasValue)
                    EscrowJam = escrowJam;
            }
            ShowErrorDialog(new OutOfOrderScreenViewModel(this));
        }

        private void DeviceManager_EscrowJamClearWaitEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_EscrowJamEndRequestEvent(object sender, EventArgs e)
        {
        }

        private void DeviceManager_EscrowJamEndEvent(object sender, EventArgs e)
        {
            ResetDevice();
            InitialiseApp();
            CloseDialog();
        }

        private void InitialiseScreenList()
        {
            GUIScreens = new List<GUIScreen>();
            GUIScreens.AddRange(ApplicationModel.dbGUIScreens);
        }

        private void InitialiseLanguageList() => ApplicationModel.GenerateLanguageList();

        private void InitialiseCurrencyList() => ApplicationModel.GenerateCurrencyList();

        private void InitialiseTransactionTypeList() => ApplicationModel.GenerateTransactionTypeList();

        private void HandleIncompleteTransaction()
        {
            Log.Debug(GetType().Name, nameof(HandleIncompleteTransaction), "Transaction", "Checking for and maintaining any incomplete transaction");
            var transactions = _transactionRepository.GetCompleted().ContinueWith(r => r.Result).Result;
            foreach (Transaction transaction in transactions)
            {
                transaction.TxCompleted = true;
                transaction.TxEndDate = new DateTime?(DateTime.Now);
                transaction.TxErrorCode = 85;
                transaction.TxErrorMessage = "Incomplete transaction aborted";
                transaction.TxResult = 85;
            }


        }

        public async void HandleIncompleteSession()
        {

            HandleIncompleteTransaction();
            var device = ApplicationModel.GetDeviceAsync();
            var depositorSessions = await _depositorSessionRepository.GetCompleted();
            foreach (DepositorSession depositorSession in depositorSessions)
            {
                depositorSession.SessionEnd = new DateTime?(DateTime.Now);
                depositorSession.Complete = true;
                depositorSession.CompleteSuccess = false;
                depositorSession.ErrorCode = new int?(84);
                depositorSession.ErrorMessage = "Session is incomplete";
                var updated = await _depositorSessionRepository.UpdateAsync(depositorSession);
            }


        }

        private void StartSession()
        {
            _deviceManager.SetCurrency(ApplicationModel.GetDeviceAsync().CurrencyListNavigation.DefaultCurrency.ToUpper());
            CurrentSession = new AppSession(this);
            CurrentSession.TransactionLimitReachedEvent += new EventHandler<EventArgs>(CurrentSession_TransactionLimitReachedEvent);
            CurrentScreenIndex = 1;
            ShowScreen();
            CurrentApplicationState = ApplicationState.IDLE;

        }

        public void EndSession()
        {
            if (CurrentSession == null)
                return;
            EndSession(true, 0, ApplicationErrorConst.ERROR_NONE);
        }

        public void EndSession(
          bool success,
          int errorcode,
          ApplicationErrorConst transactionError,
          string errormessage = "")
        {
            Log.InfoFormat(GetType().Name, nameof(EndSession), "Session", "Result = {0}", transactionError.ToString());
            if (CurrentTransaction != null)
                EndTransaction(transactionError, errormessage);
            CurrentSession?.EndSession(success, errorcode, transactionError, errormessage);
            CurrentSession = null;
            InitialiseApp();
        }

        public int CurrentScreenIndex { get; set; }

        public List<GUIScreen> GUIScreens { get; set; }

        public GUIScreen CurrentGUIScreen => GUIScreens.ElementAtOrDefault(CurrentScreenIndex) ?? null;

        public object CurrentScreen { get; set; }

        public Guid? SessionID => CurrentSession?.SessionID;

        public async void ShowScreen(int screenIndex, bool generateNewScreen = true)
        {
            try
            {
                if (screenIndex == 0)
                    InitialiseScreenList();
                if (ApplicationStatus.CashmereDeviceState == CashmereDeviceState.NONE || debugNoDevice)
                {
                    if (generateNewScreen)
                    {
                        CurrentScreenIndex = screenIndex < 0 ? 0 : screenIndex;
                        CurrentScreenIndex = screenIndex > GUIScreens.Count - 1 ? 0 : screenIndex;
                        // _depositorDBContext.GuiScreens.Attach(GUIScreens[CurrentScreenIndex]);
                        var GUIScreenTypeCode = GUIScreens[CurrentScreenIndex].GUIScreenType.Code;
                        TypeInfo typeInfo = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.GUID == GUIScreenTypeCode);
                        GUIScreen guiScreen = GUIScreens[CurrentScreenIndex];
                        var guiScreenListScreen = await _guiScreenListScreenRepository.GetByGUIScreenId(guiScreen.Id);
                        var currentGuiScreen = await _gUIScreenRepository.GetByIdAsync(CurrentGUIScreen.Id);
                        string str = CashmereTranslationService.TranslateUserText("ShowScreen().screenTitle", currentGuiScreen.GuiTextNavigation?.ScreenTitle, "[Translation Error]");
                        object instance = Activator.CreateInstance(typeInfo, str, this, guiScreenListScreen.Required);
                        this.ActivateItemAsync(instance);
                        Log.InfoFormat(GetType().Name, nameof(ShowScreen), "Navigation", "Showing screen: {0}", GUIScreens[CurrentScreenIndex]?.Name);
                        if (CurrentScreen is DepositorCustomerScreenBaseViewModel)
                        {
                            if (CurrentScreen is DepositorCustomerScreenBaseViewModel currentScreen)
                                currentScreen.Dispose();
                            CurrentScreen = null;
                        }
                        CurrentScreen = instance;
                    }
                    else
                        this.ActivateItemAsync(CurrentScreen);
                }
                else
                    ShowErrorDialog(new OutOfOrderScreenViewModel(this));

            }
            catch (Exception ex)
            {
                Log.ErrorFormat(nameof(ShowScreen), 104, ApplicationErrorConst.ERROR_SCREEN_RENDER_FAILED.ToString(), "Error displaying screen {0}: {1}>>{2}>>{3}", screenIndex.ToString() ?? "", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                throw;
            }
        }

        public void ShowScreen() => ShowScreen(CurrentScreenIndex);

        public void ShowScreen(bool genScreen) => ShowScreen(CurrentScreenIndex, genScreen);

        public void NavigateNextScreen()
        {
            if (CurrentScreenIndex + 1 >= GUIScreens.Count)
                return;
            ShowScreen(++CurrentScreenIndex);
        }

        public void NavigatePreviousScreen()
        {
            if (CurrentScreenIndex - 1 < 0)
                return;
            ShowScreen(--CurrentScreenIndex);
        }

        public void NavigateFirstScreen()
        {
            CurrentScreenIndex = 0;
            ShowScreen(CurrentScreenIndex);
        }

        public void NavigateLastScreen()
        {
            CurrentScreenIndex = GUIScreens.Count - 1;
            ShowScreen(CurrentScreenIndex);
        }

        public void ShowDialog(object screen)
        {
            if (!AdminMode && ApplicationStatus.CashmereDeviceState != CashmereDeviceState.NONE)
                return;
            this.ActivateItemAsync(screen);
        }

        public void ShowDialogBox(object screen) => this.ActivateItemAsync(screen);

        public void ShowErrorDialog(object screen)
        {
            Log.Info(GetType().Name, "ShowDialogScreen", "Screen", screen.GetType().Name);
            if (ApplicationStatus.CashmereDeviceState == CashmereDeviceState.NONE)
                return;
            this.ActivateItemAsync(screen);
        }

        public void CloseDialog(bool generateScreen = true)
        {
            if (generateScreen)
            {
                Log.Info(GetType().Name, nameof(CloseDialog), "Screen", "Closing dialog screen");
                ShowScreen();
            }
            else
                ShowScreen(false);
        }

        public bool? CanCancelTransaction => throw new NotImplementedException();

        internal void CancelSessionOnUserInput()
        {
            Log.Info(GetType().Name, "cancelSession", "Session", "User has cancelled the session");
            string message = CashmereTranslationService?.TranslateSystemText("CancelSessionOnUserInput.message", "sys_Dialog_CancelTransaction_MessageText", "Would you like to cancel your current transaction?" + Environment.NewLine + Environment.NewLine + "To delete text, please use the \"Delete\" or \"Backspace\" buttons");
            string str = CashmereTranslationService?.TranslateSystemText("CancelSessionOnUserInput.message", "sys_Dialog_CancelTransaction_TitleCaption", "Cancel Transaction");
            DeviceConfiguration deviceConfiguration = DeviceConfiguration;
            int timeout = deviceConfiguration != null ? deviceConfiguration.USER_SCREEN_TIMEOUT : 15;
            string title = str;
            if (TimeoutDialogBox.ShowDialog(message, timeout, title, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                AlertManager.SendAlert(new AlertTransactionCancelled(Log, CurrentTransaction, CurrentSession.Device, DateTime.Now));
            }
            catch (Exception ex)
            {
                Log.ErrorFormat(GetType().Name, 100, ApplicationErrorConst.ERROR_ALERT_SEND_FAILED.ToString(), "{0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
            }
            ResetDevice();
            EndSession(false, 1011, ApplicationErrorConst.WARN_DEPOSIT_CANCELED, "User has cancelled the session");
        }

        public void SplashScreen_Clicked()
        {
            Log.Info(GetType().Name, "splashScreen_Clicked", "Screen", "The splash screen has been clicked");
            HandleIncompleteSession();
            StartSession();
        }

        public void TermsAccepted(bool accepted = true)
        {
            CurrentSession.TermsAccepted = accepted;
            if (accepted)
            {
                Log.Info(GetType().Name, "termsAccepted", "User Input", "Terms Accepted");
                CurrentSession.TermsAccepted = true;
                NavigateNextScreen();
            }
            else
            {
                Log.Info(GetType().Name, "termsAccepted", "User Input", "Terms Rejected");
                EndSession(false, 20, ApplicationErrorConst.WARN_TERMS_REJECT_BY_USER, "Customer Rejected Terms and Conditions");
            }
        }

        public void SetCurrency(Currency value)
        {
            Log.Info(GetType().Name, nameof(SetCurrency), "User Input", value.Code.ToUpper());
            CurrentTransaction.Currency = value;
            if (debugNoDevice)
                return;
            DeviceManager.SetCurrency(CurrentTransaction.Currency.Code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.Code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.Code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.Code.ToUpper());
            DeviceManager.SetCurrency(CurrentTransaction.Currency.Code.ToUpper());
        }

        internal void CreateTransaction(TransactionTypeListItem value)
        {
            //_depositorDBContext.TransactionTypeListItems.Attach(value);
            Log.Info(GetType().Name, "SetTransactionType", "User Input", value.Name);
            GUIScreens.AddRange(ApplicationModel.GetTransactionTypeScreenList(value).ToList());
            if (CurrentSession.Transaction == null)
                CurrentSession.CreateTransaction(value);
            CurrentTransaction.TransactionType = value;

        }

        internal async Task<AccountsListResponse> SearchAccountListAsync(
          string searchText,
          TransactionTypeListItem txType,
          string Currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountsListResponse1;
            Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.Name);
            AccountsListResponse response = new();
            Guid guid = Guid.NewGuid();
            string str1 = guid.ToString();
            if (applicationViewModel.debugNoCoreBanking)
            {
                AccountsListResponse accountsListResponse2 = new()
                {
                    RequestID = str1,
                    MessageDateTime = DateTime.Now,
                    ServerErrorCode = "0",
                    IsSuccess = true
                };
                guid = Guid.NewGuid();
                accountsListResponse2.MessageID = guid.ToString();
                accountsListResponse2.Accounts = new List<Account>()
          {
            new Account()
            {
              AccountNumber = "1234567890",
              AccountName = "Account1"
            },
            new Account()
            {
              AccountNumber = "1234567891",
              AccountName = "Account2"
            },
            new Account()
            {
              AccountNumber = "1234567892",
              AccountName = "Account3"
            },
            new Account()
            {
              AccountNumber = "1234567893",
              AccountName = "Account4"
            },
            new Account()
            {
              AccountNumber = "1234567894",
              AccountName = "Account5"
            },
            new Account()
            {
              AccountNumber = "1234567895",
              AccountName = "Account6"
            },
            new Account()
            {
              AccountNumber = "1234567896",
              AccountName = "Account7"
            },
            new Account()
            {
              AccountNumber = "1234567897",
              AccountName = "Account8"
            },
            new Account()
            {
              AccountNumber = "1234567898",
              AccountName = "Account9"
            },
            new Account()
            {
              AccountNumber = "1234567899",
              AccountName = "Account10"
            },
            new Account()
            {
              AccountNumber = "1122334455",
              AccountName = "Account11"
            },
            new Account()
            {
              AccountNumber = "1112334567",
              AccountName = "Account12"
            }
          };
                response = accountsListResponse2;
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.Name, Currency);
                    Device device = ApplicationViewModel.GetDeviceAsync();
                    DateTime now = DateTime.Now;
                    GUIControlServiceClient controlServiceClient = new(DeviceConfiguration.API_CDM_GUI_URI, device.AppId, device.AppKey, null);
                    AccountsListRequest accountsListRequest = new()
                    {
                        Currency = applicationViewModel.CurrentTransaction?.CurrencyCode,
                        Language = applicationViewModel.CurrentLanguage
                    };
                    AppSession currentSession = applicationViewModel.CurrentSession;
                    string str2;
                    if (currentSession == null)
                    {
                        str2 = null;
                    }
                    else
                    {
                        guid = currentSession.SessionID;
                        str2 = guid.ToString();
                    }
                    accountsListRequest.SessionID = str2;
                    guid = Guid.NewGuid();
                    accountsListRequest.MessageID = guid.ToString();
                    accountsListRequest.AppName = device.MachineName;
                    accountsListRequest.MessageDateTime = DateTime.Now;
                    accountsListRequest.TransactionType = applicationViewModel.CurrentTransaction.TransactionType.Id;
                    accountsListRequest.AppID = device.AppId;
                    accountsListRequest.DeviceID = device.Id;
                    accountsListRequest.PageNumber = PageNumber;
                    accountsListRequest.PageSize = PageSize;
                    accountsListRequest.SearchText = searchText;
                    AccountsListRequest request = accountsListRequest;
                    response = await controlServiceClient.SearchAccountAsync(request);
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    if (response.IsSuccess)
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Response", "{0}", response.ToString());
                        if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                            response.Accounts.RemoveAll(p => !p.Currency.Equals(Currency));
                    }
                    if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                    if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.SetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                    response.IsSuccess = false;
                }
            }
            accountsListResponse1 = response;

            return accountsListResponse1;
        }

        internal async Task<AccountsListResponse> GetAccountListAsync(
          TransactionTypeListItem txType,
          string Currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountListAsync;
            Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.Name);
            AccountsListResponse response = new();
            Guid guid = Guid.NewGuid();
            string str1 = guid.ToString();
            if (applicationViewModel.debugNoCoreBanking)
            {
                AccountsListResponse accountsListResponse = new()
                {
                    RequestID = str1,
                    MessageDateTime = DateTime.Now,
                    ServerErrorCode = "0",
                    IsSuccess = true
                };
                guid = Guid.NewGuid();
                accountsListResponse.MessageID = guid.ToString();
                accountsListResponse.Accounts = new List<Account>()
          {
            new Account()
            {
              AccountNumber = "1234567890",
              AccountName = "Account1"
            },
            new Account()
            {
              AccountNumber = "1234567891",
              AccountName = "Account2"
            },
            new Account()
            {
              AccountNumber = "1234567892",
              AccountName = "Account3"
            },
            new Account()
            {
              AccountNumber = "1234567893",
              AccountName = "Account4"
            },
            new Account()
            {
              AccountNumber = "1234567894",
              AccountName = "Account5"
            },
            new Account()
            {
              AccountNumber = "1234567895",
              AccountName = "Account6"
            },
            new Account()
            {
              AccountNumber = "1234567896",
              AccountName = "Account7"
            },
            new Account()
            {
              AccountNumber = "1234567897",
              AccountName = "Account8"
            },
            new Account()
            {
              AccountNumber = "1234567898",
              AccountName = "Account9"
            },
            new Account()
            {
              AccountNumber = "1234567899",
              AccountName = "Account10"
            },
            new Account()
            {
              AccountNumber = "1122334455",
              AccountName = "Account11"
            },
            new Account()
            {
              AccountNumber = "1112334567",
              AccountName = "Account12"
            }
          };
                response = accountsListResponse;
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.Name, Currency);
                    Device device = ApplicationViewModel.GetDeviceAsync();
                    DateTime now = DateTime.Now;
                    GUIControlServiceClient controlServiceClient = new(DeviceConfiguration.API_CDM_GUI_URI, device.AppId, device.AppKey, null);
                    AccountsListRequest accountsListRequest = new()
                    {
                        Currency = applicationViewModel.CurrentTransaction?.CurrencyCode,
                        Language = applicationViewModel.CurrentLanguage
                    };
                    AppSession currentSession = applicationViewModel.CurrentSession;
                    string str2;
                    if (currentSession == null)
                    {
                        str2 = null;
                    }
                    else
                    {
                        guid = currentSession.SessionID;
                        str2 = guid.ToString();
                    }
                    accountsListRequest.SessionID = str2;
                    guid = Guid.NewGuid();
                    accountsListRequest.MessageID = guid.ToString();
                    accountsListRequest.AppName = device.MachineName;
                    accountsListRequest.MessageDateTime = DateTime.Now;
                    accountsListRequest.TransactionType = applicationViewModel.CurrentTransaction.TransactionType.Id;
                    accountsListRequest.AppID = device.AppId;
                    accountsListRequest.DeviceID = device.Id;
                    accountsListRequest.PageNumber = PageNumber;
                    accountsListRequest.PageSize = PageSize;
                    AccountsListRequest request = accountsListRequest;
                    response = await controlServiceClient.GetAccountsListAsync(request);
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    if (response.IsSuccess)
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Response", "{0}", response.ToString());
                        if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                            response.Accounts.RemoveAll(p => !p.Currency.Equals(Currency));
                    }
                    if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                    if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.SetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                    response.IsSuccess = false;
                }
            }
            accountListAsync = response;

            return accountListAsync;
        }

        internal async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          string accountNumber,
          string Currency,
          int txType)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountNumberValidationResponse validationResponse1;
            Log.Info(applicationViewModel.GetType().Name, "ValidateAccountNumber", "User Input", accountNumber);
            AccountNumberValidationResponse response = new();
            if (applicationViewModel.debugNoCoreBanking)
            {
                if (accountNumber == "1234")
                {
                    AccountNumberValidationResponse validationResponse2 = new()
                    {
                        AccountName = "",
                        CanTransact = false,
                        MessageDateTime = DateTime.Now
                    };
                    Guid guid = Guid.NewGuid();
                    validationResponse2.RequestID = guid.ToString().ToUpper();
                    guid = Guid.NewGuid();
                    validationResponse2.MessageID = guid.ToString().ToUpper();
                    validationResponse2.IsSuccess = false;
                    validationResponse2.PublicErrorCode = 400.ToString() ?? "";
                    validationResponse2.PublicErrorMessage = "Account Does Not Exist";
                    response = validationResponse2;
                }
                else
                {
                    AccountNumberValidationResponse validationResponse3 = new()
                    {
                        AccountName = "Test Account",
                        CanTransact = true,
                        MessageDateTime = DateTime.Now
                    };
                    Guid guid = Guid.NewGuid();
                    validationResponse3.RequestID = guid.ToString().ToUpper();
                    guid = Guid.NewGuid();
                    validationResponse3.MessageID = guid.ToString().ToUpper();
                    validationResponse3.IsSuccess = true;
                    validationResponse3.PublicErrorCode = 200.ToString() ?? "";
                    validationResponse3.PublicErrorMessage = "Validated Successfully";
                    response = validationResponse3;
                }
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Request", "Account = {0} Currency = {1}", accountNumber, Currency);
                    Device device = ApplicationViewModel.GetDeviceAsync();
                    IntegrationServiceClient integrationServiceClient = new(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                    AccountNumberValidationRequest request = new()
                    {
                        AccountNumber = accountNumber,
                        AppID = device.AppId,
                        AppName = device.MachineName,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        SessionID = applicationViewModel.SessionID.Value.ToString(),
                        DeviceID = device.Id,
                        Currency = Currency,
                        Language = applicationViewModel.CurrentLanguage,
                        TransactionType = txType
                    };
                    // ISSUE: explicit non-virtual call
                    response = await integrationServiceClient.ValidateAccountNumberAsync(request);
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    if (response.IsSuccess)
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Response", "AccountName = {0} ErrorCode = {1}, ErrorMessage = {2}", response.AccountName, response.ServerErrorCode, response.ServerErrorMessage);
                        if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                        {
                            switch (response?.AccountCurrency)
                            {
                                case null:
                                    break;
                                default:
                                    if (response?.AccountCurrency?.ToUpper() != Currency.ToUpper())
                                    {
                                        Log.InfoFormat(applicationViewModel.GetType().Name, "Transaction", "Cross Currency Not Allowed", "Cannot deposit {2} into {1} Account {0}.", accountNumber, response.AccountCurrency, Currency);
                                        response.IsSuccess = false;
                                        response.PublicErrorMessage = string.Format("account indicated is a {0} account kindly enter the correct Currency account", response.AccountCurrency);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                    if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.SetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                    response.IsSuccess = false;
                    response.PublicErrorMessage = "Validation Error Occurred, please contact an administrator.";
                }
            }
            Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Result", "Result = {0} AccountName = {1} CanTransact={2} Error = {3}", response.IsSuccess, response.AccountName, response.CanTransact, response?.ServerErrorMessage);
            validationResponse1 = response;

            return validationResponse1;
        }

        internal async Task<AccountNumberValidationResponse> _FinacleValidateAccountNumberAsync(
        string accountNumber,
        string Currency,
        int txType)
        {
            var applicationViewModel = this;
            AccountNumberValidationResponse validationResponse1;
            Log.Info(applicationViewModel.GetType().Name, "ValidateAccountNumber", "User Input", accountNumber);
            var response = new AccountNumberValidationResponse();
            if (applicationViewModel.debugNoCoreBanking)
            {
                if (accountNumber == "1234")
                {
                    var validationResponse2 = new AccountNumberValidationResponse
                    {
                        AccountName = "",
                        CanTransact = false,
                        MessageDateTime = DateTime.Now
                    };
                    var guid = Guid.NewGuid();
                    validationResponse2.RequestID = guid.ToString().ToUpper();
                    guid = Guid.NewGuid();
                    validationResponse2.MessageID = guid.ToString().ToUpper();
                    validationResponse2.IsSuccess = false;
                    validationResponse2.PublicErrorCode = 400.ToString() ?? "";
                    validationResponse2.PublicErrorMessage = "Account Does Not Exist";
                    response = validationResponse2;
                }
                else
                {
                    var validationResponse3 = new AccountNumberValidationResponse
                    {
                        AccountName = "Test Account",
                        CanTransact = true,
                        MessageDateTime = DateTime.Now
                    };
                    var guid = Guid.NewGuid();
                    validationResponse3.RequestID = guid.ToString().ToUpper();
                    guid = Guid.NewGuid();
                    validationResponse3.MessageID = guid.ToString().ToUpper();
                    validationResponse3.IsSuccess = true;
                    validationResponse3.PublicErrorCode = 200.ToString() ?? "";
                    validationResponse3.PublicErrorMessage = "Validated Successfully";
                    response = validationResponse3;
                }
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Request", "Account = {0} Currency = {1}", accountNumber, Currency);
                    var device = GetDeviceAsync();
                    var integrationServiceClient = new FinacleIntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                    var request = new AccountNumberValidationRequest
                    {
                        AccountNumber = accountNumber,
                        AppID = device.AppId,
                        AppName = device.MachineName,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        SessionID = applicationViewModel.SessionID.Value.ToString(),
                        DeviceID = device.Id,
                        Currency = Currency,
                        Language = applicationViewModel.CurrentLanguage,
                        TransactionType = txType
                    };
                    // ISSUE: explicit non-virtual call
                    response = await (integrationServiceClient.ValidateAccountNumberAsync(request));
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                    if (response.IsSuccess)
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Response", "AccountName = {0} ErrorCode = {1}, ErrorMessage = {2}", response.AccountName, response.ServerErrorCode, response.ServerErrorMessage);
                        if (!DeviceConfiguration.ALLOW_CROSS_CURRENCY_TX)
                        {
                            switch (response?.AccountCurrency)
                            {
                                case null:
                                    break;
                                default:
                                    if (response?.AccountCurrency?.ToUpper() != Currency.ToUpper())
                                    {
                                        Log.InfoFormat(applicationViewModel.GetType().Name, "Transaction", "Cross Currency Not Allowed", "Cannot deposit {2} into {1} Account {0}.", accountNumber, response.AccountCurrency, Currency);
                                        response.IsSuccess = false;
                                        response.PublicErrorMessage = string.Format("account indicated is a {0} account kindly enter the correct Currency account", response.AccountCurrency);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    if (applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.UnSetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Account Validation Exception: {0}", ex.MessageString());
                    if (!applicationViewModel.ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SERVER_CONNECTION))
                        applicationViewModel.SetCashmereDeviceState(CashmereDeviceState.SERVER_CONNECTION);
                    response.IsSuccess = false;
                    response.PublicErrorMessage = "Validation Error Occurred, please contact an administrator.";
                }
            }
            Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Result", "Result = {0} AccountName = {1} CanTransact={2} Error = {3}", response.IsSuccess, response.AccountName, response.CanTransact, response?.ServerErrorMessage);
            validationResponse1 = response;
            return validationResponse1;
        }
        internal async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          string accountNumber,
          string refAccountNumber,
          string transactionType)
        {
            ApplicationViewModel applicationViewModel = this;
            ReferenceAccountNumberValidationResponse validationResponse1;

            ReferenceAccountNumberValidationResponse response = new();
            if (applicationViewModel.debugNoCoreBanking)
            {
                if (refAccountNumber == "1234")
                {
                    ReferenceAccountNumberValidationResponse validationResponse2 = new()
                    {
                        AccountName = "",
                        CanTransact = false,
                        MessageDateTime = DateTime.Now,
                        RequestID = Guid.NewGuid().ToString().ToUpper(),
                        MessageID = Guid.NewGuid().ToString().ToUpper(),
                        IsSuccess = false,
                        PublicErrorCode = 400.ToString() ?? "",
                        PublicErrorMessage = "Account Does Not Exist"
                    };
                    response = validationResponse2;
                }
                else
                {
                    ReferenceAccountNumberValidationResponse validationResponse3 = new()
                    {
                        AccountName = "Test Account",
                        CanTransact = true,
                        MessageDateTime = DateTime.Now,
                        RequestID = Guid.NewGuid().ToString().ToUpper(),
                        MessageID = Guid.NewGuid().ToString().ToUpper(),
                        IsSuccess = true,
                        PublicErrorCode = 200.ToString() ?? "",
                        PublicErrorMessage = "Validated Successfully"
                    };
                    response = validationResponse3;
                }
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateReferenceAccountNumber", "Validation Request", "Account = {0} Type = {1}", refAccountNumber, applicationViewModel.CurrentTransaction.TransactionType.CbTxType);
                    Device device = applicationViewModel.CurrentSession.Device;
                    IntegrationServiceClient integrationServiceClient = new(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                    ReferenceAccountNumberValidationRequest request = new()
                    {
                        AccountNumber = accountNumber,
                        ReferenceAccountNumber = refAccountNumber,
                        AppID = device.AppId,
                        AppName = device.MachineName,
                        DeviceID = device.Id,
                        MessageDateTime = DateTime.Now,
                        MessageID = Guid.NewGuid().ToString(),
                        SessionID = applicationViewModel.SessionID.Value.ToString(),
                        Currency = applicationViewModel.CurrentTransaction.CurrencyCode,
                        Language = applicationViewModel.CurrentLanguage,
                        TransactionType = applicationViewModel.CurrentTransaction.TransactionType.Id
                    };
                    // ISSUE: explicit non-virtual call
                    response = await integrationServiceClient.ValidateReferenceAccountNumberAsync(request);
                    applicationViewModel.CheckIntegrationResponseMessageDateTime(response.MessageDateTime);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat(applicationViewModel.GetType().Name, 3, ApplicationErrorConst.ERROR_SYSTEM.ToString(), "Ref Account Validation Exception: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                    response.IsSuccess = false;
                    response.PublicErrorMessage = "Validation Error Occurred, please contact an administrator.";
                }
            }
            Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateReferenceAccountNumber", "ReferenceValidation Result", "Result = {0} AccountName = {1} CanTransact={2}", response.IsSuccess, response.AccountName, response.CanTransact);
            validationResponse1 = response;

            return validationResponse1;
        }

        internal void ReferencesAccepted(bool success = true) => Log.Info(GetType().Name, "VerifyReferences", nameof(ReferencesAccepted), "User accepted the references");

        private void DeviceManager_DeviceLockedEvent(object sender, EventArgs e)
        {
            SetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);
            Log.Debug(GetType().Name, "Device", "Device State Changed", "Setting CashmereDeviceState.DEVICE_LOCK");
        }

        private void DeviceManager_DeviceUnlockedEvent(object sender, EventArgs e)
        {
            if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_LOCK))
                return;
            UnSetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);
        }

        private async void DeviceManager_StatusReportEvent(object sender, DeviceStatusChangedEventArgs e)
        {

            Device device = ApplicationModel.GetDeviceAsync();
            if (device != null)
            {
                DeviceStatus deviceStatus = await _deviceStatusRepository.GetByDeviceId(device.Id);
                if (deviceStatus == null)
                {
                    deviceStatus = await _deviceStatusRepository.AddAsync(new DeviceStatus()
                    {
                        DeviceId = device.Id,
                        MachineName = Environment.MachineName.ToUpperInvariant(),
                        TransactionStatus = e.ControllerStatus.Transaction?.Status.ToString(),
                        TransactionType = e.ControllerStatus.Transaction?.Type.ToString(),
                        BagNoteCapacity = e.ControllerStatus.Bag.NoteCapacity.ToString() ?? "",
                        BagNoteLevel = e.ControllerStatus.Bag.NoteLevel,
                        BagNumber = e.ControllerStatus.Bag.BagNumber,
                        BagPercentFull = e.ControllerStatus.Bag.PercentFull,
                        BagStatus = e.ControllerStatus.Bag.BagState.ToString() ?? "",
                        BagValueCapacity = e.ControllerStatus.Bag.ValueCapacity,
                        BagValueLevel = e.ControllerStatus.Bag.ValueLevel,
                        BaCurrency = e.ControllerStatus.NoteAcceptor.Currency,
                        BaStatus = e.ControllerStatus.NoteAcceptor.Status.ToString(),
                        BaType = e.ControllerStatus.NoteAcceptor.Type.ToString(),
                        ControllerState = e.ControllerStatus.ControllerState.ToString(),
                        CurrentStatus = (int)ApplicationStatus.CashmereDeviceState,
                        EscrowPosition = e.ControllerStatus.Escrow.Position.ToString(),
                        EscrowStatus = e.ControllerStatus.Escrow.Status.ToString(),
                        EscrowType = e.ControllerStatus.Escrow.Type.ToString(),
                        Id = Guid.NewGuid(),
                        Modified = DateTime.Now,
                        SensorsStatus = e.ControllerStatus.Sensor.Status.ToString(),
                        SensorsType = e.ControllerStatus.Sensor.Type.ToString(),
                        SensorsValue = e.ControllerStatus.Sensor.Value,
                        SensorsBag = e.ControllerStatus.Sensor.Bag.ToString(),
                        SensorsDoor = e.ControllerStatus.Sensor.Door.ToString()
                    });
                }
                else
                {
                    deviceStatus.DeviceId = device.Id;
                    deviceStatus.MachineName = Environment.MachineName.ToUpperInvariant();
                    deviceStatus.TransactionStatus = e.ControllerStatus.Transaction?.Status.ToString();
                    deviceStatus.TransactionType = e.ControllerStatus.Transaction?.Type.ToString();
                    deviceStatus.BagNoteCapacity = e.ControllerStatus.Bag.NoteCapacity.ToString() ?? "";
                    deviceStatus.BagNoteLevel = e.ControllerStatus.Bag.NoteLevel;
                    deviceStatus.BagNumber = e.ControllerStatus.Bag.BagNumber;
                    deviceStatus.BagPercentFull = e.ControllerStatus.Bag.PercentFull;
                    deviceStatus.BagStatus = e.ControllerStatus.Bag.BagState.ToString() ?? "";
                    deviceStatus.BagValueCapacity = e.ControllerStatus.Bag.ValueCapacity;
                    deviceStatus.BagValueLevel = e.ControllerStatus.Bag.ValueLevel;
                    deviceStatus.BaCurrency = e.ControllerStatus.NoteAcceptor.Currency;
                    deviceStatus.BaStatus = e.ControllerStatus.NoteAcceptor.Status.ToString();
                    deviceStatus.BaType = e.ControllerStatus.NoteAcceptor.Type.ToString();
                    deviceStatus.ControllerState = e.ControllerStatus.ControllerState.ToString();
                    deviceStatus.CurrentStatus = (int)ApplicationStatus.CashmereDeviceState;
                    deviceStatus.EscrowPosition = e.ControllerStatus.Escrow.Position.ToString();
                    deviceStatus.EscrowStatus = e.ControllerStatus.Escrow.Status.ToString();
                    deviceStatus.EscrowType = e.ControllerStatus.Escrow.Type.ToString();
                    deviceStatus.Modified = DateTime.Now;
                    deviceStatus.SensorsStatus = e.ControllerStatus.Sensor.Status.ToString();
                    deviceStatus.SensorsType = e.ControllerStatus.Sensor.Type.ToString();
                    deviceStatus.SensorsValue = e.ControllerStatus.Sensor.Value;
                    deviceStatus.SensorsBag = e.ControllerStatus.Sensor.Bag.ToString();
                    deviceStatus.SensorsDoor = e.ControllerStatus.Sensor.Door.ToString();
                    var reslt = await _deviceStatusRepository.UpdateAsync(deviceStatus);
                }
            }
            if (e.ControllerStatus.Sensor.Door == DeviceSensorDoor.OPEN)
            {
                if (debugDisableSafeSensor)
                    UnSetCashmereDeviceState(CashmereDeviceState.SAFE);
                else if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SAFE))
                    SetCashmereDeviceState(CashmereDeviceState.SAFE);
            }
            else if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.SAFE))
                UnSetCashmereDeviceState(CashmereDeviceState.SAFE);
            if ((e.ControllerStatus.Bag.BagState == BagState.OK || e.ControllerStatus.Bag.BagState == BagState.CAPACITY) && e.ControllerStatus.Sensor.Bag != DeviceSensorBag.REMOVED)
            {
                if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.BAG))
                    UnSetCashmereDeviceState(CashmereDeviceState.BAG);
            }
            else
            {
                if (debugDisableBagSensor)
                {
                    UnSetCashmereDeviceState(CashmereDeviceState.BAG);
                }
                else
                {
                    AppSession currentSession = CurrentSession;
                    bool flag = currentSession == null || !currentSession.CountingStarted;
                    if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.BAG) & flag)
                        SetCashmereDeviceState(CashmereDeviceState.BAG);
                }
                if ((CurrentApplicationState == ApplicationState.SPLASH || CurrentApplicationState == ApplicationState.STARTUP) && e.ControllerStatus.Bag.BagState == BagState.CLOSED)
                {
                    Log.ErrorFormat(GetType().Name, 95, ApplicationErrorConst.ERROR_INCOMPLETE_CIT.ToString(), "Bag is closed on during {0}, finalising incomplete CIT", CurrentApplicationState.ToString());
                    CurrentApplicationState = ApplicationState.CIT_BAG_CLOSED;
                }
            }
            if ((e != null ? (e.DeviceManagerState != DeviceManagerState.OUT_OF_ORDER ? 1 : 0) : 1) != 0)
            {
                if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_MANAGER))
                    UnSetCashmereDeviceState(CashmereDeviceState.DEVICE_MANAGER);
            }
            else if (DeviceManager.DeviceManagerMode == DeviceManagerMode.NONE && !ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_MANAGER))
                SetCashmereDeviceState(CashmereDeviceState.DEVICE_MANAGER);
            if (CurrentApplicationState == ApplicationState.SPLASH || CurrentApplicationState == ApplicationState.STARTUP || CurrentApplicationState == ApplicationState.CIT_END)
            {
                if (e != null)
                {
                    ControllerState? controllerState1 = e.ControllerStatus?.ControllerState;
                    ControllerState controllerState2 = ControllerState.IDLE;
                    if (controllerState1.GetValueOrDefault() == controllerState2 & controllerState1.HasValue)
                    {
                        if (e != null)
                        {
                            DeviceTransactionStatus? status = e.ControllerStatus?.Transaction?.Status;
                            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                            if (status.GetValueOrDefault() == transactionStatus & status.HasValue)
                            {
                                if (DeviceManager.CashAccSysSerialFix.DE50Mode != DE50Mode.NeutralSettingMode)
                                {
                                    if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.CONTROLLER))
                                    {
                                        SetCashmereDeviceState(CashmereDeviceState.CONTROLLER);
                                        Log.Debug(GetType().Name, "Device", "Device State Changed", "Setting CashmereDeviceState.CONTROLLER on DE50 in deposit");
                                        ResetDevice();
                                        goto label_41;
                                    }
                                    else
                                        goto label_41;
                                }
                                else if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.CONTROLLER))
                                {
                                    UnSetCashmereDeviceState(CashmereDeviceState.CONTROLLER);
                                    goto label_41;
                                }
                                else
                                    goto label_41;
                            }
                        }
                        if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.CONTROLLER))
                        {
                            SetCashmereDeviceState(CashmereDeviceState.CONTROLLER);
                            ResetDevice();
                            goto label_41;
                        }
                        else
                            goto label_41;
                    }
                }
                if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.CONTROLLER))
                    SetCashmereDeviceState(CashmereDeviceState.CONTROLLER);
                ResetDevice();
            }
        label_41:
            if (DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM)
            {
                if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.ESCROW_JAM))
                    SetCashmereDeviceState(CashmereDeviceState.ESCROW_JAM);
                if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_MANAGER))
                    UnSetCashmereDeviceState(CashmereDeviceState.DEVICE_MANAGER);
            }
            else if (ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.ESCROW_JAM))
                UnSetCashmereDeviceState(CashmereDeviceState.ESCROW_JAM);

            ApplicationStatus.ControllerStatus = e.ControllerStatus;
            if (DeviceStatusChangedEvent == null)
                return;
            DeviceStatusChangedEvent(this, e);
        }

        private void DeviceManager_RaiseControllerStateChangedEvent(
          object sender,
          ControllerStateChangedEventArgs e)
        {
            if (CurrentApplicationState == ApplicationState.STARTUP)
                Log.DebugFormat(GetType().Name, "OnControllerStateChangedEvent", "EventHandling", "Controller state has changed during startup to {0}", e.ControllerState.ToString());
            else
                Log.DebugFormat(GetType().Name, "OnControllerStateChangedEvent", "EventHandling", "Controller state has changed to {0}", e.ControllerState.ToString());
        }

        private void DeviceManager_DoorOpenEvent(object sender, EventArgs e)
        {
            if (CurrentApplicationState == ApplicationState.CIT_BAG_CLOSED)
            {
                AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDeviceAsync(), DateTime.Now, true));
                CurrentApplicationState = ApplicationState.CIT_DOOR_OPENED;
                Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_DOOR_OPENED");
            }
            else
            {
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door opened outside of a CIT");
                AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDeviceAsync(), DateTime.Now));
            }

        }

        private void DeviceManager_DoorClosedEvent(object sender, EventArgs e)
        {
            lock (DoorClosedLock)
            {
                Log.Debug(GetType().Name, nameof(DeviceManager_DoorClosedEvent), "EventHandling", "DoorClosedEvent");
                if (CurrentApplicationState == ApplicationState.CIT_BAG_REPLACED)
                {
                    AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDeviceAsync(), DateTime.Now, true));
                    if (lastCIT == null || lastCIT.Complete)
                        return;
                    CurrentApplicationState = ApplicationState.CIT_DOOR_CLOSED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REPLACED");
                    EndCIT();
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door closed outside of a CIT");
                    AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDeviceAsync(), DateTime.Now));
                }

            }
        }

        private void DeviceManager_BagRemovedEvent(object sender, EventArgs e)
        {
            Log.Debug(GetType().Name, nameof(DeviceManager_BagRemovedEvent), "EventHandling", "BagRemovedEvent");
            if (CurrentApplicationState == ApplicationState.CIT_DOOR_OPENED)
            {
                CurrentApplicationState = ApplicationState.CIT_BAG_REMOVED;
                Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDeviceAsync(), DateTime.Now, true));
            }
            else
            {
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag removed outside of a CIT");
                AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDeviceAsync(), DateTime.Now));
            }

        }

        private void DeviceManager_BagPresentEvent(object sender, EventArgs e)
        {
            lock (BagReplacedLock)
            {
                if (lastCIT != null && !lastCIT.Complete)
                {
                    CurrentApplicationState = ApplicationState.CIT_BAG_REPLACED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                    AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDeviceAsync(), DateTime.Now, true));
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag inserted outside of a CIT");
                    AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDeviceAsync(), DateTime.Now));
                }

            }
        }

        private async void DeviceManager_BagOpenedEvent(object sender, EventArgs e)
        {
            var last_CIT = await _citRepository.LastCIT(lastCIT.Id);
            var device = ApplicationModel.GetDeviceAsync();
            var ciTs = await _citRepository.GetInCompleteByDeviceId(lastCIT.Id, device.Id);
            lock (BagOpenLock)
            {
                Log.Debug(GetType().Name, nameof(DeviceManager_BagOpenedEvent), "EventHandling", "BagOpenedEvent");
                if (last_CIT != null && !last_CIT.Complete)
                {
                    CurrentApplicationState = ApplicationState.CIT_END;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_END");
                    if (last_CIT != null)
                    {
                        last_CIT.Complete = true;
                        last_CIT.CITCompleteDate = DateTime.Now;
                        var reslt = _citRepository.UpdateAsync(last_CIT).ContinueWith(x => x.Result).Result;
                        Task.Run(() => PostCITTransactionsAsync(last_CIT));
                        foreach (var cit in ciTs)
                        {
                            cit.Complete = true;
                            cit.CITCompleteDate = DateTime.Now;
                            cit.CITError = 95;
                            cit.CITErrorMessage = "Incomplete CIT completed by a newer CIT";
                            AlertManager.SendAlert(new AlertCITFailed(cit, device, DateTime.Now));
                            var output = _citRepository.UpdateAsync(cit).ContinueWith(x => x.Result).Result;
                        }
                    }

                    AlertManager.SendAlert(new AlertCITSuccess(last_CIT, device, DateTime.Now));
                    InitialiseApp();
                    DeviceManager.DeviceManagerMode = DeviceManagerMode.NONE;
                }
                if (last_CIT != null && last_CIT.Complete)
                    return;
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag opened outside of a CIT");
            }
        }

        public async Task<PostTransactionResponse> _FinaclePostToCoreBankingAsync(
               Guid requestID,
               Transaction transaction)
        {
            var applicationViewModel = this;
            if (applicationViewModel.debugNoCoreBanking)
            {
                var log = Log;
                var name = applicationViewModel.GetType().Name;
                var objArray = new object[4]
                {
                    requestID,
                    transaction.TxAccountNumber,
                    transaction.TxCurrency,
                    null
                };
                var txAmount = transaction.TxAmount;
                long num2 = 100;
                var guid = Guid.NewGuid();
                guid = Guid.NewGuid();
                guid = Guid.NewGuid();
                objArray[3] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num2) : new long?();
                log.InfoFormat(name, "PostToCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", objArray);
                var coreBankingAsync = new PostTransactionResponse
                {
                    MessageID = guid.ToString().ToUpper(),
                    RequestID = guid.ToString().ToUpper(),
                    PostResponseCode = 200.ToString() ?? "",
                    PostResponseMessage = "Posted",
                    MessageDateTime = DateTime.Now,
                    IsSuccess = true,
                    TransactionDateTime = DateTime.Now,
                    TransactionID = guid.ToString().ToUpper()
                };
                return coreBankingAsync;
            }
            try
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting transaction {0}", transaction.ToString());
                var transactionTypeListItem = await _transactionTypeListItemRepository.GetByIdAsync(transaction.TxType.Value);
                var applicationModel = applicationViewModel.ApplicationModel;
                string str1;
                if (applicationModel == null)
                {
                    str1 = null;
                }
                else
                {
                    var suspenseAccounts = applicationModel.GetDeviceAsync().DeviceSuspenseAccounts;
                    str1 = suspenseAccounts != null ? suspenseAccounts.FirstOrDefault(x => x.Enabled && string.Equals(x.CurrencyCode, CurrentTransaction?.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))?.AccountNumber : null;
                }
                var str2 = str1;
                var log = Log;
                var name = applicationViewModel.GetType().Name;
                var objArray = new object[5]
                {
                    requestID,
                    transaction.TxAccountNumber,
                    transaction.TxCurrency,
                    null,
                    null
                };
                var txAmount = transaction.TxAmount;
                long num3 = 100;
                objArray[3] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num3) : new long?();
                objArray[4] = str2;
                log.InfoFormat(name, "PostToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", objArray);
                var device = applicationViewModel.CurrentSession.Device;
                var integrationServiceClient = new FinacleIntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                var guid = Guid.NewGuid();
                guid = applicationViewModel.SessionID.Value;
                var request = new PostTransactionRequest
                {
                    AppID = device.AppId,
                    AppName = device.MachineName,
                    MessageDateTime = DateTime.Now,
                    MessageID = guid.ToString(),
                    Language = applicationViewModel.CurrentLanguage,
                    DeviceID = device.Id,
                    SessionID = guid.ToString(),
                    FundsSource = transaction.FundsSource,
                    RefAccountName = transaction.CbRefAccountName,
                    RefAccountNumber = transaction.TxRefAccount,
                    DeviceReferenceNumber = string.Format("{0:#}", transaction.TxRandomNumber),
                    DepositorIDNumber = transaction.TxIdNumber,
                    DepositorName = transaction.TxDepositorName,
                    DepositorPhone = transaction.TxPhone,
                    TransactionType = transactionTypeListItem?.CbTxType,
                    TransactionTypeID = transactionTypeListItem.Id
                };
                txAmount = transaction.TxAmount;
                var postTransactionData = new PostTransactionData
                {
                    TransactionID = transaction.Id,
                    DebitAccount = new PostBankAccount()
                    {
                        AccountNumber = transaction.TxSuspenseAccount,
                        Currency = transaction.TxCurrency.ToUpper()
                    },
                    CreditAccount = new PostBankAccount()
                    {
                        AccountName = transaction.CbAccountName,
                        AccountNumber = transaction.TxAccountNumber,
                        Currency = transaction.TxCurrency.ToUpper()
                    },
                    Amount = txAmount.Value / 100.0M,
                    DateTime = transaction.TxEndDate.Value,
                    DeviceID = transaction.Device.Id,
                    DeviceNumber = transaction.Device.DeviceNumber,
                    Narration = transaction.TxNarration
                };
                request.Transaction = postTransactionData;
                var coreBankingAsync = await (integrationServiceClient.PostTransactionAsync(request));
                applicationViewModel.CheckIntegrationResponseMessageDateTime(coreBankingAsync.MessageDateTime);
                return coreBankingAsync;
            }
            catch (Exception ex)
            {
                var ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Log.Error(applicationViewModel.GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), ErrorDetail);
                var coreBankingAsync = new PostTransactionResponse
                {
                    MessageDateTime = DateTime.Now,
                    PostResponseCode = "-1",
                    PostResponseMessage = ErrorDetail,
                    RequestID = requestID.ToString().ToUpperInvariant(),
                    ServerErrorCode = "-1",
                    ServerErrorMessage = ErrorDetail,
                    IsSuccess = false
                };
                return coreBankingAsync;
            }
        }
        private async Task PostCITTransactionsAsync(CIT cit)
        {
            if (DeviceConfiguration.CIT_ALLOW_POST)
            {
                Log.Info(nameof(ApplicationViewModel), "Processing", nameof(PostCITTransactionsAsync), "DeviceConfig CIT_ALLOW_POST = {0}", new object[1]
                {
          DeviceConfiguration.CIT_ALLOW_POST
                });
                try
                {
                    if (cit == null)
                        throw new NullReferenceException("null CIT from DB");
                    foreach (var CITTransaction in cit.CITTransactions)
                    {
                        if (CITTransaction.Amount > 0L)
                        {
                            if (CITTransaction.CIT.Device.GetCITSuspenseAccount(CITTransaction.Currency) != null)
                            {
                                Log.InfoFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Posting CITTransaction Id={0}, account={1}, suspense={2}, Currency={3}, Amount={4:#,##0.##}", CITTransaction.Id, CITTransaction.AccountNumber, CITTransaction.SuspenseAccount, CITTransaction.Currency, CITTransaction.Amount / 100.0);
                                var coreBankingAsync = await PostCITTransactionToCoreBankingAsync(cit.Id, CITTransaction);
                                CITTransaction.CbDate = coreBankingAsync.TransactionDateTime;
                                CITTransaction.CbTxNumber = coreBankingAsync.TransactionID;
                                CITTransaction.CbTxStatus = coreBankingAsync.PostResponseCode;
                                CITTransaction.CbStatusDetail = coreBankingAsync.PostResponseMessage;
                                int result;
                                CITTransaction.ErrorCode = int.TryParse(coreBankingAsync.ServerErrorCode, out result) ? result : -1;
                                CITTransaction.ErrorMessage = coreBankingAsync.ServerErrorMessage;
                                Log.Info(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "Response = {0}", new object[1]
                                {
                  coreBankingAsync.ToString()
                                });
                            }
                            else
                                Log.WarningFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Error posting CITTransaction Id={0}, no CITSuspenseAccount for Currency {1}", CITTransaction.Id, CITTransaction.Currency);
                        }
                        else
                            Log.Warning(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "Skipping CITPost on zero count");
                        //SaveToDatabaseAsync().Wait();
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("ApplicationViewModel.StartCIT", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", lastCIT.Id, ex.MessageString());
                }
            }
            else
                Log.Info(nameof(ApplicationViewModel), "CIT_ALLOW_POST", nameof(PostCITTransactionsAsync), "Not allowed by config");
            //SaveToDatabaseAsync().Wait();
            Log.Trace(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "End of Function");
        }
        private void DeviceManager_BagClosedEvent(object sender, EventArgs e) => Log.Info(GetType().Name, nameof(DeviceManager_BagClosedEvent), "EventHandling", "BagClosedEvent");

        private void DeviceManager_BagFullAlertEvent(object sender, ControllerStatus e)
        {
            Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullAlertEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
            AlertManager.SendAlert(new AlertBagFull(ApplicationModel.GetDeviceAsync(), DateTime.Now, e.Bag));

        }

        private void DeviceManager_BagFullWarningEvent(object sender, ControllerStatus e)
        {
            Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullWarningEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
            AlertManager.SendAlert(new AlertBagFullWarning(ApplicationModel.GetDeviceAsync(), DateTime.Now, e.Bag));

        }

        private void CurrentSession_TransactionLimitReachedEvent(object sender, EventArgs e)
        {
        }

        private async void ApplicationStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool flag = ApplicationStatus.CashmereDeviceState == CashmereDeviceState.NONE;
            if (e.PropertyName == "CashmereDeviceState")
            {
                if (!flag)
                {
                    if (CurrentSession != null)
                    {
                        CashAccSysDeviceManager.CashAccSysDeviceManager deviceManager = DeviceManager;
                        if ((deviceManager != null ? (deviceManager.DeviceManagerMode == DeviceManagerMode.NONE ? 1 : 0) : 0) != 0)
                            EndSession(false, (int)ApplicationStatus.CashmereDeviceState, ApplicationErrorConst.ERROR_SYSTEM, ApplicationStatus.CashmereDeviceState.ToString());
                    }
                    CashAccSysDeviceManager.CashAccSysDeviceManager deviceManager1 = DeviceManager;
                    if ((deviceManager1 != null ? (deviceManager1.DeviceManagerMode != DeviceManagerMode.ESCROW_JAM ? 1 : 0) : 1) != 0)
                        ShowErrorDialog(new OutOfOrderScreenViewModel(this));
                }
                else if (CurrentSession != null)
                {
                    EndSession(false, (int)ApplicationStatus.CashmereDeviceState, ApplicationErrorConst.ERROR_SYSTEM, "State OK now, but still in session. CashmereDeviceState=" + ApplicationStatus.CashmereDeviceState.ToString());
                }
                else
                {
                    CurrentApplicationState = ApplicationState.SPLASH;
                    InitialiseApp();
                }
            }

            DeviceStatus deviceStatus = await _deviceStatusRepository.GetByMachineName(Environment.MachineName);
            if (deviceStatus == null)
            {
                var device = await _deviceRepository.GetDevice(Environment.MachineName);
                deviceStatus = CashmereDepositCommonClasses.GenerateDeviceStatus(device.Id);
            }
            deviceStatus.CurrentStatus = (int)ApplicationStatus.CashmereDeviceState;
            deviceStatus.Modified = new DateTime?(DateTime.Now);


        }

        private void DeviceManager_RaiseDeviceStateChangedEvent(
          object sender,
          DeviceStateChangedEventArgs e)
        {
            Log?.DebugFormat(GetType().Name, "OnRaiseDeviceStateChangedEvent", "EventHandling", "Device status has changed to {0}", e.Data.ToString());
            if (CurrentApplicationState == ApplicationState.DEVICE_ERROR)
            {
                if (e.Data == DeviceState.JAM)
                    return;
                if (CurrentTransaction != null)
                {
                    AlertManager.SendAlert(new AlertNoteJamClearSuccess(CurrentTransaction.Transaction, CurrentSession.Device, DateTime.Now));
                    Log?.Info(GetType().Name, "OnRaiseDeviceStateChangedEvent", "EventHandling", "Note Jam Cleared");
                    EndTransaction(ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam");
                }
                EndSession(false, 227, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, "Note Jam detected, terminating transaction");
            }
            else
            {
                if (e.Data != DeviceState.JAM)
                    return;
                Log?.Error(GetType().Name, 87, "OnRaiseDeviceStateChangedEvent", "Note Jam Detected");
                CurrentApplicationState = ApplicationState.DEVICE_ERROR;
                if (CurrentSession != null)
                    CurrentSession.CountingEnded = true;
                if (CurrentTransaction != null)
                {
                    AlertManager.SendAlert(new AlertNoteJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                    CurrentTransaction.NoteJamDetected = true;
                }
                ShowDialog(new NoteJamScreenViewModel(this));
            }
        }

        public event EventHandler<EventArgs> NotifyCurrentTransactionStatusChangedEvent;

        private void DeviceManager_NotifyCurrentTransactionStatusChangedEvent(
          object sender,
          EventArgs e)
        {
            if (NotifyCurrentTransactionStatusChangedEvent == null)
                return;
            NotifyCurrentTransactionStatusChangedEvent(this, e);
        }

        private void DeviceManager_ConnectionEvent(object sender, StringResult e)
        {
            Log.DebugFormat(GetType().Name, "OnConnectionEvent", "EventHandling", "Connection state = {0}", e.data);
            if (CurrentApplicationState != ApplicationState.STARTUP)
                return;
            if (e.resultCode == 0)
            {
                Console.WriteLine("Connection to DC Successfull");
                Log.Debug(GetType().Name, "OnConnectionEvent", "EventHandling", "Connection to DC Successfull");
                if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.COUNTING_DEVICE))
                    return;
                UnSetCashmereDeviceState(CashmereDeviceState.COUNTING_DEVICE);
            }
            else
            {
                if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.COUNTING_DEVICE))
                    SetCashmereDeviceState(CashmereDeviceState.COUNTING_DEVICE);
                Console.WriteLine("Connection to DC FAILED: {0}", e.message);
                Console.WriteLine("Attempting to reconnect");
                DeviceManager.Connect();
            }
        }

        internal void DebugOnTransactionStartedEvent(object sender, DeviceTransaction e) => DeviceManager_TransactionStartedEvent(sender, e);

        private void DeviceManager_TransactionStartedEvent(object sender, DeviceTransaction e)
        {
            if (CurrentApplicationState == ApplicationState.COUNT_STARTED)
            {
                CurrentApplicationState = ApplicationState.COUNTING;
                CashInStart();
            }
            else
                Log.WarningFormat(GetType().Name, nameof(DeviceManager_TransactionStartedEvent), "Invalid State", "DeviceManager_TransactionStartedEvent invalid in current state: {0}", CurrentApplicationState);
        }

        public event EventHandler<DeviceTransactionResult> TransactionStatusEvent;

        private void DeviceManager_TransactionStatusEvent(object sender, DeviceTransactionResult e)
        {
            Log.InfoFormat(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "EventHandling", "Escrow Total = {0}, Safe Total = {1}", e?.data?.CurrentTransactionResult?.EscrowTotalDisplayString, e?.data?.CurrentTransactionResult?.TotalDroppedAmountDisplayString);
            if (!InSessionAndTransaction("DeviceManager_TransactionEndEvent()"))
                return;
            string sessionId = e?.data?.SessionID;
            Guid guid = CurrentSession.SessionID;
            string b1 = guid.ToString();
            if (string.Equals(sessionId, b1, StringComparison.InvariantCultureIgnoreCase))
            {
                string transactionId = e?.data?.TransactionID;
                guid = CurrentTransaction.Transaction.Id;
                string b2 = guid.ToString();
                if (string.Equals(transactionId, b2, StringComparison.InvariantCultureIgnoreCase))
                {
                    AppTransaction currentTransaction1 = CurrentTransaction;
                    int num1;
                    if (currentTransaction1 == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        AppSession session = currentTransaction1.Session;
                        if (session == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            int num2 = session.CountingStarted ? 1 : 0;
                            num1 = 1;
                        }
                    }
                    if (num1 != 0)
                    {
                        AppTransaction currentTransaction2 = CurrentTransaction;
                        int num3;
                        if (currentTransaction2 == null)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            bool? countingStarted = currentTransaction2.Session?.CountingStarted;
                            bool flag = true;
                            num3 = countingStarted.GetValueOrDefault() == flag & countingStarted.HasValue ? 1 : 0;
                        }
                        if (num3 != 0)
                        {
                            CurrentSession.HasCounted = true;
                            if (e.level == ErrorLevel.SUCCESS)
                            {
                                AppTransaction currentTransaction3 = CurrentTransaction;
                                TransactionStatusResponseResult TransactionResult = new()
                                {
                                    level = e.level,
                                    data = e.data.CurrentTransactionResult
                                };
                                currentTransaction3.HandleDenominationResult(TransactionResult);
                                if (TransactionStatusEvent == null)
                                    return;
                                TransactionStatusEvent(this, e);
                                return;
                            }
                            Log.Warning(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "FAILED: e.level == ErrorLevel.SUCCESS");
                            return;
                        }
                    }
                    Log.Warning(GetType().Name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "FAILED: CurrentTransaction?.Session?.CountingStarted != null && CurrentTransaction?.Session?.CountingStarted != true");
                }
                else
                {
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    object[] objArray = new object[3]
                    {
             e.data?.SessionID,
             e?.data?.TransactionID,
            null
                    };
                    guid = CurrentTransaction.Transaction.Id;
                    objArray[2] = guid.ToString();
                    log.WarningFormat(name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "TransactionStatusEvent received for TransactionID{0} while CurrentTransactionID is {1}", objArray);
                    DeviceManager.ResetDevice(false);
                }
            }
            else
            {
                DepositorLogger log = Log;
                string name = GetType().Name;
                object[] objArray = new object[3]
                {
           e.data?.SessionID,
           e?.data?.SessionID,
          null
                };
                guid = CurrentSession.SessionID;
                objArray[2] = guid.ToString();
                log.WarningFormat(name, nameof(DeviceManager_TransactionStatusEvent), "InvalidOperation", "TransactionStatusEvent received for SessionID{0} while CurrentSessionID is {1}", objArray);
                DeviceManager.ResetDevice(false);
            }
        }

        private void DeviceManager_CashInStartedEvent(object sender, DeviceTransactionResult e) => Count();

        public event EventHandler<DeviceTransactionResult> CountStartedEvent;

        private void DeviceManager_CountStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (!InSessionAndTransaction("ApplicationViewModel.DeviceManager_CountStartedEvent()") || CountStartedEvent == null)
                return;
            CountStartedEvent(this, e);
        }

        public event EventHandler<DeviceTransactionResult> CountPauseEvent;

        private void DeviceManager_CountPauseEvent(object sender, DeviceTransactionResult e)
        {
            if (CurrentSession != null)
            {
                if (CurrentTransaction != null)
                {
                    if (CurrentApplicationState != ApplicationState.COUNTING)
                        return;
                    CurrentApplicationState = ApplicationState.COUNT_ENDING;
                    if (CountPauseEvent == null)
                        return;
                    CountPauseEvent(this, e);
                }
                else
                {
                    Log.WarningFormat(GetType().Name, "DropPauseEvent", "InvalidOperation", "DropPausedEvent received outside of a GUI transaction. Session={0} Transaction={1}", e.data?.SessionID, e?.data?.TransactionID);
                    DeviceManager.ResetDevice(false);
                }
            }
            else
            {
                Log.WarningFormat(GetType().Name, "DropPauseEvent", "InvalidOperation", "DropPausedEvent received outside of a GUI Session. Session={0}", e.data?.SessionID);
                DeviceManager.ResetDevice(false);
            }
        }

        public event EventHandler<DeviceTransactionResult> CountEndEvent;

        private void DeviceManager_CountEndEvent(object sender, DeviceTransactionResult e)
        {
            if (!InSessionAndTransaction("ApplicationViewModel.DeviceManager_CountEndEvent()") || CountEndEvent == null)
                return;
            CountEndEvent(this, e);
        }

        private void DeviceManager_TransactionEndEvent(object sender, DeviceTransactionResult e)
        {
            Log.InfoFormat(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "EventHandling", "Transaction Result: Currency = {0}, TotalDeposit = {1}, TotalDispense = {2}, Result = {3}", e?.data?.Currency, e?.data?.CurrentTransactionResult?.TotalDroppedAmountDisplayString, e?.data?.CurrentTransactionResult?.DispensedAmountDisplayString, e?.data?.CurrentTransactionResult?.ResultAmountDisplayString);
            if (!InSessionAndTransaction("DeviceManager_TransactionEndEvent()", false))
                return;
            string sessionId = e?.data?.SessionID;
            Guid guid = CurrentSession.SessionID;
            string b1 = guid.ToString();
            if (string.Equals(sessionId, b1, StringComparison.InvariantCultureIgnoreCase))
            {
                string transactionId = e?.data?.TransactionID;
                guid = CurrentTransaction.Transaction.Id;
                string b2 = guid.ToString();
                if (string.Equals(transactionId, b2, StringComparison.InvariantCultureIgnoreCase))
                {
                    AppTransaction currentTransaction1 = CurrentTransaction;
                    int num1;
                    if (currentTransaction1 == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        AppSession session = currentTransaction1.Session;
                        if (session == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            int num2 = session.CountingEnded ? 1 : 0;
                            num1 = 1;
                        }
                    }
                    if (num1 != 0)
                    {
                        AppTransaction currentTransaction2 = CurrentTransaction;
                        int num3;
                        if (currentTransaction2 == null)
                        {
                            num3 = 1;
                        }
                        else
                        {
                            bool? countingEnded = currentTransaction2.Session?.CountingEnded;
                            bool flag = true;
                            num3 = !(countingEnded.GetValueOrDefault() == flag & countingEnded.HasValue) ? 1 : 0;
                        }
                        if (num3 != 0)
                        {
                            CurrentTransaction.Session.CountingEnded = true;
                            CurrentSession.HasCounted = true;
                            if (e.level == ErrorLevel.SUCCESS)
                            {
                                if (CurrentTransaction != null)
                                {
                                    AppTransaction currentTransaction3 = CurrentTransaction;
                                    TransactionStatusResponseResult TransactionResult = new()
                                    {
                                        level = e.level,
                                        data = e.data.CurrentTransactionResult
                                    };
                                    currentTransaction3.HandleDenominationResult(TransactionResult);
                                    ValidateTransactedAmount();
                                    return;
                                }
                                Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: CurrentTransaction != null");
                                return;
                            }
                            Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: e.level == ErrorLevel.SUCCESS");
                            return;
                        }
                    }
                    Log.Warning(GetType().Name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "FAILED: CurrentTransaction?.Session?.CountingEnded != null && CurrentTransaction?.Session?.CountingEnded != true");
                }
                else
                {
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    object[] objArray = new object[3]
                    {
             e.data?.SessionID,
             e?.data?.TransactionID,
            null
                    };
                    guid = CurrentTransaction.Transaction.Id;
                    objArray[2] = guid.ToString();
                    log.WarningFormat(name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "TransactionStatusEvent received for TransactionID{0} while CurrentTransactionID is {1}", objArray);
                    DeviceManager.ResetDevice(false);
                }
            }
            else
            {
                DepositorLogger log = Log;
                string name = GetType().Name;
                object[] objArray = new object[3]
                {
           e.data?.SessionID,
           e?.data?.SessionID,
          null
                };
                guid = CurrentSession.SessionID;
                objArray[2] = guid.ToString();
                log.WarningFormat(name, nameof(DeviceManager_TransactionEndEvent), "InvalidOperation", "TransactionStatusEvent received for SessionID{0} while CurrentSessionID is {1}", objArray);
                DeviceManager.ResetDevice(false);
            }
        }

        private async void DeviceManager_CITResultEvent(object sender, CITResult e)
        {
            Log.Debug(GetType().Name, "OnCITResultEvent", "EventHandling", "CIT Result");
            if (CurrentApplicationState != ApplicationState.CIT_START || e.level != ErrorLevel.SUCCESS)
                return;
            CurrentApplicationState = ApplicationState.CIT_BAG_CLOSED;
            Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_CLOSED");
            var cit = await _citRepository.GetByIdAsync(lastCIT.Id);
            PrintCITReceipt(cit);

        }

        private void Printer_StatusChangedEvent(
          object sender,
          DepositorPrinter.PrinterStateChangedEventArgs e)
        {
            if ((ApplicationStatus.CashmereDeviceState & CashmereDeviceState.PRINTER) == CashmereDeviceState.PRINTER)
            {
                if (e.state.HasError || !ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.PRINTER))
                    return;
                UnSetCashmereDeviceState(CashmereDeviceState.PRINTER);
            }
            else
            {
                if (e.state.HasError || ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.PRINTER))
                    return;
                SetCashmereDeviceState(CashmereDeviceState.PRINTER);
            }
        }

        private void ApplicationModel_DatabaseStorageErrorEvent(object sender, EventArgs e)
        {
            if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DATABASE))
                return;
            UnSetCashmereDeviceState(CashmereDeviceState.DATABASE);
        }

        private void OnApplicationStartupFailedEvent(
          object sender,
          ApplicationErrorConst e,
          string errorMessage)
        {
            Log.ErrorFormat(GetType().Name, (int)e, e.ToString(), "{0:G}: {1}", e, errorMessage);

            Device device = ApplicationModel.GetDeviceAsync();
            if (AlertManager != null)
            {
                if (device != null)
                    AlertManager.SendAlert(new AlertDeviceStartupFailed(string.Format("{0:G}: {1}", e, errorMessage), device, DateTime.Now));
            }

            Console.WriteLine(string.Format(string.Format("{0:G}: {1}", e, errorMessage)));
            Application.Current.Shutdown((int)e);
        }

        public event EventHandler<DeviceStatusChangedEventArgs> DeviceStatusChangedEvent;

        internal void ConnectToDevice()
        {
            Log.Debug(GetType().Name, nameof(ConnectToDevice), "EventHandling", "Connecting to the device");
            if (debugNoDevice)
            {
                StringResult e = new()
                {
                    resultCode = 0,
                    extendedResult = "ACCEPTED",
                    level = ErrorLevel.SUCCESS
                };
                DeviceManager_ConnectionEvent(this, e);
            }
            else
                DeviceManager.Connect();
        }

        internal void DeviceTransactionStart(long transactionLimitCents = 0, long transactionValueCents = 0)
        {
            if (CurrentSession == null)
                throw new NullReferenceException("CurrentSession cannot be null in ApplicationViewModel.TransactionStart()");
            if (CurrentTransaction == null)
                throw new NullReferenceException("CurrentTransaction cannot be null in ApplicationViewModel.TransactionStart()");
            if (debugNoDevice)
                return;
            try
            {
                CashAccSysDeviceManager.CashAccSysDeviceManager deviceManager = DeviceManager;
                string currencyCode = CurrentTransaction.CurrencyCode;
                string accountNumber = CurrentTransaction?.AccountNumber;
                AppSession currentSession = CurrentSession;
                Guid guid;
                string sessionID;
                if (currentSession == null)
                {
                    sessionID = null;
                }
                else
                {
                    guid = currentSession.SessionID;
                    sessionID = guid.ToString().ToUpperInvariant();
                }
                AppTransaction currentTransaction = CurrentTransaction;
                string transactionID;
                if (currentTransaction == null)
                {
                    transactionID = null;
                }
                else
                {
                    guid = currentTransaction.Transaction.Id;
                    transactionID = guid.ToString().ToUpperInvariant();
                }
                long transactionLimitCents1 = transactionLimitCents;
                long transactionValueCents1 = transactionValueCents;
                deviceManager.TransactionStart(currencyCode, accountNumber, sessionID, transactionID, transactionLimitCents1, transactionValueCents1);
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine("DeviceTransaction already in progress");
                Log.WarningFormat(GetType().Name, "DeviceTransactionStart Failed", "DeviceTransactionStart Failed", "DeviceTransaction already in progress: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                ResetDevice();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("something went wrong. Reset the DeviceManager");
                Log.WarningFormat(GetType().Name, "DeviceTransactionStart Failed", "DeviceTransactionStart Failed", "ERROR: {0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
                ResetDevice();
            }
        }

        internal void CashInStart()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "CashInStart()");
            AppTransaction currentTransaction = CurrentTransaction;
            int num1;
            if (currentTransaction == null)
            {
                num1 = 0;
            }
            else
            {
                AppSession session = currentTransaction.Session;
                if (session == null)
                {
                    num1 = 0;
                }
                else
                {
                    int num2 = session.CountingStarted ? 1 : 0;
                    num1 = 1;
                }
            }
            if (num1 == 0)
                throw new NullReferenceException("Session cannot be null when calling CashInStart()");
            CurrentTransaction.Session.CountingStarted = true;
            if (debugNoDevice)
                return;
            DeviceManager.CashInStart();
        }

        public bool CanCount => DeviceManager.CanCount && InSessionAndTransaction("ApplicationViewModel.CanCount()", false);

        internal void Count(bool countNotes = true)
        {
            if (debugNoDevice)
                return;
            Log.Info(GetType().Name, nameof(Count), "Device Management", "Request Count");
            if (!InSessionAndTransaction("ApplicationViewModel.Count()"))
                return;
            DeviceManager.Count();
        }

        internal void PauseCount()
        {
            if (debugNoDevice || ApplicationStatus.ControllerStatus.ControllerState == ControllerState.DROP_PAUSED)
                return;
            Log.Info(GetType().Name, "Count", "Device Management", "PauseCount()");
            if (!InSessionAndTransaction("ApplicationViewModel.PauseCount()"))
                return;
            DeviceManager.PauseCount();
        }

        public bool CanEscrowDrop
        {
            get
            {
                if (!InSessionAndTransaction("ApplicationViewModel.CanEscrowDrop()", false) || !DeviceManager.CanEscrowDrop)
                    return false;
                long? droppedAmountCents1 = CurrentTransaction?.DroppedAmountCents;
                long? droppedAmountCents2 = DeviceManager?.DroppedAmountCents;
                return droppedAmountCents1.GetValueOrDefault() == droppedAmountCents2.GetValueOrDefault() & droppedAmountCents1.HasValue == droppedAmountCents2.HasValue;
            }
        }

        public bool CanEndCount => InSessionAndTransaction("ApplicationViewModel.CanEndCount()", false) && DeviceManager.CanEndCount;

        internal void EscrowDrop()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowDrop()");
            if (debugNoDevice)
                throw new NotImplementedException("Debug EscrowDrop() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowDrop()"))
                return;
            DeviceManager.EscrowDrop();
        }

        public bool CanEscrowReject => InSessionAndTransaction("ApplicationViewModel.CanEscrowReject()", false) && DeviceManager.CanEscrowReject;

        internal void EscrowReject()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowReject()");
            if (debugNoDevice)
                throw new NotImplementedException("Debug EscrowReject() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowReject()"))
                return;
            DeviceManager.EscrowReject();
        }

        public bool CanTransactionEnd => InSessionAndTransaction("ApplicationViewModel.CanTransactionEnd", false) && DeviceManager.CanTransactionEnd;

        internal void DeviceTransactionEnd()
        {
            Log.Info(GetType().Name, "Count", "DeviceTransactionEnd()", "DeviceTransactionEnd()");
            DeviceManager.TransactionEnd();
        }

        internal void ValidateTransactedAmount()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "ValidateTransactedAmount()");

            if (CurrentTransaction == null)
                return;
            var transactionLimits1 = CurrentTransaction.TransactionLimits;
            long? nullable1;
            if ((transactionLimits1 != null ? transactionLimits1.ShowFundsSource ? 1 : 0 : 0) != 0)
            {
                var droppedDisplayAmount = CurrentTransaction.DroppedDisplayAmount;
                nullable1 = CurrentTransaction.TransactionLimits?.FundsSourceAmount;
                var nullable2 = nullable1.HasValue ? nullable1.GetValueOrDefault() : new Decimal?();
                var valueOrDefault = nullable2.GetValueOrDefault();
                if (droppedDisplayAmount > valueOrDefault & nullable2.HasValue)
                {
                    var log = Log;
                    var name = GetType().Name;
                    var objArray = new object[2]
                    {
                        CurrentTransaction.DroppedDenomination.TotalValue / 100L,
                        null
                    };
                    var transactionLimits2 = CurrentTransaction.TransactionLimits;
                    long? nullable3;
                    if (transactionLimits2 == null)
                    {
                        nullable1 = new long?();
                        nullable3 = nullable1;
                    }
                    else
                        nullable3 = transactionLimits2.FundsSourceAmount;
                    objArray[1] = nullable3;
                    log.InfoFormat(name, "Count", "Device Management", "Transaction of {0} is above value limit of {1}, insert FundsSource screen", objArray);
                    ShowFundsSourceScreen();
                    return;
                }
            }
            var transactionLimits3 = CurrentTransaction.TransactionLimits;
            if ((transactionLimits3 != null ? transactionLimits3.PreventOverdeposit ? 1 : 0 : 0) != 0 && CurrentTransaction.IsOverDeposit)
            {
                var title = CashmereTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Title", "Transaction Limit Exceeded");
                var format = CashmereTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Message", "Transaction limit of {0} {1} has been exceeded. Please visit your nearest branch to finish the deposit.");
                var upper = CurrentTransaction.CurrencyCode.ToUpper();
                var transactionLimits4 = CurrentTransaction.TransactionLimits;
                long? nullable4;
                if (transactionLimits4 == null)
                {
                    nullable1 = new long?();
                    nullable4 = nullable1;
                }
                else
                    nullable4 = transactionLimits4.OverdepositAmount;
                // ISSUE: variable of a boxed type
                var local = (ValueType)nullable4;
                var message = string.Format(format, upper, local);
                ShowUserMessageScreen(title, message);
                var log = Log;
                var name = GetType().Name;
                var ErrorName = ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED.ToString();
                var objArray1 = new object[5];
                var Id = CurrentTransaction.Transaction.Id;
                objArray1[0] = Id.ToString().ToUpper();
                objArray1[1] = CurrentTransaction?.CurrencyCode?.ToUpper();
                objArray1[2] = CurrentTransaction?.DroppedDisplayAmount;
                objArray1[3] = CurrentTransaction?.TransactionType?.DefaultAccountCurrency?.ToUpper();
                var transactionLimits5 = CurrentTransaction.TransactionLimits;
                long? nullable5;
                if (transactionLimits5 == null)
                {
                    nullable1 = new long?();
                    nullable5 = nullable1;
                }
                else
                    nullable5 = transactionLimits5.OverdepositAmount;
                objArray1[4] = nullable5;
                log.ErrorFormat(name, 27, ErrorName, "Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray1);
                var objArray2 = new object[5];
                Id = CurrentTransaction.Transaction.Id;
                objArray2[0] = Id.ToString().ToUpper();
                objArray2[1] = CurrentTransaction?.CurrencyCode?.ToUpper();
                objArray2[2] = CurrentTransaction?.DroppedDisplayAmount;
                objArray2[3] = CurrentTransaction?.TransactionType?.DefaultAccountCurrency?.ToUpper();
                var transactionLimits6 = CurrentTransaction.TransactionLimits;
                long? nullable6;
                if (transactionLimits6 == null)
                {
                    nullable1 = new long?();
                    nullable6 = nullable1;
                }
                else
                    nullable6 = transactionLimits6.OverdepositAmount;
                objArray2[4] = nullable6;
                EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED, string.Format("Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray2));
            }
            else
            {
                var currentTransaction = CurrentTransaction;
                if ((currentTransaction != null ? currentTransaction.NoteJamDetected ? 1 : 0 : 0) != 0)
                {
                    EndTransaction(ApplicationErrorConst.ERROR_DEVICE_NOTEJAM, ApplicationErrorConst.ERROR_DEVICE_NOTEJAM.ToString());
                }
                else
                {
                    Log.Info(GetType().Name, "Count", "Device Management", "Transaction below limit, continue as normal");
                    EndTransaction();
                    NavigateNextScreen();
                }
            }
        }


        internal async void ShowFundsSourceScreen()
        {
            if ((bool)!CurrentTransaction?.TransactionLimits?.ShowFundsSource)
                return;
            var entity = await _gUIScreenRepository.GetGUIScreenByCode(new Guid("33EC330E-FB51-4626-906D-1A3F77AAA5E2"));
            if (entity != null)
            {
                GUIScreens.Insert(CurrentScreenIndex + 1, entity);
                //((IObjectContextAdapter) _depositorDBContext).ObjectContext.Detach((object) entity);
                NavigateNextScreen();
            }
            else
            {
                Log.Error(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "cannot find FundsSourceScreen in database");
                EndTransaction(ApplicationErrorConst.ERROR_DATABASE_GENERAL, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString());
            }
        }

        internal void ShowUserMessageScreen(string title, string message, bool required = false) => this.ActivateItemAsync((object)new UserMessageScreenViewModel(title, this, DeviceConfiguration.USER_SCREEN_TIMEOUT, required)
        {
            Message = message
        });

        internal void EndTransaction(ApplicationErrorConst result = ApplicationErrorConst.ERROR_NONE, string ErrorMessage = null)
        {
            Log.InfoFormat(GetType().Name, nameof(EndTransaction), "Device Management", "EndTransaction with result {0}", result.ToString());
            if (CurrentTransaction == null)
                return;
            if (result == ApplicationErrorConst.ERROR_NONE || result == ApplicationErrorConst.ERROR_DEVICE_NOTEJAM && DeviceConfiguration.POST_ON_NOTEJAM || result == ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM && DeviceConfiguration.POST_ON_ESCROWJAM || result == ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT)
            {
                CurrentTransaction.EndDate = DateTime.Now;
                if (CurrentTransaction.DroppedAmountCents > 0L)
                {
                    try
                    {
                        lock (CurrentTransaction.PostingLock)
                        {
                            if (!CurrentTransaction.isPosting)
                            {
                                if (!CurrentTransaction.hasPosted)
                                {
                                    Log.InfoFormat(GetType().Name, "Attempt", "Posting", "Transaction [{0}] of Amount {1} {2:###,##0.00} is NOT over the limit of {3} {4:###,##0.00} or overdeposit is disabled.", CurrentTransaction.Transaction.Id.ToString().ToUpper(), CurrentTransaction?.CurrencyCode?.ToUpper(), CurrentTransaction?.DroppedDisplayAmount, CurrentTransaction?.TransactionType?.DefaultAccountCurrency?.ToUpper(), CurrentTransaction.TransactionLimits?.OverdepositAmount);
                                    CurrentTransaction.isPosting = true;
                                    CurrentTransaction.hasPosted = true;
                                    PostTransactionResponse result1 = Task.Run(() => PostToCoreBankingAsync(SessionID.Value, CurrentTransaction.Transaction)).Result;
                                    CurrentTransaction.Transaction.CbDate = new DateTime?(result1.TransactionDateTime);
                                    CurrentTransaction.Transaction.CbTxNumber = result1.TransactionID;
                                    CurrentTransaction.Transaction.CbTxStatus = result1.PostResponseCode;
                                    CurrentTransaction.Transaction.CbStatusDetail = result1.PostResponseMessage;
                                    EscrowJam escrowJam = CurrentTransaction.Transaction.EscrowJams.FirstOrDefault();
                                    if (escrowJam != null)
                                        escrowJam.PostedAmount = CurrentTransaction.Transaction.TxAmount.GetValueOrDefault();
                                    if (result1.IsSuccess)
                                    {
                                        Log.InfoFormat(GetType().Name, "Attempt", "Posting", "Post SUCCESS with txID = {0}", result1?.TransactionID);
                                        try
                                        {
                                            AlertManager.SendAlert(new AlertTransactionCustomerAlert(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Error("AppTransaction", "Send Customer SMS", nameof(EndTransaction), "Could not send customer SMS with error: {0}", new object[1]
                                            {
                         ex.MessageString()
                                            });
                                        }
                                        CurrentTransaction.EndTransaction(result, ErrorMessage);
                                    }
                                    else
                                        CurrentTransaction.EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE, result1.ServerErrorMessage);
                                }
                                else
                                    Log.WarningFormat(GetType().Name, "PostingInProgress", "Posting", "Transaction [{0}] has already posted", CurrentTransaction?.Transaction?.Id);
                            }
                            else
                                Log.WarningFormat(GetType().Name, "PostingInProgress", "Posting", "Transaction [{0}] is already posting", CurrentTransaction?.Transaction?.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat(GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), "Transaction posting failed>>ERROR: {0}", ex.MessageString());
                        CurrentTransaction?.EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString());
                    }
                }
                else
                {
                    Log.Warning(GetType().Name, "Skip Post on Zero Count", "Posting", "Posting skipped on account of zero Amount counted");
                    CurrentTransaction.EndTransaction(result, ErrorMessage);
                }
            }
            else
                CurrentTransaction.EndTransaction(result, ErrorMessage);
            if (CurrentTransaction == null)
                return;
            CurrentSession.Transaction = null;
        }

        internal void CancelCount()
        {
            Log.Info(GetType().Name, nameof(CancelCount), "Device Management", nameof(CancelCount));
            DeviceTransactionEnd();
        }

        internal void ResetDevice()
        {
            Log.Info(GetType().Name, nameof(ResetDevice), "Device Management", nameof(ResetDevice));
            DeviceManager.ResetDevice(false);
        }

        internal void StartCIT(string sealNumber)
        {
            Log.InfoFormat(GetType().Name, nameof(StartCIT), "Device Management", "Seal Number = {0}, Bag Number = {1}", sealNumber, lastCIT.NewBagNumber);
            CurrentApplicationState = ApplicationState.CIT_START;
            if (debugNoDevice)
            {
                CITResult e = new()
                {
                    level = ErrorLevel.SUCCESS,
                    extendedResult = "",
                    resultCode = 0,
                    data = new CITResultBody()
                    {
                        BagNumber = lastCIT?.NewBagNumber,
                        Currency = "KES",
                        CurrencyCount = 1,
                        DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        DeviceSerialNumber = "wfsdfsd",
                        Name = "Harrold",
                        TotalValue = 3000L,
                        TransactionCount = 3,
                        denomination = new Denomination()
                        {
                            DenominationItems = new List<DenominationItem>()
            {
              new DenominationItem()
              {
                Currency = "KES",
                denominationValue = 1000,
                count = 3L,
                type = DenominationItemType.NOTE
              }
            }
                        }
                    }
                };
                DeviceManager_CITResultEvent(this, e);
            }
            else
                _deviceManager.StartCIT(sealNumber);
        }

        internal void EndCIT()
        {
            Log.InfoFormat(GetType().Name, nameof(EndCIT), "Device Management", "Bag Number = {0}", lastCIT.NewBagNumber);
            _deviceManager.EndCIT(lastCIT.NewBagNumber);

        }

        internal void LockDevice(bool lockedByDevice, ApplicationErrorConst error, string errorMessage)
        {

            Device device = ApplicationModel.GetDeviceAsync();
            Log.WarningFormat(GetType().Name, nameof(LockDevice), "Device Lock", "LockedByDevice = {0}: Error: {1}>>{2}", lockedByDevice ? "TRUE" : (object)"FALSE", error.ToString(), errorMessage);
            device.Enabled = false;
            Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));");
            AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));
            Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "_depositorDBContext.DeviceLocks.Add(new DeviceLock");
            _deviceLockRepository.AddAsync(new DeviceLock()
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Locked = true,
                LockingUser = CurrentUser?.Id,
                LockDate = DateTime.Now,
                LockedByDevice = lockedByDevice,
                WebLockingUser = null
            });

            SetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);

        }

        internal void UnLockDevice(bool lockedByDevice, string lockMessage = null)
        {

            Device device = ApplicationModel.GetDeviceAsync();
            Log.Info(GetType().Name, nameof(UnLockDevice), "Device Lock", lockMessage);
            device.Enabled = true;
            AlertManager.SendAlert(new AlertDeviceUnLocked(lockMessage, device, DateTime.Now));
            _deviceLockRepository.AddAsync(new DeviceLock()
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Locked = true,
                LockingUser = CurrentUser?.Id,
                LockDate = DateTime.Now,
                LockedByDevice = lockedByDevice,
                WebLockingUser = null
            });

            if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_LOCK))
                return;
            UnSetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);

        }

        internal void ClearEscrowJam() => DeviceManager.ClearEscrowJam();

        internal void EndEscrowJam() => DeviceManager.EndEscrowJam();

        internal async void LogoffUsers(bool forced = false)
        {
            try
            {
                if (CurrentUser != null)
                {
                    DeviceLogin deviceLogin = await _deviceLoginRepository.GetFirst(CurrentUser.Id);
                    if (deviceLogin != null)
                    {
                        deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                        deviceLogin.ForcedLogout = new bool?(forced);
                    }
                }
                if (ValidatingUser != null)
                {
                    DeviceLogin deviceLogin = await _deviceLoginRepository.GetFirst(CurrentUser.Id);
                    if (deviceLogin != null)
                    {
                        deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                        deviceLogin.ForcedLogout = new bool?(forced);
                    }
                }


            }
            catch (Exception ex)
            {
            }
            finally
            {
                CurrentUser = null;
                ValidatingUser = null;
            }
        }

        internal async void LockUser(
          ApplicationUser user,
          bool lockedByDevice,
          ApplicationErrorConst error,
          string errorMessage)
        {
            Log.WarningFormat(GetType().Name, nameof(LockUser), "User Locked by device", "Error {0} {1} {2}", error, error.ToString(), errorMessage);
            ApplicationUser applicationUser = await _applicationUserRepository.GetByIdAsync(user.Id);
            applicationUser.DepositorEnabled = new bool?(false);
            applicationUser.IsActive = new bool?(false);
            AlertManager.SendAlert(new AlertUserLocked(user, ApplicationModel.GetDeviceAsync(), errorMessage, DateTime.Now));
            await _userLockRepository.AddAsync(new UserLock()
            {
                Id = Guid.NewGuid(),
                ApplicationUserLoginDetail = user.ApplicationUserLoginDetail,
                LockType = new int?(0),
                InitiatingUser = CurrentUser?.Id,
                LogDate = new DateTime?(DateTime.Now),
                WebPortalInitiated = new bool?(false)
            });


        }

        internal async void UnLockUser(ApplicationUser user, bool lockedByUser, string lockMessage = null)
        {
            Log.Info(GetType().Name, nameof(UnLockUser), "User Lock", lockMessage);
            ApplicationUser applicationUser = await _applicationUserRepository.GetByIdAsync(user.Id);
            applicationUser.DepositorEnabled = new bool?(true);
            applicationUser.IsActive = new bool?(true);
            AlertManager.SendAlert(new AlertUserUnLocked(user, ApplicationModel.GetDeviceAsync(), lockMessage, DateTime.Now));
            await _userLockRepository.AddAsync(new UserLock()
            {
                Id = Guid.NewGuid(),
                ApplicationUserLoginDetail = user.ApplicationUserLoginDetail,
                LockType = new int?(1),
                InitiatingUser = CurrentUser?.Id,
                LogDate = new DateTime?(DateTime.Now),
                WebPortalInitiated = new bool?(false)
            });


        }

        internal bool UserPermissionAllowed(
          ApplicationUser currentUser,
          string activityString,
          bool isAuthorising = false)
        {
            try
            {
                return GetUserPermissionAsync(currentUser, activityString, isAuthorising).ContinueWith(x => x.Result).Result != null;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        internal async Task<Permission> GetUserPermissionAsync(
          ApplicationUser user,
          string activity,
          bool isAuthenticating = false)
        {
            if (user == null)
                return null;
            if (string.IsNullOrWhiteSpace(activity))
                return null;

            Activity Activity = await _activityRepository.GetByName(activity);
            if (Activity == null)
                return null;
            return await _permissionRepository.GetFirst(user, Activity.Id, isAuthenticating);

        }

        internal void TimeoutSession(string screen, double timeout, string message = null)
        {
            Log.InfoFormat(GetType().Name, nameof(TimeoutSession), "Session", "Screen {0} has timed out after {1:0.###} millisecond(s) with message {2}", screen, timeout, message);
            AlertManager.SendAlert(new AlertTransactionTimedout(CurrentTransaction, ApplicationModel.GetDeviceAsync(), DateTime.Now));
            if (CurrentSession != null)
                CurrentSession.CountingEnded = true;
            EndTransaction(ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT, "Timeout on screen " + screen);
            EndSession(false, 1014, ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT, string.Format("Screen {0} has timed out after {1:0.###} miliseconds with message {2}", screen, timeout, message));
            ResetDevice();
        }

        internal void StartCountingProcess()
        {
            if (CurrentApplicationState != ApplicationState.COUNT_STARTED)
            {
                Log.Info(GetType().Name, nameof(StartCountingProcess), "Device Management", "BeginCount");
                CurrentApplicationState = ApplicationState.COUNT_STARTED;
                ReferencesAccepted();
                var prevent_overdeposit = CurrentTransaction?.TransactionType?.TxLimitListNavigation?.Get_prevent_overdeposit(CurrentTransaction.Currency);
                var overdeposit_amount = CurrentTransaction?.TransactionType?.TxLimitListNavigation?.Get_overdeposit_amount(CurrentTransaction.Currency);
                long transactionLimitCents =
                    prevent_overdeposit.GetValueOrDefault()
                    ? overdeposit_amount.GetValueOrDefault()
                    : long.MaxValue;
                AppTransaction currentTransaction = CurrentTransaction;
                long transactionValueCents = currentTransaction != null ? currentTransaction.TransactionValue : 0L;
                DeviceTransactionStart(transactionLimitCents, transactionValueCents);
            }
            else
                Log.Warning(GetType().Name, nameof(StartCountingProcess), "InvalidOperation", "Invalid state change requested");
        }

        public void PrintReceipt(Transaction transaction, bool reprint = false)
        {
            if (debugDisablePrinter)
                return;
            Log.InfoFormat(GetType().Name, nameof(PrintReceipt), "Commands", "Transaction Code = {0}, Reprint = {1}", transaction.Id, reprint);
            Printer.PrintTransaction(transaction, reprint);

        }

        public void PrintCITReceipt(CIT cit, bool reprint = false)
        {
            if (debugDisablePrinter)
                return;
            Log.InfoFormat(GetType().Name, nameof(PrintCITReceipt), "Commands", "CIT Code = {0}, Reprint = {1}", cit.Id, reprint);
            Printer.PrintCIT(cit, reprint);
        }

        public async Task<PostTransactionResponse> PostToCoreBankingAsync(
          Guid requestID,
          Transaction transaction)
        {
            ApplicationViewModel applicationViewModel = this;
            int num1 = 0;
            if (num1 != 0 && applicationViewModel.debugNoCoreBanking)
            {
                DepositorLogger log = Log;
                string name = applicationViewModel.GetType().Name;
                object[] objArray = new object[4]
                {
             requestID,
             transaction.TxAccountNumber,
             transaction.TxCurrency,
            null
                };
                long? txAmount = transaction.TxAmount;
                long num2 = 100;
                objArray[3] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num2) : new long?();
                log.InfoFormat(name, "PostToCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", objArray);
                PostTransactionResponse coreBankingAsync = new();
                Guid guid = Guid.NewGuid();
                coreBankingAsync.MessageID = guid.ToString().ToUpper();
                guid = Guid.NewGuid();
                coreBankingAsync.RequestID = guid.ToString().ToUpper();
                coreBankingAsync.PostResponseCode = 200.ToString() ?? "";
                coreBankingAsync.PostResponseMessage = "Posted";
                coreBankingAsync.MessageDateTime = DateTime.Now;
                coreBankingAsync.IsSuccess = true;
                coreBankingAsync.TransactionDateTime = DateTime.Now;
                guid = Guid.NewGuid();
                coreBankingAsync.TransactionID = guid.ToString().ToUpper();
                return coreBankingAsync;
            }
            try
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting transaction {0}", transaction.ToString());
                TransactionTypeListItem transactionTypeListItem = await _transactionTypeListItemRepository.GetByIdAsync(transaction.TxType.Value);
                ApplicationModel applicationModel = applicationViewModel.ApplicationModel;
                string str1;
                if (applicationModel == null)
                {
                    str1 = null;
                }
                else
                {
                    ICollection<DeviceSuspenseAccount> suspenseAccounts = applicationModel.GetDeviceAsync().DeviceSuspenseAccounts;
                    str1 = suspenseAccounts != null ? suspenseAccounts.FirstOrDefault(x => x.Enabled && string.Equals(x.CurrencyCode, CurrentTransaction?.CurrencyCode, StringComparison.InvariantCultureIgnoreCase))?.AccountNumber : null;
                }
                string str2 = str1;
                DepositorLogger log = Log;
                string name = applicationViewModel.GetType().Name;
                object[] objArray = new object[5]
                {
                     requestID,
                     transaction.TxAccountNumber,
                     transaction.TxCurrency,
                    null,
                    null
                };
                long? txAmount = transaction.TxAmount;
                long num3 = 100;
                objArray[3] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num3) : new long?();
                objArray[4] = str2;
                log.InfoFormat(name, "PostToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", objArray);
                Device device = applicationViewModel.CurrentSession.Device;
                IntegrationServiceClient integrationServiceClient = new(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                PostTransactionRequest request = new()
                {
                    AppID = device.AppId,
                    AppName = device.MachineName,
                    MessageDateTime = DateTime.Now
                };
                Guid guid = Guid.NewGuid();
                request.MessageID = guid.ToString();
                request.Language = applicationViewModel.CurrentLanguage;
                request.DeviceID = device.Id;
                guid = applicationViewModel.SessionID.Value;
                request.SessionID = guid.ToString();
                request.FundsSource = transaction.FundsSource;
                request.RefAccountName = transaction.CbRefAccountName;
                request.RefAccountNumber = transaction.TxRefAccount;
                request.DeviceReferenceNumber = string.Format("{0:#}", transaction.TxRandomNumber);
                request.DepositorIDNumber = transaction.TxIdNumber;
                request.DepositorName = transaction.TxDepositorName;
                request.DepositorPhone = transaction.TxPhone;
                request.TransactionType = transactionTypeListItem?.CbTxType;
                request.TransactionTypeID = transactionTypeListItem.Id;
                PostTransactionData postTransactionData = new()
                {
                    TransactionID = transaction.Id,
                    DebitAccount = new PostBankAccount()
                    {
                        AccountNumber = transaction.TxSuspenseAccount,
                        Currency = transaction.TxCurrency.ToUpper()
                    },
                    CreditAccount = new PostBankAccount()
                    {
                        AccountName = transaction.CbAccountName,
                        AccountNumber = transaction.TxAccountNumber,
                        Currency = transaction.TxCurrency.ToUpper()
                    }
                };
                txAmount = transaction.TxAmount;
                postTransactionData.Amount = txAmount.Value / 100.0M;
                postTransactionData.DateTime = transaction.TxEndDate.Value;
                postTransactionData.DeviceID = transaction.Device.Id;
                postTransactionData.DeviceNumber = transaction.Device.DeviceNumber;
                postTransactionData.Narration = transaction.TxNarration;
                request.Transaction = postTransactionData;
                // ISSUE: explicit non-virtual call
                PostTransactionResponse coreBankingAsync = await integrationServiceClient.PostTransactionAsync(request);
                applicationViewModel.CheckIntegrationResponseMessageDateTime(coreBankingAsync.MessageDateTime);
                return coreBankingAsync;
            }
            catch (Exception ex)
            {
                string ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Log.Error(applicationViewModel.GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), ErrorDetail);
                PostTransactionResponse coreBankingAsync = new()
                {
                    MessageDateTime = DateTime.Now,
                    PostResponseCode = "-1",
                    PostResponseMessage = ErrorDetail,
                    RequestID = requestID.ToString().ToUpperInvariant(),
                    ServerErrorCode = "-1",
                    ServerErrorMessage = ErrorDetail,
                    IsSuccess = false
                };
                return coreBankingAsync;
            }

        }

        public async Task<PostCITTransactionResponse> PostCITTransactionToCoreBankingAsync(
          Guid requestID,
          CITTransaction CITTransaction)
        {
            ApplicationViewModel applicationViewModel = this;
            if (applicationViewModel.debugNoCoreBanking)
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "PostCITTransactionToCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.AccountNumber, CITTransaction.Currency, CITTransaction.Amount / 100L);
                PostCITTransactionResponse coreBankingAsync = new()
                {
                    MessageID = Guid.NewGuid().ToString().ToUpper(),
                    RequestID = Guid.NewGuid().ToString().ToUpper(),
                    PostResponseCode = 200.ToString() ?? "",
                    PostResponseMessage = "Posted",
                    MessageDateTime = DateTime.Now,
                    IsSuccess = true
                };
                return coreBankingAsync;
            }
            Device device = ApplicationViewModel.GetDeviceAsync();
            try
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting CITTransaction {0}", CITTransaction.ToString());
                Log.InfoFormat(applicationViewModel.GetType().Name, "PostCITTransactionToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.AccountNumber, CITTransaction.Currency, CITTransaction.Amount / 100L, CITTransaction.SuspenseAccount);
                IntegrationServiceClient integrationServiceClient = new(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                PostCITTransactionRequest request = new()
                {
                    SessionID = CITTransaction.Id.ToString(),
                    AppID = device.AppId,
                    AppName = device.MachineName,
                    MessageDateTime = DateTime.Now,
                    MessageID = Guid.NewGuid().ToString(),
                    Language = applicationViewModel.CurrentLanguage,
                    DeviceID = device.Id,
                    Transaction = new PostTransactionData()
                    {
                        TransactionID = CITTransaction.Id,
                        Amount = CITTransaction.Amount / 100.0M,
                        DateTime = CITTransaction.Datetime,
                        DeviceID = device.Id,
                        DeviceNumber = device.DeviceNumber,
                        Narration = CITTransaction.Narration,
                        DebitAccount = new PostBankAccount()
                        {
                            AccountNumber = CITTransaction.AccountNumber,
                            Currency = CITTransaction.Currency.ToUpper()
                        },
                        CreditAccount = new PostBankAccount()
                        {
                            AccountNumber = CITTransaction.SuspenseAccount,
                            Currency = CITTransaction.Currency.ToUpper()
                        }
                    }
                };
                // ISSUE: explicit non-virtual call
                PostCITTransactionResponse coreBankingAsync = await integrationServiceClient.PostCITTransactionAsync(request);
                applicationViewModel.CheckIntegrationResponseMessageDateTime(coreBankingAsync.MessageDateTime);
                return coreBankingAsync;
            }
            catch (Exception ex)
            {
                string ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Log.Error(applicationViewModel.GetType().Name, 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), ErrorDetail);
                PostCITTransactionResponse coreBankingAsync = new();
                Guid guid = applicationViewModel.SessionID.Value;
                coreBankingAsync.SessionID = guid.ToString();
                coreBankingAsync.AppID = device.AppId;
                coreBankingAsync.AppName = device.MachineName;
                coreBankingAsync.MessageDateTime = DateTime.Now;
                guid = Guid.NewGuid();
                coreBankingAsync.MessageID = guid.ToString();
                coreBankingAsync.PostResponseCode = "-1";
                coreBankingAsync.PostResponseMessage = ErrorDetail;
                coreBankingAsync.RequestID = requestID.ToString().ToUpperInvariant();
                coreBankingAsync.ServerErrorCode = "-1";
                coreBankingAsync.ServerErrorMessage = ErrorDetail;
                coreBankingAsync.IsSuccess = false;
                coreBankingAsync.PublicErrorCode = "500";
                coreBankingAsync.PublicErrorMessage = "System error. Contact administrator";
                return coreBankingAsync;
            }

        }

        internal void SetLanguage(string language = null)
        {
            if (language == null)
                language = CurrentSession?.Language;
            try
            {
                if (string.IsNullOrEmpty(language))
                    return;
                IEnumerable<ResourceDictionary> source1 = Application.Current.Resources.MergedDictionaries
                    .Where(x => x.Source != null && x.Source.OriginalString.Contains("Lang_")).ToList();

                //  Application.Current.Resources.MergedDictionaries.Where((Func<ResourceDictionary, bool>)(x => (x?.Source?.ResourceDictionar?.Contains("Lang_"))));
                List<ResourceDictionary> resourceDictionaryList;
                if (source1 == null)
                {
                    resourceDictionaryList = null;
                }
                else
                {
                    IEnumerable<ResourceDictionary> source2 = source1.Where(y =>
                    {
                        if (y == null)
                            return false;
                        Uri source3 = y.Source;
                        int? nullable1;
                        if ((object)source3 == null)
                        {
                            nullable1 = new int?();
                        }
                        else
                        {
                            string originalString = source3.OriginalString;
                            if (originalString == null)
                            {
                                nullable1 = new int?();
                            }
                            else
                            {
                                IEnumerable<char> source4 = originalString.Where(z => z == '.');
                                nullable1 = source4 != null ? new int?(source4.Count()) : new int?();
                            }
                        }
                        int? nullable2 = nullable1;
                        int num = 1;
                        return nullable2.GetValueOrDefault() == num & nullable2.HasValue;
                    });
                    resourceDictionaryList = source2 != null ? source2.ToList() : null;
                }
                List<ResourceDictionary> source5 = resourceDictionaryList;
                foreach (ResourceDictionary resourceDictionary1 in source5)
                {
                    string requestedCulture = string.Format("{0}.{1}.xaml", resourceDictionary1?.Source?.OriginalString.Replace(".xaml", ""), language);
                    ResourceDictionary resourceDictionary2 = Application.Current.Resources.MergedDictionaries.FirstOrDefault(d => string.Equals(d?.Source?.OriginalString, requestedCulture, StringComparison.InvariantCultureIgnoreCase));
                    if (resourceDictionary2 == null)
                    {
                        requestedCulture = resourceDictionary1?.Source?.OriginalString;
                        resourceDictionary2 = source5.FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
                    }
                    if (resourceDictionary2 != null)
                    {
                        Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
                        Application.Current.Resources.MergedDictionaries.Add(resourceDictionary2);
                    }
                }
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                CurrentLanguage = language;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal void SetLanguage(Language value)
        {
            if (CurrentSession == null)
                return;
            CurrentSession.Language = value.Code;
            SetLanguage(value.Code);
        }

        internal static string GetDeviceNumber()
        {
            return ApplicationViewModel.GetDeviceAsync()?.DeviceNumber;
        }

        internal static Device GetDeviceAsync()
        {
            try
            {
                Device device = _deviceRepository.GetDevice(Environment.MachineName).ContinueWith(r => r.Result).Result;
                //Device device = _depositorDBContext.Devices.Where(x => x.MachineName == Environment.MachineName)
                //    .Include(x => x.Branch)
                //     .Include(x => x.ConfigGroupNavigation)
                //     .FirstOrDefault();
                if (device != null)
                    return device;
                Log.Fatal(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Could not get device info from database, terminating");
                throw new InvalidOperationException(string.Format("Device with machine name = {0} does not exists in the local database.", Environment.MachineName));
            }
            catch (Exception ex)
            {
                Log.FatalFormat(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "{0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                throw;
            }
        }


        internal bool InSessionAndTransaction(string callingCode, bool enforceDeviceState = true)
        {
            if (CurrentSession != null)
            {
                if (CurrentTransaction != null)
                    return true;
                if (enforceDeviceState && DeviceManager.CurrentState != DeviceManagerState.NONE)
                {
                    Log.WarningFormat(GetType().Name, nameof(InSessionAndTransaction), "InvalidOperation", "Not in Transaction at {0}. DeviceManager.CurrentState = {1}", callingCode, DeviceManager.CurrentState);
                    DeviceManager.ResetDevice(false);
                }
                return false;
            }
            if (DeviceManager.CurrentState != DeviceManagerState.NONE)
            {
                Log.WarningFormat(GetType().Name, nameof(InSessionAndTransaction), "InvalidOperation", "Not in Session at {0}. DeviceManager.CurrentState = {1}", callingCode, DeviceManager.CurrentState);
                DeviceManager.ResetDevice(false);
            }
            return false;
        }

        internal void ShowController()
        {
            Log.InfoFormat(GetType().Name + ".ShowController()", "Show Controller", "Diagnostics", "Show controller command has been issued by user {0}", CurrentUser?.Username);
            DeviceManager.ShowDeviceController();
        }

        internal void ShutdownPC(ShutdownCommand command, string reason, uint time = 0)
        {
            time = time.Clamp(0U, 600U);
            string str = "";
            switch (command)
            {
                case ShutdownCommand.SHUTDOWN:
                    str = "-s";
                    break;
                case ShutdownCommand.RESTART:
                    str = "-r";
                    break;
                case ShutdownCommand.LOGOFF:
                    str = "-l";
                    break;
            }
            string arguments = string.Format("{0} -f -t {1}  -d p:0:0 -c \"{2}\"", str, time, reason.Substring(0, reason.Length.Clamp(0, 512)));
            Log.InfoFormat(GetType().Name + ".ShutdownPC()", "Running shutdown command", "Diagnostics", "shutdown.exe {0}", arguments);
            Process.Start("shutdown.exe", arguments);
        }

        private void CheckIntegrationResponseMessageDateTime(DateTime MessageDateTime)
        {
            DateTime dateTime1 = DateTime.Now.AddSeconds(DeviceConfiguration.MESSAGEKEEPALIVETIME);
            DateTime dateTime2 = DateTime.Now.AddSeconds(-DeviceConfiguration.MESSAGEKEEPALIVETIME);
            if (!(MessageDateTime < dateTime1) || !(MessageDateTime > dateTime2))
                throw new Exception(string.Format("Invalid MessageDateTime: value {0:yyyy-MM-dd HH:mm:ss.fff} is NOT between {1:yyyy-MM-dd HH:mm:ss.fff} and {2:yyyy-MM-dd HH:mm:ss.fff}", MessageDateTime, dateTime2, dateTime1));
        }
    }
}
