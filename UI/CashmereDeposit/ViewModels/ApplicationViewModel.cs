﻿

using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.UserControls;
using CashmereUtil.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CashAccSysDeviceManager;
using Cashmere.API.Messaging.CDM.GUIControl.AccountsLists;
using Cashmere.API.Messaging.CDM.GUIControl.Clients;
using Cashmere.API.Messaging.Integration.Controllers;
using Cashmere.API.Messaging.Integration.ServerPing;
using Cashmere.API.Messaging.Integration.Transactions;
using Cashmere.API.Messaging.Integration.Validations.AccountNumberValidations;
using Cashmere.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using CashmereDeposit.Interfaces;
using CashmereDeposit.Models;
using CashmereDeposit.Models.Submodule;
using CashmereDeposit.Utils;
using CashmereDeposit.Utils.AlertClasses;
using Activity = Cashmere.Library.CashmereDataAccess.Entities.Activity;

namespace CashmereDeposit.ViewModels
{
    // [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
    public class ApplicationViewModel : Conductor<Screen>, IShell
    {
        private bool _adminMode;
        private ApplicationUser _currentUser;
        private ApplicationUser _validatingUser;
        private CashAccSysDeviceManager.CashAccSysDeviceManager _deviceManager;
        private ApplicationState _currentApplicationState = ApplicationState.STARTUP;
        private DispatcherTimer statusTimer = new DispatcherTimer(DispatcherPriority.Send);

        public bool debugNoDevice { get; }

        public bool debugNoCoreBanking { get; }

        public bool debugDisableSafeSensor { get; }

        public bool debugDisableBagSensor { get; }

        public bool debugDisablePrinter { get; }

        public static CashmereTranslationService CashmereTranslationService { get; set; }

        public object NavigationLock { get; set; } = new object();

        public object BagOpenLock { get; private set; } = new object();

        public object BagReplacedLock { get; private set; } = new object();

        public object DoorClosedLock { get; private set; } = new object();

        internal EscrowJam EscrowJam { get; set; }

        public bool AdminMode
        {
            get { return _adminMode; }
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
            get { return _currentUser; }
            set
            {
                _currentUser = value;
                NotifyOfPropertyChange((Expression<Func<ApplicationUser>>)(() => CurrentUser));
            }
        }

        public ApplicationUser ValidatingUser
        {
            get { return _validatingUser; }
            set
            {
                _validatingUser = value;
                NotifyOfPropertyChange((Expression<Func<ApplicationUser>>)(() => ValidatingUser));
            }
        }

        public ApplicationModel ApplicationModel { get; }

        public AppSession CurrentSession { get; set; }

        public AppTransaction CurrentTransaction
        {
            get { return CurrentSession?.Transaction; }
        }

        public CashAccSysDeviceManager.CashAccSysDeviceManager DeviceManager
        {
            get { return _deviceManager; }
            set { _deviceManager = value; }
        }

        public List<Language> LanguagesAvailable
        {
            get { return ApplicationModel.LanguageList; }
        }

        public List<Currency> CurrenciesAvailable
        {
            get { return ApplicationModel.CurrencyList; }
        }

        public List<TransactionTypeListItem> TransactionTypesAvailable
        {
            get { return ApplicationModel.TransactionList; }
        }

        public ApplicationState CurrentApplicationState
        {
            get { return _currentApplicationState; }
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
                using DepositorDBContext DBContext = new DepositorDBContext();
                Device device = ApplicationModel.GetDevice(DBContext);
                return DBContext.CITs.Where(y => y.DeviceId == device.Id).OrderByDescending(x => x.CITDate).FirstOrDefault();
            }
        }

        public AlertManager AlertManager { get; set; }

        public static DeviceConfiguration DeviceConfiguration { get; set; }

        public LicenseMechanism License { get; private set; }

        private StartupViewModel StartupViewModel { get; }

        public ApplicationViewModel(StartupViewModel startupModel)
        {
            ApplicationStatus.PropertyChanged += new PropertyChangedEventHandler(ApplicationStatus_PropertyChanged);
            StartupViewModel = startupModel;
            Log = new DepositorLogger(this);
            DeviceConfiguration = DeviceConfiguration.Initialise();
            AlertLog = new DepositorLogger(this, "DepositorCommunicationService");
            AlertManager = startupModel.AlertManager;
            AlertManager.Log = Log;
            Log.Info(GetType().Name, "Application Startup", "Constructor", "Initialising Application");
            using DepositorDBContext DBContext = new DepositorDBContext();
            DBContext.Devices.FirstOrDefault(x => x.MachineName == Environment.MachineName);
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
            AlertManager.SendAlert(new AlertDeviceStartupSuccess(ApplicationModel.GetDevice(DBContext), DateTime.Now));
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = false;
            backgroundWorker.DoWork += new DoWorkEventHandler(statusWorker_DoWork);
            backgroundWorker.RunWorkerAsync();
        }

        private void InitialiseLicense()
        {
            using (new DepositorDBContext())
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
        }

        public void InitialiseApp()
        {
            Log.Info(GetType().Name, nameof(InitialiseApp), "Initialisation", "Initialising Application");
            CheckHDDSpace();
            InitialiseLicense();
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
            InitialiseCoreBankingAsync().AsResult();
            CheckDeviceLockStatus();
            CheckHDDSpace();
        }

        private void CheckHDDSpace()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
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
                AlertManager.SendAlert(new AlertHDDFull(availableSpace, minimumSpace, ApplicationModel.GetDevice(DBContext), DateTime.Now));
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
            using DepositorDBContext DBContext = new DepositorDBContext();
            Device device = ApplicationModel.GetDevice(DBContext);
            if (device == null)
                return;
            DeviceManager.Enabled = device.Enabled;
        }

        private async Task InitialiseCoreBankingAsync()
        {
            ApplicationViewModel applicationViewModel = this;
            DepositorDBContext DBContext = new DepositorDBContext();
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
                        Device device = applicationViewModel.ApplicationModel.GetDevice(DBContext);
                        Guid id = device.Id;
                        Guid appId = applicationViewModel.ApplicationModel.GetDevice(DBContext).AppId;
                        IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, appId, device.AppKey, null);
                        IntegrationServerPingRequest request = new IntegrationServerPingRequest();
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
                                    goto label_19;
                                }
                                else
                                    goto label_19;
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
                DBContext?.Dispose();
            }
        label_19:
            DBContext = null;
        }

        public void InitialiseUsersAndPermissions()
        {
            LogoffUsers();
        }

        private void InitialiseEmailManager()
        {
        }

        private void InitialiseFolders()
        {
            Directory.CreateDirectory(DeviceConfiguration.TRANSACTION_LOG_FOLDER);
        }

        private void InitialiseApplicationModel()
        {
            ApplicationModel.InitialiseApplicationModel();
        }

        private void SystemStartupChecks()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Debug(GetType().Name, nameof(SystemStartupChecks), "Initialisation", "Performing startup checks");
            Log.Debug(GetType().Name, "SystemStartupDatabaseCheck", "Initialisation", "Performing database check");
            if (!ApplicationModel.TestConnection())
            {
                Log.Fatal(GetType().Name, 88, ApplicationErrorConst.ERROR_DATABASE_OFFLINE.ToString(), "Could not connect to database during system startup, terminating...");
                OnApplicationStartupFailedEvent(this, ApplicationErrorConst.ERROR_DATABASE_OFFLINE, "Could not connect to database during system startup, terminating...");
            }
            else if (ApplicationModel.GetDevice(DBContext) == null)
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
            using DepositorDBContext DBContext = new DepositorDBContext();
            Device device = ApplicationModel.GetDevice(DBContext);
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
                        SaveToDatabase(DBContext);
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
                EscrowJam escrowJam = DBContext.EscrowJams.OrderByDescending(x => x.DateDetected).FirstOrDefault();
                if (escrowJam == null || (escrowJam.RecoveryDate.HasValue || DeviceManager.DeviceManagerMode == DeviceManagerMode.ESCROW_JAM))
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
                    CurrentTransaction.DroppedDenomination += CurrentTransaction.CountedDenominationResult;
                    CurrentSession.SaveToDatabase();
                }
                AlertManager.SendAlert(new AlertEscrowJam(CurrentTransaction, CurrentSession.Device, DateTime.Now));
                CurrentTransaction.NoteJamDetected = true;
                EndTransaction(ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM, ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM.ToString() + ": Escrow Jam detected. DO NOT Post until after CIT");
            }
            if (sender == this && DeviceManager.DeviceManagerMode != DeviceManagerMode.ESCROW_JAM)
                DeviceManager.OnEscrowJamStartEvent(this, EventArgs.Empty);
            else if (EscrowJam == null)
            {
                EscrowJam escrowJam = new DepositorDBContext().EscrowJams.OrderByDescending(x => x.DateDetected).FirstOrDefault();
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
            GUIScreens = new List<GuiScreen>();
            GUIScreens.AddRange(ApplicationModel.dbGUIScreens);
        }

        private void InitialiseLanguageList()
        {
            ApplicationModel.GenerateLanguageList();
        }

        private void InitialiseCurrencyList()
        {
            ApplicationModel.GenerateCurrencyList();
        }

        private void InitialiseTransactionTypeList()
        {
            ApplicationModel.GenerateTransactionTypeList();
        }

        private void HandleIncompleteTransaction()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Debug(GetType().Name, nameof(HandleIncompleteTransaction), "Transaction", "Checking for and maintaining any incomplete transaction");
            DbSet<Transaction> transactions = DBContext.Transactions;
            Expression<Func<Transaction, bool>> predicate = x => x.TxCompleted == false;
            foreach (Transaction transaction in transactions.Where(predicate).ToList())
            {
                transaction.TxCompleted = true;
                transaction.TxEndDate = new DateTime?(DateTime.Now);
                transaction.TxErrorCode = 85;
                transaction.TxErrorMessage = "Incomplete transaction aborted";
                transaction.TxResult = 85;
            }
            SaveToDatabase(DBContext);
        }

        public void HandleIncompleteSession()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            HandleIncompleteTransaction();
            ApplicationModel.GetDevice(DBContext);
            DbSet<DepositorSession> depositorSessions = DBContext.DepositorSessions;
            Expression<Func<DepositorSession, bool>> predicate = x => x.Complete == false;
            foreach (DepositorSession depositorSession in depositorSessions.Where(predicate).ToList())
            {
                depositorSession.SessionEnd = new DateTime?(DateTime.Now);
                depositorSession.Complete = true;
                depositorSession.CompleteSuccess = false;
                depositorSession.ErrorCode = new int?(84);
                depositorSession.ErrorMessage = "Session is incomplete";
            }
            SaveToDatabase(DBContext);
        }

        private void StartSession()
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            _deviceManager.SetCurrency(ApplicationModel.GetDevice(DBContext).CurrencyList.DefaultCurrencyId.ToUpper());
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

        public List<GuiScreen> GUIScreens { get; set; }

        public GuiScreen CurrentGUIScreen
        {
            get { return GUIScreens.ElementAtOrDefault(CurrentScreenIndex) ?? null; }
        }

        public Screen CurrentScreen { get; set; }

        public Guid? SessionID
        {
            get { return CurrentSession?.SessionID; }
        }

        public void ShowScreen(int screenIndex, bool generateNewScreen = true)
        {
            try
            {
                using DepositorDBContext depositorDbContext = new DepositorDBContext();
                if (screenIndex == 0)
                    InitialiseScreenList();
                if (ApplicationStatus.CashmereDeviceState == CashmereDeviceState.NONE || debugNoDevice)
                {
                    if (generateNewScreen)
                    {
                        CurrentScreenIndex = screenIndex < 0 ? 0 : screenIndex;
                        CurrentScreenIndex = screenIndex > GUIScreens.Count - 1 ? 0 : screenIndex;
                        depositorDbContext.GuiScreens.Attach(GUIScreens[CurrentScreenIndex]);
                        TypeInfo typeInfo = Assembly.GetExecutingAssembly().DefinedTypes.First(x => x.GUID == GUIScreens[CurrentScreenIndex].GuiScreenType.Code);
                        GuiScreen guiScreen = GUIScreens[CurrentScreenIndex];
                        bool? required = depositorDbContext.GuiScreenListScreens.Where(x => x.GuiScreen.Id == guiScreen.Id).FirstOrDefault()?.Required;
                        string str = CashmereTranslationService.TranslateUserText("ShowScreen().screenTitle", CurrentGUIScreen?.GuiScreenText?.ScreenTitleId, "[Translation Error]");
                        var instance = Activator.CreateInstance(typeInfo, str, this, required) as Screen;
                        ActivateItemAsync(instance);
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
                        ActivateItemAsync(CurrentScreen);
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

        public void ShowScreen()
        {
            ShowScreen(CurrentScreenIndex);
        }

        public void ShowScreen(bool genScreen)
        {
            ShowScreen(CurrentScreenIndex, genScreen);
        }

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

        public void ShowDialog(Screen screen)
        {
            if (!AdminMode && ApplicationStatus.CashmereDeviceState != CashmereDeviceState.NONE)
                return;
            ActivateItemAsync(screen);
        }

        public void ShowDialogBox(Screen screen)
        {
            ActivateItemAsync(screen);
        }

        public void ShowErrorDialog(Screen screen)
        {
            Log.Info(GetType().Name, "ShowDialogScreen", "Screen", screen.GetType().Name);
            if (ApplicationStatus.CashmereDeviceState == CashmereDeviceState.NONE)
                return;
            ActivateItemAsync(screen);
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

        public bool? CanCancelTransaction
        {
            get { throw new NotImplementedException(); }
        }

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
            using DepositorDBContext depositorDbContext = new DepositorDBContext();
            depositorDbContext.TransactionTypeListItems.Attach(value);
            Log.Info(GetType().Name, "SetTransactionType", "User Input", value.Name);
            GUIScreens.AddRange(ApplicationModel.GetTransactionTypeScreenList(value).ToList());
            if (CurrentSession.Transaction == null)
                CurrentSession.CreateTransaction(value);
            CurrentTransaction.TransactionType = value;
        }

        internal async Task<AccountsListResponse> SearchAccountListAsync(
          string searchText,
          TransactionTypeListItem txType,
          string currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountsListResponse1;
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.Name);
            AccountsListResponse response = new AccountsListResponse();
            Guid guid = Guid.NewGuid();
            string str1 = guid.ToString();
            if (applicationViewModel.debugNoCoreBanking)
            {
                AccountsListResponse accountsListResponse2 = new AccountsListResponse
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
                        account_number = "1234567890",
                        account_name = "Account1"
                    },
                    new Account()
                    {
                        account_number = "1234567891",
                        account_name = "Account2"
                    },
                    new Account()
                    {
                        account_number = "1234567892",
                        account_name = "Account3"
                    },
                    new Account()
                    {
                        account_number = "1234567893",
                        account_name = "Account4"
                    },
                    new Account()
                    {
                        account_number = "1234567894",
                        account_name = "Account5"
                    },
                    new Account()
                    {
                        account_number = "1234567895",
                        account_name = "Account6"
                    },
                    new Account()
                    {
                        account_number = "1234567896",
                        account_name = "Account7"
                    },
                    new Account()
                    {
                        account_number = "1234567897",
                        account_name = "Account8"
                    },
                    new Account()
                    {
                        account_number = "1234567898",
                        account_name = "Account9"
                    },
                    new Account()
                    {
                        account_number = "1234567899",
                        account_name = "Account10"
                    },
                    new Account()
                    {
                        account_number = "1122334455",
                        account_name = "Account11"
                    },
                    new Account()
                    {
                        account_number = "1112334567",
                        account_name = "Account12"
                    }
                };
                response = accountsListResponse2;
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.Name, currency);
                    Device device = GetDevice(DBContext);
                    DateTime now = DateTime.Now;
                    GUIControlServiceClient controlServiceClient = new GUIControlServiceClient(DeviceConfiguration.API_CDM_GUI_URI, device.AppId, device.AppKey, null);
                    AccountsListRequest accountsListRequest = new AccountsListRequest
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
                            response.Accounts.RemoveAll(p => !p.currency.Equals(currency));
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
          string currency,
          int PageNumber = 0,
          int PageSize = 1000)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountsListResponse accountsListResponse1;
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Info(applicationViewModel.GetType().Name, "GetAccountList", "User Input", txType.Name);
            AccountsListResponse response = new AccountsListResponse();
            Guid guid = Guid.NewGuid();
            string str1 = guid.ToString();
            if (applicationViewModel.debugNoCoreBanking)
            {
                AccountsListResponse accountsListResponse2 = new AccountsListResponse
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
                        account_number = "1234567890",
                        account_name = "Account1"
                    },
                    new Account()
                    {
                        account_number = "1234567891",
                        account_name = "Account2"
                    },
                    new Account()
                    {
                        account_number = "1234567892",
                        account_name = "Account3"
                    },
                    new Account()
                    {
                        account_number = "1234567893",
                        account_name = "Account4"
                    },
                    new Account()
                    {
                        account_number = "1234567894",
                        account_name = "Account5"
                    },
                    new Account()
                    {
                        account_number = "1234567895",
                        account_name = "Account6"
                    },
                    new Account()
                    {
                        account_number = "1234567896",
                        account_name = "Account7"
                    },
                    new Account()
                    {
                        account_number = "1234567897",
                        account_name = "Account8"
                    },
                    new Account()
                    {
                        account_number = "1234567898",
                        account_name = "Account9"
                    },
                    new Account()
                    {
                        account_number = "1234567899",
                        account_name = "Account10"
                    },
                    new Account()
                    {
                        account_number = "1122334455",
                        account_name = "Account11"
                    },
                    new Account()
                    {
                        account_number = "1112334567",
                        account_name = "Account12"
                    }
                };
                response = accountsListResponse2;
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "GetAccountList", "Validation Request", "TxType = {0} Currency = {1}", txType.Name, currency);
                    Device device = GetDevice(DBContext);
                    DateTime now = DateTime.Now;
                    GUIControlServiceClient controlServiceClient = new GUIControlServiceClient(DeviceConfiguration.API_CDM_GUI_URI, device.AppId, device.AppKey, null);
                    AccountsListRequest accountsListRequest = new AccountsListRequest
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
                            response.Accounts.RemoveAll(p => !p.currency.Equals(currency));
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

        internal async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          string accountNumber,
          string currency,
          int txType)
        {
            ApplicationViewModel applicationViewModel = this;
            AccountNumberValidationResponse validationResponse1;
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Info(applicationViewModel.GetType().Name, "ValidateAccountNumber", "User Input", accountNumber);
            AccountNumberValidationResponse response = new AccountNumberValidationResponse();
            if (applicationViewModel.debugNoCoreBanking)
            {
                if (accountNumber == "1234")
                {
                    AccountNumberValidationResponse validationResponse2 = new AccountNumberValidationResponse
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
                    AccountNumberValidationResponse validationResponse2 = new AccountNumberValidationResponse
                    {
                        AccountName = "Test Account",
                        CanTransact = true,
                        MessageDateTime = DateTime.Now
                    };
                    Guid guid = Guid.NewGuid();
                    validationResponse2.RequestID = guid.ToString().ToUpper();
                    guid = Guid.NewGuid();
                    validationResponse2.MessageID = guid.ToString().ToUpper();
                    validationResponse2.IsSuccess = true;
                    validationResponse2.PublicErrorCode = 200.ToString() ?? "";
                    validationResponse2.PublicErrorMessage = "Validated Successfully";
                    response = validationResponse2;
                }
            }
            else
            {
                try
                {
                    Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateAccountNumber", "Validation Request", "Account = {0} Currency = {1}", accountNumber, currency);
                    Device device = GetDevice(DBContext);
                    IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                    AccountNumberValidationRequest request = new AccountNumberValidationRequest
                    {
                        AccountNumber = accountNumber,
                        AppID = device.AppId,
                        AppName = device.MachineName,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        SessionID = applicationViewModel.SessionID.Value.ToString(),
                        DeviceID = device.Id,
                        Currency = currency,
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
                                    if (response?.AccountCurrency?.ToUpper() != currency.ToUpper())
                                    {
                                        Log.InfoFormat(applicationViewModel.GetType().Name, "Transaction", "Cross Currency Not Allowed", "Cannot deposit {2} into {1} Account {0}.", accountNumber, response.AccountCurrency, currency);
                                        response.IsSuccess = false;
                                        response.PublicErrorMessage = string.Format("account indicated is a {0} account kindly enter the correct currency account", response.AccountCurrency);
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
            using (new DepositorDBContext())
            {
                ReferenceAccountNumberValidationResponse response = new ReferenceAccountNumberValidationResponse();
                if (applicationViewModel.debugNoCoreBanking)
                {
                    if (refAccountNumber == "1234")
                    {
                        ReferenceAccountNumberValidationResponse validationResponse2 = new ReferenceAccountNumberValidationResponse
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
                        ReferenceAccountNumberValidationResponse validationResponse2 = new ReferenceAccountNumberValidationResponse
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
                        response = validationResponse2;
                    }
                }
                else
                {
                    try
                    {
                        Log.InfoFormat(applicationViewModel.GetType().Name, "ValidateReferenceAccountNumber", "Validation Request", "Account = {0} Type = {1}", refAccountNumber, applicationViewModel.CurrentTransaction.TransactionType.CbTxType);
                        Device device = applicationViewModel.CurrentSession.Device;
                        IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                        ReferenceAccountNumberValidationRequest request = new ReferenceAccountNumberValidationRequest
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
            }
            return validationResponse1;
        }

        internal void ReferencesAccepted(bool success = true)
        {
            Log.Info(GetType().Name, "VerifyReferences", nameof(ReferencesAccepted), "User accepted the references");
        }

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

        private void DeviceManager_StatusReportEvent(object sender, DeviceStatusChangedEventArgs e)
        {
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                if (device != null)
                {
                    DeviceStatus deviceStatu = DBContext.DeviceStatuses.FirstOrDefault(x => x.DeviceId == device.Id);
                    if (deviceStatu == null)
                    {
                        DBContext.DeviceStatuses.Add(new DeviceStatus()
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
                            BagValueCapacity = new long?(e.ControllerStatus.Bag.ValueCapacity),
                            BagValueLevel = new long?(e.ControllerStatus.Bag.ValueLevel),
                            BaCurrency = e.ControllerStatus.NoteAcceptor.Currency,
                            BaStatus = e.ControllerStatus.NoteAcceptor.Status.ToString(),
                            BaType = e.ControllerStatus.NoteAcceptor.Type.ToString(),
                            ControllerState = e.ControllerStatus.ControllerState.ToString(),
                            CurrentStatus = (int)ApplicationStatus.CashmereDeviceState,
                            EscrowPosition = e.ControllerStatus.Escrow.Position.ToString(),
                            EscrowStatus = e.ControllerStatus.Escrow.Status.ToString(),
                            EscrowType = e.ControllerStatus.Escrow.Type.ToString(),
                            Id = Guid.NewGuid(),
                            Modified = new DateTime?(DateTime.Now),
                            SensorsStatus = e.ControllerStatus.Sensor.Status.ToString(),
                            SensorsType = e.ControllerStatus.Sensor.Type.ToString(),
                            SensorsValue = e.ControllerStatus.Sensor.Value,
                            SensorsBag = e.ControllerStatus.Sensor.Bag.ToString(),
                            SensorsDoor = e.ControllerStatus.Sensor.Door.ToString()
                        });
                    }
                    else
                    {
                        deviceStatu.DeviceId = device.Id;
                        deviceStatu.MachineName = Environment.MachineName.ToUpperInvariant();
                        deviceStatu.TransactionStatus = e.ControllerStatus.Transaction?.Status.ToString();
                        deviceStatu.TransactionType = e.ControllerStatus.Transaction?.Type.ToString();
                        deviceStatu.BagNoteCapacity = e.ControllerStatus.Bag.NoteCapacity.ToString() ?? "";
                        deviceStatu.BagNoteLevel = e.ControllerStatus.Bag.NoteLevel;
                        deviceStatu.BagNumber = e.ControllerStatus.Bag.BagNumber;
                        deviceStatu.BagPercentFull = e.ControllerStatus.Bag.PercentFull;
                        deviceStatu.BagStatus = e.ControllerStatus.Bag.BagState.ToString() ?? "";
                        deviceStatu.BagValueCapacity = new long?(e.ControllerStatus.Bag.ValueCapacity);
                        deviceStatu.BagValueLevel = new long?(e.ControllerStatus.Bag.ValueLevel);
                        deviceStatu.BaCurrency = e.ControllerStatus.NoteAcceptor.Currency;
                        deviceStatu.BaStatus = e.ControllerStatus.NoteAcceptor.Status.ToString();
                        deviceStatu.BaType = e.ControllerStatus.NoteAcceptor.Type.ToString();
                        deviceStatu.ControllerState = e.ControllerStatus.ControllerState.ToString();
                        deviceStatu.CurrentStatus = (int)ApplicationStatus.CashmereDeviceState;
                        deviceStatu.EscrowPosition = e.ControllerStatus.Escrow.Position.ToString();
                        deviceStatu.EscrowStatus = e.ControllerStatus.Escrow.Status.ToString();
                        deviceStatu.EscrowType = e.ControllerStatus.Escrow.Type.ToString();
                        deviceStatu.Modified = new DateTime?(DateTime.Now);
                        deviceStatu.SensorsStatus = e.ControllerStatus.Sensor.Status.ToString();
                        deviceStatu.SensorsType = e.ControllerStatus.Sensor.Type.ToString();
                        deviceStatu.SensorsValue = e.ControllerStatus.Sensor.Value;
                        deviceStatu.SensorsBag = e.ControllerStatus.Sensor.Bag.ToString();
                        deviceStatu.SensorsDoor = e.ControllerStatus.Sensor.Door.ToString();
                    }
                }
                SaveToDatabase(DBContext);
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
            }
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
            using DepositorDBContext DBContext = new DepositorDBContext();
            if (CurrentApplicationState == ApplicationState.CIT_BAG_CLOSED)
            {
                AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                CurrentApplicationState = ApplicationState.CIT_DOOR_OPENED;
                Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_DOOR_OPENED");
            }
            else
            {
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door opened outside of a CIT");
                AlertManager.SendAlert(new AlertSafeDoorOpen(ApplicationModel.GetDevice(DBContext), DateTime.Now));
            }
        }

        private void DeviceManager_DoorClosedEvent(object sender, EventArgs e)
        {
            lock (DoorClosedLock)
            {
                using DepositorDBContext DBContext = new DepositorDBContext();
                Log.Debug(GetType().Name, nameof(DeviceManager_DoorClosedEvent), "EventHandling", "DoorClosedEvent");
                if (CurrentApplicationState == ApplicationState.CIT_BAG_REPLACED)
                {
                    AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                    if (lastCIT == null || lastCIT.Complete)
                        return;
                    CurrentApplicationState = ApplicationState.CIT_DOOR_CLOSED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REPLACED");
                    EndCIT();
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "door closed outside of a CIT");
                    AlertManager.SendAlert(new AlertSafeDoorClosed(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                }
            }
        }

        private void DeviceManager_BagRemovedEvent(object sender, EventArgs e)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Debug(GetType().Name, nameof(DeviceManager_BagRemovedEvent), "EventHandling", "BagRemovedEvent");
            if (CurrentApplicationState == ApplicationState.CIT_DOOR_OPENED)
            {
                CurrentApplicationState = ApplicationState.CIT_BAG_REMOVED;
                Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
            }
            else
            {
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag removed outside of a CIT");
                AlertManager.SendAlert(new AlertBagRemoved(ApplicationModel.GetDevice(DBContext), DateTime.Now));
            }
        }

        private void DeviceManager_BagPresentEvent(object sender, EventArgs e)
        {
            lock (BagReplacedLock)
            {
                using DepositorDBContext DBContext = new DepositorDBContext();
                if (lastCIT != null && !lastCIT.Complete)
                {
                    CurrentApplicationState = ApplicationState.CIT_BAG_REPLACED;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_REMOVED");
                    AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDevice(DBContext), DateTime.Now, true));
                }
                else
                {
                    Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag inserted outside of a CIT");
                    AlertManager.SendAlert(new AlertBagInserted(ApplicationModel.GetDevice(DBContext), DateTime.Now));
                }
            }
        }

        private void DeviceManager_BagOpenedEvent(object sender, EventArgs e)
        {
            lock (BagOpenLock)
            {
                using DepositorDBContext DBContext = new DepositorDBContext();
                CIT last_CIT = DBContext.CITs.Include(y => y.StartUser).Include(z => z.AuthorisingUser).FirstOrDefault(x => x.Id == lastCIT.Id);
                Device device = ApplicationModel.GetDevice(DBContext);
                Log.Debug(GetType().Name, nameof(DeviceManager_BagOpenedEvent), "EventHandling", "BagOpenedEvent");
                if (last_CIT != null && !last_CIT.Complete)
                {
                    CurrentApplicationState = ApplicationState.CIT_END;
                    Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_END");
                    if (last_CIT != null)
                    {
                        last_CIT.Complete = true;
                        last_CIT.CITCompleteDate = new DateTime?(DateTime.Now);
                        SaveToDatabase(DBContext);
                        Task.Run((Func<Task>)(() => PostCITTransactionsAsync(last_CIT, DBContext)));
                        DbSet<CIT> ciTs = DBContext.CITs;
                        Expression<Func<CIT, bool>> predicate = x => x.Id != last_CIT.Id && x.DeviceId == device.Id && x.Complete == false;
                        foreach (CIT cit in ciTs.Where(predicate).ToList())
                        {
                            cit.Complete = true;
                            cit.CITCompleteDate = new DateTime?(DateTime.Now);
                            cit.CITError = 95;
                            cit.CITErrorMessage = "Incomplete CIT completed by a newer CIT";
                            AlertManager.SendAlert(new AlertCITFailed(cit, device, DateTime.Now));
                        }
                    }
                    SaveToDatabase(DBContext);
                    AlertManager.SendAlert(new AlertCITSuccess(last_CIT, device, DateTime.Now));
                    InitialiseApp();
                    DeviceManager.DeviceManagerMode = DeviceManagerMode.NONE;
                }
                if (last_CIT != null && last_CIT.Complete)
                    return;
                Log.Warning(GetType().Name, "ApplicationState", "EventHandling", "bag opened outside of a CIT");
            }
        }

        private async Task PostCITTransactionsAsync(CIT cit, DepositorDBContext DBContext)
        {
            if (DeviceConfiguration.CIT_ALLOW_POST)
            {
                try
                {
                    if (cit == null)
                        throw new NullReferenceException("null CIT from DB");
                    foreach (CITTransaction citTransaction in cit.CITTransactions)
                    {
                        CITTransaction CITTransaction = citTransaction;
                        if (CITTransaction.Amount > 0L)
                        {
                            if (CITTransaction.CIT.Device.GetCITSuspenseAccount(CITTransaction.Currency) != null)
                            {
                                Log.InfoFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Posting CITTransaction id={0}, account={1}, suspense={2}, currency={3}, amount={4:#,##0.##}", CITTransaction.Id, CITTransaction.AccountNumber, CITTransaction.SuspenseAccount, CITTransaction.Currency, CITTransaction.Amount / 100.0);
                                PostCITTransactionResponse coreBankingAsync = await PostCITTransactionToCoreBankingAsync(cit.Id, CITTransaction);
                                CITTransaction.CbDate = new DateTime?(coreBankingAsync.TransactionDateTime);
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
                                Log.WarningFormat(nameof(ApplicationViewModel), "Posting CITTransaction", "StartCIT", "Error posting CITTransaction id={0}, no CITSuspenseAccount for currency {1}", CITTransaction.Id, CITTransaction.Currency);
                        }
                        else
                            Log.Warning(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "Skipping CITPost on zero count");
                        SaveToDatabase(DBContext);
                        CITTransaction = null;
                    }
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("ApplicationViewModel.StartCIT", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", lastCIT.Id, ex.MessageString());
                }
            }
            else
                Log.Info(nameof(ApplicationViewModel), "CIT_ALLOW_POST", nameof(PostCITTransactionsAsync), "Not allowed by config");
            SaveToDatabase(DBContext);
            Log.Trace(nameof(ApplicationViewModel), "CITPost", nameof(PostCITTransactionsAsync), "End of Function");
        }

        private void DeviceManager_BagClosedEvent(object sender, EventArgs e)
        {
            Log.Info(GetType().Name, nameof(DeviceManager_BagClosedEvent), "EventHandling", "BagClosedEvent");
        }

        private void DeviceManager_BagFullAlertEvent(object sender, ControllerStatus e)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullAlertEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
            AlertManager.SendAlert(new AlertBagFull(ApplicationModel.GetDevice(DBContext), DateTime.Now, e.Bag));
        }

        private void DeviceManager_BagFullWarningEvent(object sender, ControllerStatus e)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.WarningFormat(GetType().Name, nameof(DeviceManager_BagFullWarningEvent), "EventHandling", "Percentage={0}%", e.Bag.PercentFull);
            AlertManager.SendAlert(new AlertBagFullWarning(ApplicationModel.GetDevice(DBContext), DateTime.Now, e.Bag));
        }

        private void CurrentSession_TransactionLimitReachedEvent(object sender, EventArgs e)
        {
        }

        private void ApplicationStatus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
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
            ParameterExpression parameterExpression1;
            // ISSUE: method reference
            // ISSUE: method reference
            // ISSUE: method reference
            DeviceStatus deviceStatu = DBContext.DeviceStatuses.FirstOrDefault(x => x.MachineName == Environment.MachineName);
            if (deviceStatu == null)
            {
                ParameterExpression parameterExpression2;
                // ISSUE: method reference
                // ISSUE: method reference
                // ISSUE: method reference
                deviceStatu = CashmereDepositCommonClasses.GenerateDeviceStatus(DBContext.Devices.FirstOrDefault(x => x.MachineName == Environment.MachineName).Id, DBContext);
            }
            deviceStatu.CurrentStatus = (int)ApplicationStatus.CashmereDeviceState;
            deviceStatu.Modified = new DateTime?(DateTime.Now);
            SaveToDatabase(DBContext);
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

        internal void DebugOnTransactionStartedEvent(object sender, DeviceTransaction e)
        {
            DeviceManager_TransactionStartedEvent(sender, e);
        }

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
                        int num2;
                        if (currentTransaction2 == null)
                        {
                            num2 = 0;
                        }
                        else
                        {
                            bool? countingStarted = currentTransaction2.Session?.CountingStarted;
                            bool flag = true;
                            num2 = countingStarted.GetValueOrDefault() == flag & countingStarted.HasValue ? 1 : 0;
                        }
                        if (num2 != 0)
                        {
                            CurrentSession.HasCounted = true;
                            if (e.level == ErrorLevel.SUCCESS)
                            {
                                AppTransaction currentTransaction3 = CurrentTransaction;
                                TransactionStatusResponseResult TransactionResult = new TransactionStatusResponseResult
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

        private void DeviceManager_CashInStartedEvent(object sender, DeviceTransactionResult e)
        {
            Count();
        }

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
                        int num2;
                        if (currentTransaction2 == null)
                        {
                            num2 = 1;
                        }
                        else
                        {
                            bool? countingEnded = currentTransaction2.Session?.CountingEnded;
                            bool flag = true;
                            num2 = !(countingEnded.GetValueOrDefault() == flag & countingEnded.HasValue) ? 1 : 0;
                        }
                        if (num2 != 0)
                        {
                            CurrentTransaction.Session.CountingEnded = true;
                            CurrentSession.HasCounted = true;
                            if (e.level == ErrorLevel.SUCCESS)
                            {
                                if (CurrentTransaction != null)
                                {
                                    AppTransaction currentTransaction3 = CurrentTransaction;
                                    TransactionStatusResponseResult TransactionResult = new TransactionStatusResponseResult
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

        private void DeviceManager_CITResultEvent(object sender, CITResult e)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Debug(GetType().Name, "OnCITResultEvent", "EventHandling", "CIT Result");
            if (CurrentApplicationState != ApplicationState.CIT_START || e.level != ErrorLevel.SUCCESS)
                return;
            CurrentApplicationState = ApplicationState.CIT_BAG_CLOSED;
            Log.Info(GetType().Name, "ApplicationState", "EventHandling", "CIT_BAG_CLOSED");
            PrintCITReceipt(DBContext.CITs.FirstOrDefault(x => x.Id == lastCIT.Id), DBContext);
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
            using (DepositorDBContext DBContext = new DepositorDBContext())
            {
                Device device = ApplicationModel.GetDevice(DBContext);
                if (AlertManager != null)
                {
                    if (device != null)
                        AlertManager.SendAlert(new AlertDeviceStartupFailed(string.Format("{0:G}: {1}", e, errorMessage), device, DateTime.Now));
                }
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
                StringResult e = new StringResult
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

        public bool CanCount
        {
            get { return DeviceManager.CanCount && InSessionAndTransaction("ApplicationViewModel.CanCount()", false); }
        }

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

        public bool CanEndCount
        {
            get
            {
                return InSessionAndTransaction("ApplicationViewModel.CanEndCount()", false) &&
                       DeviceManager.CanEndCount;
            }
        }

        internal void EscrowDrop()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowDrop()");
            if (debugNoDevice)
                throw new NotImplementedException("Debug EscrowDrop() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowDrop()"))
                return;
            DeviceManager.EscrowDrop();
        }

        public bool CanEscrowReject
        {
            get
            {
                return InSessionAndTransaction("ApplicationViewModel.CanEscrowReject()", false) &&
                       DeviceManager.CanEscrowReject;
            }
        }

        internal void EscrowReject()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "EscrowReject()");
            if (debugNoDevice)
                throw new NotImplementedException("Debug EscrowReject() not implemented");
            if (!InSessionAndTransaction(GetType().Name + ".EscrowReject()"))
                return;
            DeviceManager.EscrowReject();
        }

        public bool CanTransactionEnd
        {
            get
            {
                return InSessionAndTransaction("ApplicationViewModel.CanTransactionEnd", false) &&
                       DeviceManager.CanTransactionEnd;
            }
        }

        internal void DeviceTransactionEnd()
        {
            Log.Info(GetType().Name, "Count", "DeviceTransactionEnd()", "DeviceTransactionEnd()");
            DeviceManager.TransactionEnd();
        }

        internal void ValidateTransactedAmount()
        {
            Log.Info(GetType().Name, "Count", "Device Management", "ValidateTransactedAmount()");
            using DepositorDBContext DBContext = new DepositorDBContext();
            if (CurrentTransaction == null)
                return;
            var transactionLimits1 = CurrentTransaction.TransactionLimits;
            long? nullable1;
            if ((transactionLimits1 != null ? (transactionLimits1.ShowFundsSource ? 1 : 0) : 0) != 0)
            {
                Decimal droppedDisplayAmount = CurrentTransaction.DroppedDisplayAmount;
                nullable1 = CurrentTransaction.TransactionLimits?.FundsSourceAmount;
                Decimal? nullable2 = nullable1.HasValue ? new Decimal?(nullable1.GetValueOrDefault()) : new Decimal?();
                Decimal valueOrDefault = nullable2.GetValueOrDefault();
                if (droppedDisplayAmount > valueOrDefault & nullable2.HasValue)
                {
                    DepositorLogger log = Log;
                    string name = GetType().Name;
                    object[] objArray = new object[2]
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
                        nullable3 = new long?(transactionLimits2.FundsSourceAmount);
                    objArray[1] = nullable3;
                    log.InfoFormat(name, "Count", "Device Management", "Transaction of {0} is above value limit of {1}, insert FundsSource screen", objArray);
                    ShowFundsSourceScreen(DBContext);
                    return;
                }
            }
            var transactionLimits3 = CurrentTransaction.TransactionLimits;
            if ((transactionLimits3 != null ? (transactionLimits3.PreventOverdeposit ? 1 : 0) : 0) != 0 && CurrentTransaction.IsOverDeposit)
            {
                string title = CashmereTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Title", "Transaction Limit Exceeded");
                string format = CashmereTranslationService?.TranslateSystemText(GetType().Name + ".PerformSelection", "sys_Transaction_Limit_Exceeded_Message", "Transaction limit of {0} {1} has been exceeded. Please visit your nearest branch to finish the deposit.");
                string upper = CurrentTransaction.CurrencyCode.ToUpper();
                var transactionLimits2 = CurrentTransaction.TransactionLimits;
                long? nullable2;
                if (transactionLimits2 == null)
                {
                    nullable1 = new long?();
                    nullable2 = nullable1;
                }
                else
                    nullable2 = new long?(transactionLimits2.OverdepositAmount);
                // ISSUE: variable of a boxed type
                long? local = nullable2;
                string message = string.Format(format, upper, (object)local);
                ShowUserMessageScreen(title, message);
                DepositorLogger log = Log;
                string name = GetType().Name;
                string ErrorName = ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED.ToString();
                object[] objArray1 = new object[5];
                Guid id = CurrentTransaction.Transaction.Id;
                objArray1[0] = id.ToString().ToUpper();
                objArray1[1] = CurrentTransaction?.CurrencyCode?.ToUpper();
                objArray1[2] = CurrentTransaction?.DroppedDisplayAmount;
                objArray1[3] = CurrentTransaction?.TransactionType?.DefaultAccountCurrencyId?.ToUpper();
                var transactionLimits4 = CurrentTransaction.TransactionLimits;
                long? nullable3;
                if (transactionLimits4 == null)
                {
                    nullable1 = new long?();
                    nullable3 = nullable1;
                }
                else
                    nullable3 = new long?(transactionLimits4.OverdepositAmount);
                objArray1[4] = nullable3;
                log.ErrorFormat(name, 27, ErrorName, "Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray1);
                object[] objArray2 = new object[5];
                id = CurrentTransaction.Transaction.Id;
                objArray2[0] = id.ToString().ToUpper();
                objArray2[1] = CurrentTransaction?.CurrencyCode?.ToUpper();
                objArray2[2] = CurrentTransaction?.DroppedDisplayAmount;
                objArray2[3] = CurrentTransaction?.TransactionType?.DefaultAccountCurrencyId?.ToUpper();
                var transactionLimits5 = CurrentTransaction.TransactionLimits;
                long? nullable4;
                if (transactionLimits5 == null)
                {
                    nullable1 = new long?();
                    nullable4 = nullable1;
                }
                else
                    nullable4 = new long?(transactionLimits5.OverdepositAmount);
                objArray2[4] = nullable4;
                EndTransaction(ApplicationErrorConst.ERROR_TRANSACTION_LIMIT_EXCEEDED, string.Format("Credit Blocked: Transaction [{0}] of Amount {1} {2:###,##0.00} is over the limit of {3} {4:###,##0.00}.", objArray2));
            }
            else
            {
                AppTransaction currentTransaction = CurrentTransaction;
                if ((currentTransaction != null ? (currentTransaction.NoteJamDetected ? 1 : 0) : 0) != 0)
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

        internal void ShowFundsSourceScreen(DepositorDBContext DBContext)
        {
            if ((bool)!CurrentTransaction?.TransactionLimits?.ShowFundsSource)
                return;

            GuiScreen guiScreen = DBContext.GuiScreens.Where(x => x.GuiScreenType.Code == new Guid("33EC330E-FB51-4626-906D-1A3F77AAA5E2")).OrderBy(y => y.Id).FirstOrDefault();
            if (guiScreen != null)
            {
                GUIScreens.Insert(CurrentScreenIndex + 1, guiScreen);
                NavigateNextScreen();
            }
            else
            {
                Log.Error(GetType().Name, 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "cannot find FundsSourceScreen in database");
                EndTransaction(ApplicationErrorConst.ERROR_DATABASE_GENERAL, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString());
            }
        }

        internal void ShowUserMessageScreen(string title, string message, bool required = false)
        {
            ActivateItemAsync(
                new UserMessageScreenViewModel(title, this, DeviceConfiguration.USER_SCREEN_TIMEOUT, required)
                {
                    Message = message
                });
        }

        internal void EndTransaction(ApplicationErrorConst result = ApplicationErrorConst.ERROR_NONE, string ErrorMessage = null)
        {
            Log.InfoFormat(GetType().Name, nameof(EndTransaction), "Device Management", "EndTransaction with result {0}", result.ToString());
            if (CurrentTransaction == null)
                return;
            if (result == ApplicationErrorConst.ERROR_NONE || result == ApplicationErrorConst.ERROR_DEVICE_NOTEJAM && DeviceConfiguration.POST_ON_NOTEJAM || (result == ApplicationErrorConst.ERROR_DEVICE_ESCROWJAM && DeviceConfiguration.POST_ON_ESCROWJAM || result == ApplicationErrorConst.WARN_DEPOSIT_TIMEOUT))
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
                                    Log.InfoFormat(GetType().Name, "Attempt", "Posting", "Transaction [{0}] of Amount {1} {2:###,##0.00} is NOT over the limit of {3} {4:###,##0.00} or overdeposit is disabled.", CurrentTransaction.Transaction.Id.ToString().ToUpper(), CurrentTransaction?.CurrencyCode?.ToUpper(), CurrentTransaction?.DroppedDisplayAmount, CurrentTransaction?.TransactionType?.DefaultAccountCurrencyId?.ToUpper(), CurrentTransaction.TransactionLimits?.OverdepositAmount);
                                    CurrentTransaction.isPosting = true;
                                    CurrentTransaction.hasPosted = true;
                                    PostTransactionResponse result1 = Task.Run((Func<Task<PostTransactionResponse>>)(() => PostToCoreBankingAsync(SessionID.Value, CurrentTransaction.Transaction))).Result;
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
                    Log.Warning(GetType().Name, "Skip Post on Zero Count", "Posting", "Posting skipped on account of zero amount counted");
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
                CITResult e = new CITResult
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
                            denominationItems = new List<DenominationItem>()
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
            using (new DepositorDBContext())
            {
                Log.InfoFormat(GetType().Name, nameof(EndCIT), "Device Management", "Bag Number = {0}", lastCIT.NewBagNumber);
                _deviceManager.EndCIT(lastCIT.NewBagNumber);
            }
        }

        internal void LockDevice(bool lockedByDevice, ApplicationErrorConst error, string errorMessage)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Device device = ApplicationModel.GetDevice(DBContext);
            Log.WarningFormat(GetType().Name, nameof(LockDevice), "Device Lock", "LockedByDevice = {0}: Error: {1}>>{2}", lockedByDevice ? "TRUE" : (object)"FALSE", error.ToString(), errorMessage);
            device.Enabled = false;
            Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));");
            AlertManager.SendAlert(new AlertDeviceLocked(errorMessage, device, DateTime.Now));
            Log.Debug(GetType().Name, nameof(LockDevice), "Device Lock", "DBContext.DeviceLocks.Add(new DeviceLock");
            DBContext.DeviceLocks.Add(new DeviceLock()
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Locked = true,
                LockingUser = CurrentUser?.Id,
                LockDate = DateTime.Now,
                LockedByDevice = lockedByDevice,
                WebLockingUser = null
            });
            SaveToDatabase(DBContext);
            SetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);
        }

        internal void UnLockDevice(bool lockedByDevice, string lockMessage = null)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Device device = ApplicationModel.GetDevice(DBContext);
            Log.Info(GetType().Name, nameof(UnLockDevice), "Device Lock", lockMessage);
            device.Enabled = true;
            AlertManager.SendAlert(new AlertDeviceUnLocked(lockMessage, device, DateTime.Now));
            DBContext.DeviceLocks.Add(new DeviceLock()
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Locked = false,
                LockingUser = CurrentUser?.Id,
                LockDate = DateTime.Now,
                LockedByDevice = lockedByDevice,
                WebLockingUser = null
            });
            SaveToDatabase(DBContext);
            if (!ApplicationStatus.CashmereDeviceState.HasFlag(CashmereDeviceState.DEVICE_LOCK))
                return;
            UnSetCashmereDeviceState(CashmereDeviceState.DEVICE_LOCK);
        }

        internal void ClearEscrowJam()
        {
            DeviceManager.ClearEscrowJam();
        }

        internal void EndEscrowJam()
        {
            DeviceManager.EndEscrowJam();
        }

        internal void LogoffUsers(bool forced = false)
        {
            try
            {
                using DepositorDBContext DBContext = new DepositorDBContext();
                if (CurrentUser != null)
                {
                    DeviceLogin deviceLogin = DBContext.DeviceLogins.Where(x => x.UserId == CurrentUser.Id && x.Success == true).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                    if (deviceLogin != null)
                    {
                        deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                        deviceLogin.ForcedLogout = new bool?(forced);
                    }
                }
                if (ValidatingUser != null)
                {
                    DeviceLogin deviceLogin = DBContext.DeviceLogins.Where(x => x.UserId == ValidatingUser.Id && x.Success == true).OrderByDescending(x => x.LoginDate).FirstOrDefault();
                    if (deviceLogin != null)
                    {
                        deviceLogin.LogoutDate = new DateTime?(DateTime.Now);
                        deviceLogin.ForcedLogout = new bool?(forced);
                    }
                }
                SaveToDatabase(DBContext);
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

        internal void LockUser(
          ApplicationUser user,
          bool lockedByDevice,
          ApplicationErrorConst error,
          string errorMessage)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.WarningFormat(GetType().Name, nameof(LockUser), "User Locked by device", "Error {0} {1} {2}", error, error.ToString(), errorMessage);
            ApplicationUser applicationUser = DBContext.ApplicationUsers.First(x => x.Id == user.Id);
            applicationUser.DepositorEnabled = new bool?(false);
            applicationUser.IsActive = new bool?(false);
            AlertManager.SendAlert(new AlertUserLocked(user, ApplicationModel.GetDevice(DBContext), errorMessage, DateTime.Now));
            DBContext.UserLocks.Add(new UserLock()
            {
                Id = Guid.NewGuid(),
                ApplicationUserLoginDetailId = user.ApplicationUserLoginDetailId,
                LockType = new int?(0),
                InitiatingUserId = CurrentUser?.Id,
                LogDate = new DateTime?(DateTime.Now),
                WebPortalInitiated = new bool?(false)
            });
            SaveToDatabase(DBContext);
        }

        internal void UnLockUser(ApplicationUser user, bool lockedByUser, string lockMessage = null)
        {
            using DepositorDBContext DBContext = new DepositorDBContext();
            Log.Info(GetType().Name, nameof(UnLockUser), "User Lock", lockMessage);
            ApplicationUser applicationUser = DBContext.ApplicationUsers.First(x => x.Id == user.Id);
            applicationUser.DepositorEnabled = new bool?(true);
            applicationUser.IsActive = new bool?(true);
            AlertManager.SendAlert(new AlertUserUnLocked(user, ApplicationModel.GetDevice(DBContext), lockMessage, DateTime.Now));
            DBContext.UserLocks.Add(new UserLock()
            {
                Id = Guid.NewGuid(),
                ApplicationUserLoginDetailId = user.ApplicationUserLoginDetailId,
                LockType = new int?(1),
                InitiatingUserId = CurrentUser?.Id,
                LogDate = new DateTime?(DateTime.Now),
                WebPortalInitiated = new bool?(false)
            });
            SaveToDatabase(DBContext);
        }

        internal bool UserPermissionAllowed(
          ApplicationUser currentUser,
          string activityString,
          bool isAuthorising = false)
        {
            using (new DepositorDBContext())
            {
                try
                {
                    return GetUserPermission(currentUser, activityString, isAuthorising) != null;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        internal Permission GetUserPermission(
          ApplicationUser user,
          string activity,
          bool isAuthenticating = false)
        {
            if (user == null)
                return null;
            if (string.IsNullOrWhiteSpace(activity))
                return null;
            using DepositorDBContext depositorDbContext = new DepositorDBContext();
            Activity Activity = depositorDbContext.Activities.FirstOrDefault(x => x.Name.Equals(activity, StringComparison.InvariantCultureIgnoreCase));
            if (Activity == null)
                return null;
            return depositorDbContext.Permissions.FirstOrDefault(x => x.RoleId == user.RoleId && x.ActivityId == Activity.Id && (isAuthenticating ? x.StandaloneCanAuthenticate : x.StandaloneAllowed));
        }

        internal void TimeoutSession(string screen, double timeout, string message = null)
        {
            Log.InfoFormat(GetType().Name, nameof(TimeoutSession), "Session", "Screen {0} has timed out after {1:0.###} seconds with message {2}", screen, timeout, message);
            AlertManager.SendAlert(new AlertTransactionTimedout(CurrentTransaction, ApplicationModel.GetDevice(new DepositorDBContext()), DateTime.Now));
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
                long transactionLimitCents = (long)(CurrentTransaction?.TransactionType?.TxLimitList?.Get_prevent_overdeposit(CurrentTransaction.Currency) != null ? CurrentTransaction?.TransactionType?.TxLimitList?.Get_overdeposit_amount(CurrentTransaction.Currency) : long.MaxValue);
                AppTransaction currentTransaction = CurrentTransaction;
                long transactionValueCents = currentTransaction != null ? currentTransaction.TransactionValue : 0L;
                DeviceTransactionStart(transactionLimitCents, transactionValueCents);
            }
            else
                Log.Warning(GetType().Name, nameof(StartCountingProcess), "InvalidOperation", "Invalid state change requested");
        }

        public void PrintReceipt(Transaction transaction, bool reprint = false, DepositorDBContext txDBContext = null)
        {
            if (debugDisablePrinter)
                return;
            using DepositorDBContext depositorDbContext = new DepositorDBContext();
            txDBContext = txDBContext ?? depositorDbContext;
            Log.InfoFormat(GetType().Name, nameof(PrintReceipt), "Commands", "Transaction code = {0}, Reprint = {1}", transaction.Id, reprint);
            Printer.PrintTransaction(transaction, txDBContext, reprint);
        }

        public void PrintCITReceipt(CIT cit, DepositorDBContext DBContext, bool reprint = false)
        {
            if (debugDisablePrinter)
                return;
            Log.InfoFormat(GetType().Name, nameof(PrintCITReceipt), "Commands", "CIT code = {0}, Reprint = {1}", cit.Id, reprint);
            Printer.PrintCIT(cit, DBContext, reprint);
        }

        public async Task<PostTransactionResponse> PostToCoreBankingAsync(
          Guid requestID,
          Transaction transaction)
        {
            ApplicationViewModel applicationViewModel = this;
            using DepositorDBContext DBContext = new DepositorDBContext();
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
                PostTransactionResponse transactionResponse = new PostTransactionResponse();
                Guid guid = Guid.NewGuid();
                transactionResponse.MessageID = guid.ToString().ToUpper();
                guid = Guid.NewGuid();
                transactionResponse.RequestID = guid.ToString().ToUpper();
                transactionResponse.PostResponseCode = 200.ToString() ?? "";
                transactionResponse.PostResponseMessage = "Posted";
                transactionResponse.MessageDateTime = DateTime.Now;
                transactionResponse.IsSuccess = true;
                transactionResponse.TransactionDateTime = DateTime.Now;
                guid = Guid.NewGuid();
                transactionResponse.TransactionID = guid.ToString().ToUpper();
                return transactionResponse;
            }
            try
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting transaction {0}", transaction.ToString());
                TransactionTypeListItem transactionTypeListItem = DBContext.TransactionTypeListItems.FirstOrDefault(x => x.Id == transaction.TxType.Value);
                ApplicationModel applicationModel = applicationViewModel.ApplicationModel;
                string str1;
                if (applicationModel == null)
                {
                    str1 = null;
                }
                else
                {
                    ICollection<DeviceSuspenseAccount> suspenseAccounts = applicationModel.GetDevice(DBContext).DeviceCITSuspenseAccounts;
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
                long num2 = 100;
                objArray[3] = txAmount.HasValue ? new long?(txAmount.GetValueOrDefault() / num2) : new long?();
                objArray[4] = str2;
                log.InfoFormat(name, "PostToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", objArray);
                Device device = applicationViewModel.CurrentSession.Device;
                IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                Guid guid = Guid.NewGuid();
                guid = applicationViewModel.SessionID.Value;

                PostTransactionRequest request = new PostTransactionRequest
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
                PostTransactionData postTransactionData = new PostTransactionData
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
                PostTransactionResponse transactionResponse = await (integrationServiceClient.PostTransactionAsync(request));
                applicationViewModel.CheckIntegrationResponseMessageDateTime(transactionResponse.MessageDateTime);
                return transactionResponse;
            }
            catch (Exception ex)
            {
                string ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Log.Error(applicationViewModel.GetType().Name, 91, ApplicationErrorConst.ERROR_TRANSACTION_POST_FAILURE.ToString(), ErrorDetail);
                PostTransactionResponse transactionResponse = new PostTransactionResponse
                {
                    MessageDateTime = DateTime.Now,
                    PostResponseCode = "-1",
                    PostResponseMessage = ErrorDetail,
                    RequestID = requestID.ToString().ToUpperInvariant(),
                    ServerErrorCode = "-1",
                    ServerErrorMessage = ErrorDetail,
                    IsSuccess = false
                };
                return transactionResponse;
            }
        }

        public async Task<PostCITTransactionResponse> PostCITTransactionToCoreBankingAsync(
          Guid requestID,
          CITTransaction CITTransaction)
        {
            ApplicationViewModel applicationViewModel = this;
            using DepositorDBContext DBContext = new DepositorDBContext();
            if (applicationViewModel.debugNoCoreBanking)
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "PostCITTransactionToCoreBanking", "Commands", "DebugPosting: RequestID = {0}, AccountNumber = {1}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.AccountNumber, CITTransaction.Currency, CITTransaction.Amount / 100L);
                PostCITTransactionResponse transactionResponse = new PostCITTransactionResponse
                {
                    MessageID = Guid.NewGuid().ToString().ToUpper(),
                    RequestID = Guid.NewGuid().ToString().ToUpper(),
                    PostResponseCode = 200.ToString() ?? "",
                    PostResponseMessage = "Posted",
                    MessageDateTime = DateTime.Now,
                    IsSuccess = true
                };
                return transactionResponse;
            }
            Device device = GetDevice(DBContext);
            try
            {
                Log.InfoFormat(applicationViewModel.GetType().Name, "Posting to live core banking", "Integation", "posting CITTransaction {0}", CITTransaction.ToString());
                Log.InfoFormat(applicationViewModel.GetType().Name, "PostCITTransactionToCoreBanking", "Commands", "RequestID = {0}, AccountNumber = {1}, Suspense Account {4}, Currency = {2}, Amount = {3:N2}", requestID, CITTransaction.AccountNumber, CITTransaction.Currency, CITTransaction.Amount / 100L, CITTransaction.SuspenseAccount);
                IntegrationServiceClient integrationServiceClient = new IntegrationServiceClient(DeviceConfiguration.API_INTEGRATION_URI, device.AppId, device.AppKey, null);
                PostCITTransactionRequest request = new PostCITTransactionRequest
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
                PostCITTransactionResponse transactionResponse = await (integrationServiceClient.PostCITTransactionAsync(request));
                applicationViewModel.CheckIntegrationResponseMessageDateTime(transactionResponse.MessageDateTime);
                return transactionResponse;
            }
            catch (Exception ex)
            {
                string ErrorDetail = string.Format("Post failed with error: {0}>>{1}>>{2}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message);
                Log.Error(applicationViewModel.GetType().Name, 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), ErrorDetail);
                PostCITTransactionResponse transactionResponse = new PostCITTransactionResponse();
                Guid guid = applicationViewModel.SessionID.Value;
                transactionResponse.SessionID = guid.ToString();
                transactionResponse.AppID = device.AppId;
                transactionResponse.AppName = device.MachineName;
                transactionResponse.MessageDateTime = DateTime.Now;
                guid = Guid.NewGuid();
                transactionResponse.MessageID = guid.ToString();
                transactionResponse.PostResponseCode = "-1";
                transactionResponse.PostResponseMessage = ErrorDetail;
                transactionResponse.RequestID = requestID.ToString().ToUpperInvariant();
                transactionResponse.ServerErrorCode = "-1";
                transactionResponse.ServerErrorMessage = ErrorDetail;
                transactionResponse.IsSuccess = false;
                transactionResponse.PublicErrorCode = "500";
                transactionResponse.PublicErrorMessage = "System error. Contact administrator";
                return transactionResponse;
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
                IEnumerable<ResourceDictionary> source1 = Application.Current.Resources.MergedDictionaries.Where<ResourceDictionary>(x =>
                {
                    bool? nullable;
                    if (x == null)
                    {
                        nullable = new bool?();
                    }
                    else
                    {
                        Uri source = x.Source;
                        nullable = (object)source != null ? source.OriginalString?.Contains("Lang_") : new bool?();
                    }
                    return nullable.GetValueOrDefault();
                });
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
                        Uri source1 = y.Source;
                        int? nullable1;
                        if ((object)source1 == null)
                        {
                            nullable1 = new int?();
                        }
                        else
                        {
                            string originalString = source1.OriginalString;
                            if (originalString == null)
                            {
                                nullable1 = new int?();
                            }
                            else
                            {
                                IEnumerable<char> source2 = originalString.Where(z => z == '.');
                                nullable1 = source2 != null ? new int?(source2.Count()) : new int?();
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
                    string str1;
                    if (resourceDictionary1 == null)
                    {
                        str1 = null;
                    }
                    else
                    {
                        Uri source2 = resourceDictionary1.Source;
                        str1 = (object)source2 != null ? source2.OriginalString.Replace(".xaml", "") : null;
                    }
                    string str2 = language;
                    string requestedCulture = string.Format("{0}.{1}.xaml", str1, str2);
                    ResourceDictionary resourceDictionary2 = null;
                    foreach (var dictionary in Application.Current.Resources.MergedDictionaries.Where((Func<ResourceDictionary, bool>)(d =>
                             {
                                 string a;
                                 if (d == null)
                                 {
                                     a = null;
                                 }
                                 else
                                 {
                                     Uri source = d.Source;
                                     a = (object)source != null ? source.OriginalString : null;
                                 }

                                 string b = requestedCulture;
                                 return string.Equals(a, b, StringComparison.InvariantCultureIgnoreCase);
                             })))
                    {
                        resourceDictionary2 = dictionary;
                        break;
                    }

                    if (resourceDictionary2 == null)
                    {
                        string str3;
                        if (resourceDictionary1 == null)
                        {
                            str3 = null;
                        }
                        else
                        {
                            Uri source2 = resourceDictionary1.Source;
                            str3 = (object)source2 != null ? source2.OriginalString : null;
                        }
                        requestedCulture = str3;
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
            using DepositorDBContext DBContext = new DepositorDBContext();
            return GetDevice(DBContext)?.DeviceNumber;
        }

        internal static Device GetDevice(DepositorDBContext DBContext)
        {
            try
            {
                ParameterExpression parameterExpression;
                // ISSUE: method reference
                // ISSUE: method reference
                // ISSUE: method reference
                Device device = DBContext.Devices.FirstOrDefault(x => x.MachineName == Environment.MachineName);
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

        public static void SaveToDatabase(DepositorDBContext DBContext)
        {
            try
            {
                DBContext.SaveChanges();
            }
            catch (ValidationException ex)
            {
                string ErrorDetail = string.Format("{0}>>{1}>>{2}>stack>{3}>Validation Errors: ", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message, ex.StackTrace);
                foreach (var entityValidationError in ex.ValidationResult.MemberNames)
                {
                    ErrorDetail += ">validation error>";
                    //foreach (ValidationError validationError in (IEnumerable<ValidationError>) entityValidationError)
                    ErrorDetail = ErrorDetail + "ErrorMessage=>" + entityValidationError;
                }
                Console.WriteLine(ErrorDetail);
                Log.Error("ApplicationViewModel.SaveToDatabase()", 3, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString("G"), ErrorDetail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Saving to Database: {0}", string.Format("{0}\n{1}", ex.Message, ex?.InnerException?.Message));
                Log.ErrorFormat(nameof(ApplicationViewModel), 89, ApplicationErrorConst.ERROR_DATABASE_GENERAL.ToString(), "Error Saving to Database: {0}", string.Format("{0}>>{1}>>{2}>>{3}", ex.Message, ex.InnerException?.Message, ex.InnerException?.InnerException?.Message, ex.InnerException?.InnerException?.InnerException?.Message));
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