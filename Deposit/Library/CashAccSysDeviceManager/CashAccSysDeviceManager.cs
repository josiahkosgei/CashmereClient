using CashAccSysDeviceManager.MessageClasses;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using DeviceManager;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CashAccSysDeviceManager
{
    public class CashAccSysDeviceManager : DeviceManagerBase
    {
        private bool clearHopperRequest;
        private bool escrowBillPresent;
        private string DEVICE_PORT;
        private string CONTROLLER_PORT;
        private int BAGFULL_WARN_PERCENT;
        private bool SENSOR_INVERT_DOOR;
        private string CONTROLLER_LOG_DIRECTORY;
        private DeviceManagerState _currentState = DeviceManagerState.INIT;
        public CDCMessenger DeviceMessenger;
        private DeviceManagerMode deviceManagerMode;
        private bool _isEnabled = true;
        private ControllerStatus _currentDeviceStatus = new ControllerStatus();
        private string _connectionStatus = "Disconnected";
        private bool _acceptNotes = true;
        private bool _acceptCoins;
        private long _requestAmount;
        private int _userID;
        private bool _multiDrop;
        private bool _hasEscrow = true;
        private string _tcpStatus = "Disconnected";
        private StringBuilder _comLog = new StringBuilder();
        private string _currentCurrency;
        private bool _canTransactionEnd;

        public bool ClearHopperRequest
        {
            get => clearHopperRequest;
            set
            {
                if (clearHopperRequest == value)
                    return;
                clearHopperRequest = value;
                NotifyPropertyChanged(nameof(ClearHopperRequest));
            }
        }

        public bool EscrowBillPresent
        {
            get => escrowBillPresent;
            set
            {
                if (escrowBillPresent == value)
                    return;
                escrowBillPresent = value;
                NotifyPropertyChanged(nameof(EscrowBillPresent));
            }
        }

        public Version DeviceManagerVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public DeviceManagerState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                Log.Debug(GetType().Name, nameof(CurrentState), "CurrentStateProperty", "CurrentState changing from {0} to {1}", new object[2]
                {
           CurrentState,
           value
                });
                _currentState = value;
                NotifyCurrentTransactionStatusChanged();
            }
        }

        public DeviceManagerMode DeviceManagerMode
        {
            get => deviceManagerMode;
            set => deviceManagerMode = value;
        }

        public bool Enabled
        {
            get => _isEnabled;
            set
            {
                if (value == _isEnabled)
                    return;
                _isEnabled = value;
                if (_isEnabled)
                    OnDeviceUnlockedEvent(this, EventArgs.Empty);
                else
                    OnDeviceLockedEvent(this, EventArgs.Empty);
                NotifyPropertyChanged("IsLocked");
            }
        }

        protected CashmereLogger Log { get; set; }

        public ControllerStatus CurrentDeviceStatus
        {
            get => _currentDeviceStatus;
            set => _currentDeviceStatus = value;
        }

        public CashAccSysSerialFix CashAccSysSerialFix { get; set; }

        public CashAccSysDeviceManager(
          string host,
          int port,
          string macAddress,
          int clientID,
          string DEVICE_PORT,
          string CONTROLLER_PORT,
          int BAGFULL_WARN_PERCENT,
          bool SENSOR_INVERT_DOOR,
          string CONTROLLER_LOG_DIRECTORY,
          int messagSendInterval = 1,
          string clientType = "UI")
        {
            Log = new CashmereLogger(Assembly.GetAssembly(typeof(DeviceMessageBase)).GetName().Version.ToString(), "DeviceManagerLog", null);
            Log.Debug(GetType().Name, "Constructor", "Initialisation", "Creating CashAccSysDeviceManager", Array.Empty<object>());
            this.DEVICE_PORT = DEVICE_PORT;
            this.CONTROLLER_PORT = CONTROLLER_PORT;
            this.BAGFULL_WARN_PERCENT = BAGFULL_WARN_PERCENT;
            this.SENSOR_INVERT_DOOR = SENSOR_INVERT_DOOR;
            this.CONTROLLER_LOG_DIRECTORY = CONTROLLER_LOG_DIRECTORY;
            DeviceMessenger = new CDCMessenger(host, port, macAddress, clientID, clientType, messagSendInterval);
            DeviceMessenger.ConnectionEvent += new EventHandler<AuthoriseResponse>(OnConnectionEvent);
            DeviceMessenger.StatusReportEvent += new EventHandler<StatusReport>(OnStatusReportEvent);
            DeviceMessenger.DropStatusResultEvent += new EventHandler<DropStatus>(OnDropStatusResultEvent);
            DeviceMessenger.TransactionStatusEvent += new EventHandler<TransactionStatusResponse>(OnTransactionStatusEvent);
            DeviceMessenger.DropResultEvent += new EventHandler<DropResult>(OnDropResultEvent);
            DeviceMessenger.CITResultEvent += new EventHandler<CITResult>(OnCITResultEvent);
            CashAccSysSerialFix = CashAccSysSerialFix.GetInstance(DEVICE_PORT, CONTROLLER_PORT);
            CashAccSysSerialFix.DE50StatusChangedEvent += new EventHandler<DE50StatusChangedResult>(CashAccSysSerialFix_DE50StatusChangedEvent);
            CashAccSysSerialFix.PropertyChanged += new PropertyChangedEventHandler(CashAccSysSerialFix_PropertyChanged);
        }

        private void CashAccSysSerialFix_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("DE50CassetteFull"))
            {
                ClearHopperRequest = CashAccSysSerialFix.DE50CassetteFull && CashAccSysSerialFix.HopperBillPresent;
            }
            else
            {
                if (!e.PropertyName.Equals("EscrowBillPresent"))
                    return;
                EscrowBillPresent = CashAccSysSerialFix.EscrowBillPresent;
            }
        }

        public DeviceTransaction CurrentTransaction { get; set; }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                _connectionStatus = value;
                NotifyPropertyChanged(nameof(ConnectionStatus));
            }
        }

        public override void Connect() => DeviceMessenger.Connect();

        public bool AcceptNotes
        {
            get => _acceptNotes;
            set
            {
                _acceptNotes = value;
                NotifyPropertyChanged(nameof(AcceptNotes));
            }
        }

        public bool AcceptCoins
        {
            get => _acceptCoins;
            set
            {
                _acceptCoins = value;
                NotifyPropertyChanged(nameof(AcceptCoins));
            }
        }

        public long RequestAmount
        {
            get => _requestAmount;
            set
            {
                _requestAmount = value;
                NotifyPropertyChanged(nameof(RequestAmount));
            }
        }

        public int UserID
        {
            get => _userID;
            set
            {
                _userID = value;
                NotifyPropertyChanged(nameof(UserID));
            }
        }

        public bool MultiDrop
        {
            get => _multiDrop;
            set
            {
                _multiDrop = value;
                NotifyPropertyChanged(nameof(MultiDrop));
            }
        }

        public bool HasEscrow
        {
            get => _hasEscrow;
            set
            {
                if (_hasEscrow == value)
                    return;
                _hasEscrow = value;
                NotifyPropertyChanged(nameof(HasEscrow));
            }
        }

        public DropStatusResult DropStatus => CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus;

        public long DroppedAmountCents => (long)(CurrentTransaction?.CurrentTransactionResult?.TotalDroppedAmountCents);

        public double DroppedAmountMajorCurrency => DroppedAmountCents / 100.0;

        public string TCPStatus
        {
            get => _tcpStatus;
            set
            {
                _tcpStatus = value;
                NotifyPropertyChanged(nameof(TCPStatus));
            }
        }

        public string ComLog
        {
            get => _comLog.ToString();
            set
            {
                _comLog.AppendLine(value);
                Console.WriteLine(string.Format("[{0}] COMLOG {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), value));
                NotifyPropertyChanged(nameof(ComLog));
            }
        }

        public string CurrentCurrency
        {
            get => _currentCurrency;
            set
            {
                _currentCurrency = value;
                NotifyPropertyChanged(nameof(CurrentCurrency));
            }
        }

        private void SelectCurrency(string currency) => DeviceMessenger.SetCurrency(currency);

        public void ShowDeviceController()
        {
            Log.Info(GetType().Name, "ShowDeviceController()", "Command", "Showing the device controller", Array.Empty<object>());
            DeviceMessenger.ShowDeviceController();
        }

        public event EventHandler<StringResult> ConnectionEvent;

        private void OnConnectionEvent(object sender, AuthoriseResponse authoriseResponse)
        {
            StringResult e = new StringResult();
            if (authoriseResponse.Body.Result != "ACCEPTED")
            {
                e.level = ErrorLevel.ERROR;
                e.resultCode = 260;
                e.data = authoriseResponse.Body.Result;
            }
            else
            {
                e.level = ErrorLevel.SUCCESS;
                e.resultCode = 0;
                e.data = authoriseResponse.Body.Result;
            }
            if (ConnectionEvent == null)
                return;
            ConnectionEvent(this, e);
        }

        public event EventHandler<EventArgs> NotifyCurrentTransactionStatusChangedEvent;

        private void OnNotifyCurrentTransactionStatusChangedEvent(object sender, EventArgs e)
        {
            if (NotifyCurrentTransactionStatusChangedEvent == null)
                return;
            NotifyCurrentTransactionStatusChangedEvent(this, e);
        }

        public event EventHandler<DeviceTransaction> TransactionStartedEvent;

        public void OnTransactionStartedEvent(object sender, DeviceTransaction e)
        {
            Log.Debug(GetType().Name, nameof(OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: Handling TransactionStartedEvent", new object[1]
            {
         CurrentState
            });
            if (CurrentState == DeviceManagerState.TRANSACTION_STARTING)
            {
                CurrentState = DeviceManagerState.TRANSACTION_STARTED;
                Log.Info(GetType().Name, nameof(OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: TransactionStartedEvent complete, re-raising event", new object[1]
                {
           CurrentState
                });
                if (TransactionStartedEvent == null)
                    return;
                TransactionStartedEvent(this, e);
            }
            else
            {
                Log.Warning(GetType().Name, nameof(OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: TransactionStartedEvent outside DeviceManagerState.TRANSACTION_STARTING", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
        }

        private void OnDropStatusResultEvent(object sender, DropStatus dropStatus)
        {
            Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Handling DropStatusResultEvent", new object[1]
            {
         CurrentState
            });
            DropStatusResult dropStatusResult = ProcessDropStatus(dropStatus);
            CashmereLogger log = Log;
            string name = GetType().Name;
            object[] objArray = new object[3]
            {
         CurrentState,
        null,
        null
            };
            DropStatusResultStatus statusResultStatus1;
            string str;
            if (dropStatusResult == null)
            {
                str = null;
            }
            else
            {
                DropStatusResultData data = dropStatusResult.data;
                if (data == null)
                {
                    str = null;
                }
                else
                {
                    statusResultStatus1 = data.DropStatusResultStatus;
                    str = statusResultStatus1.ToString();
                }
            }
            objArray[1] = str;
            objArray[2] = (dropStatusResult?.data?.DenominationResult?.ToString());
            log.Info(name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: DropStatusResultStatus = {1} Denomination = {2}", objArray);
            if (CurrentState == DeviceManagerState.DROP_STARTING)
            {
                CurrentState = DeviceManagerState.DROP_STARTED;
                Log.Info(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState changed from DROP_STARTING to DROP_STARTED", Array.Empty<object>());
            }
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.DropResults.CurrentDropID != null)
                {
                    if (dropStatusResult.level != ErrorLevel.ERROR)
                    {
                        DropStatusResultStatus? statusResultStatus2 = CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                        CurrentTransaction.CurrentTransactionResult.CurrentDropStatus = dropStatusResult;
                        if (!HasEscrow)
                        {
                            CurrentTransaction.CurrentTransactionResult.LastDroppedAmountCents = (long)(dropStatusResult?.data?.DenominationResult?.data?.TotalValue);
                            CurrentTransaction.CurrentTransactionResult.LastDroppedNotes = dropStatusResult?.data?.DenominationResult?.data;
                            CurrentTransaction.CurrentTransactionResult.TotalDroppedAmountCents = CurrentTransaction.CurrentTransactionResult.LastDroppedAmountCents;
                            CurrentTransaction.CurrentTransactionResult.TotalDroppedNotes = CurrentTransaction.CurrentTransactionResult.LastDroppedNotes;
                            CurrentTransaction.CurrentTransactionResult.CurrentDropStatus.data.DenominationResult = null;
                        }
                        DropStatusResultStatus? nullable = statusResultStatus2;
                        statusResultStatus1 = dropStatusResult.data.DropStatusResultStatus;
                        if (!(nullable.GetValueOrDefault() == statusResultStatus1 & nullable.HasValue))
                        {
                            Log.Info(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: DropStatus has changed from {1} to {2}", new object[3]
                            {
                 CurrentState,
                 statusResultStatus2,
                 dropStatusResult.data.DropStatusResultStatus
                            });
                            statusResultStatus1 = dropStatusResult.data.DropStatusResultStatus;
                            switch (statusResultStatus1)
                            {
                                case DropStatusResultStatus.DROPPING:
                                    CurrentState = DeviceManagerState.DROP_STOPPED;
                                    break;
                                case DropStatusResultStatus.ESCROW_REJECT:
                                    Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowRejectEvent", new object[1]
                                    {
                     CurrentState
                                    });
                                    DeviceTransactionResult e1 = new DeviceTransactionResult();
                                    e1.level = dropStatusResult.level;
                                    e1.data = CurrentTransaction;
                                    OnEscrowRejectEvent(this, e1);
                                    break;
                                case DropStatusResultStatus.ESCROW_DROP:
                                    Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowDropEvent", new object[1]
                                    {
                     CurrentState
                                    });
                                    DeviceTransactionResult e2 = new DeviceTransactionResult();
                                    e2.level = dropStatusResult.level;
                                    e2.data = CurrentTransaction;
                                    OnEscrowDropEvent(this, e2);
                                    break;
                                case DropStatusResultStatus.ESCROW_DONE:
                                    Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowOperationCompleteEvent", new object[1]
                                    {
                     CurrentState
                                    });
                                    DeviceTransactionResult e3 = new DeviceTransactionResult();
                                    e3.level = dropStatusResult.level;
                                    e3.data = CurrentTransaction;
                                    OnEscrowOperationCompleteEvent(this, e3);
                                    break;
                                case DropStatusResultStatus.DONE:
                                    Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnCountEndEvent", new object[1]
                                    {
                     CurrentState
                                    });
                                    DeviceTransactionResult e4 = new DeviceTransactionResult();
                                    e4.level = dropStatusResult.level;
                                    e4.data = CurrentTransaction;
                                    OnCountEndEvent(this, e4);
                                    break;
                            }
                        }
                        Log.Debug(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnTransactionStatusEvent", new object[1]
                        {
               CurrentState
                        });
                        OnTransactionStatusEvent(this, null);
                    }
                }
                else
                    Log.Warning(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: dropstatus received outside of a drop", new object[1]
                    {
             CurrentState
                    });
            }
            else
            {
                Log.Warning(GetType().Name, nameof(OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnDropStatusResultEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            NotifyCurrentTransactionStatusChanged();
        }

        public event EventHandler<DeviceStatusChangedEventArgs> StatusReportEvent;

        private void OnStatusReportEvent(object sender, StatusReport statusReport)
        {
            if (CurrentState == DeviceManagerState.INIT)
            {
                CurrentState = DeviceManagerState.NONE;
                Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from INIT to NONE", new object[1]
                {
           CurrentState
                });
            }
            DeviceStatusChangedEventArgs e = ProcessStatusReport(statusReport);
            DeviceState = e.ControllerStatus.NoteAcceptor.Status;
            ControllerState = e.ControllerStatus.ControllerState;
            HasEscrow = e.ControllerStatus.Escrow.Type != EscrowType.NONE && e.ControllerStatus.Escrow.Type != EscrowType.NO_DEVICE;
            if (CurrentDeviceStatus.Bag.BagState != e.ControllerStatus.Bag.BagState)
            {
                if (e.ControllerStatus.Bag.BagState == BagState.CLOSED)
                    OnBagClosedEvent(this, EventArgs.Empty);
                else if (CurrentDeviceStatus.Bag.BagState != BagState.NONE && e.ControllerStatus.Bag.BagState == BagState.OK)
                    OnBagOpenedEvent(this, EventArgs.Empty);
            }
            if (CurrentDeviceStatus.Sensor.Bag != e.ControllerStatus.Sensor.Bag)
            {
                if (CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.PRESENT && e.ControllerStatus.Sensor.Bag == DeviceSensorBag.REMOVED)
                    OnBagRemovedEvent(this, EventArgs.Empty);
                else if (CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.REMOVED && e.ControllerStatus.Sensor.Bag == DeviceSensorBag.PRESENT)
                    OnBagPresentEvent(this, EventArgs.Empty);
            }
            if (CurrentDeviceStatus.Bag.BagState == BagState.BAG_REMOVED && CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.PRESENT)
                EndCIT("12345");
            if (CurrentDeviceStatus.Bag.PercentFull != e.ControllerStatus.Bag.PercentFull)
            {
                if (e.ControllerStatus.Bag.PercentFull >= 100)
                    OnBagFullAlertEvent(this, e.ControllerStatus);
                else if (e.ControllerStatus.Bag.PercentFull >= BAGFULL_WARN_PERCENT)
                    OnBagFullWarningEvent(this, e.ControllerStatus);
            }
            if (CurrentDeviceStatus.Sensor.Door != DeviceSensorDoor.NONE && CurrentDeviceStatus.Sensor.Door != e.ControllerStatus.Sensor.Door)
            {
                if (e.ControllerStatus.Sensor.Door == DeviceSensorDoor.OPEN)
                    OnDoorOpenEvent(this, EventArgs.Empty);
                else if (e.ControllerStatus.Sensor.Door == DeviceSensorDoor.CLOSED)
                    OnDoorClosedEvent(this, EventArgs.Empty);
            }
            if (CurrentDeviceStatus.ControllerState != e.ControllerStatus.ControllerState)
            {
                switch (e.ControllerStatus.ControllerState)
                {
                    case ControllerState.IDLE:
                        Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: ControllerState changed from {1} to {2}", new object[3]
                        {
               CurrentState,
               CurrentDeviceStatus.ControllerState,
               e.ControllerStatus.ControllerState
                        });
                        if (e.ControllerStatus.Transaction.Status != DeviceTransactionStatus.NONE && CurrentTransaction == null && DeviceManagerMode == DeviceManagerMode.NONE)
                        {
                            Log.Warning(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to IDLE", new object[1]
                            {
                 CurrentState
                            });
                            CurrentState = DeviceManagerState.OUT_OF_ORDER;
                            ResetDevice(false);
                            break;
                        }
                        break;
                    case ControllerState.DROP:
                        Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnCountStartedEvent", new object[1]
                        {
               CurrentState
                        });
                        OnCountStartedEvent(this, new DeviceTransactionResult()
                        {
                            data = CurrentTransaction
                        });
                        break;
                    case ControllerState.DROP_PAUSED:
                        Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnCountPauseEvent", new object[1]
                        {
               CurrentState
                        });
                        OnCountPauseEvent(this, new DeviceTransactionResult()
                        {
                            data = CurrentTransaction
                        });
                        break;
                    case ControllerState.ESCROW_DROP:
                        if (CurrentTransaction == null)
                        {
                            Log.Warning(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to ESCROW_DROP", new object[1]
                            {
                 CurrentState
                            });
                            CurrentState = DeviceManagerState.OUT_OF_ORDER;
                            ResetDevice(false);
                            break;
                        }
                        break;
                    case ControllerState.ESCROW_REJECT:
                        if (CurrentTransaction == null)
                        {
                            Log.Warning(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to ESCROW_REJECT", new object[1]
                            {
                 CurrentState
                            });
                            CurrentState = DeviceManagerState.OUT_OF_ORDER;
                            ResetDevice(false);
                            break;
                        }
                        break;
                }
            }
            ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
            EscrowStatus? status1;
            DeviceTransactionStatus? status2;
            if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
            {
                if (HasEscrow)
                {
                    ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                    int num;
                    if (currentDeviceStatus2 == null)
                    {
                        num = 0;
                    }
                    else
                    {
                        status1 = currentDeviceStatus2.Escrow?.Status;
                        EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                        num = status1.GetValueOrDefault() == escrowStatus & status1.HasValue ? 1 : 0;
                    }
                    if (num == 0)
                        goto label_48;
                }
                ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                int num1;
                if (currentDeviceStatus3 == null)
                {
                    num1 = 1;
                }
                else
                {
                    status2 = currentDeviceStatus3.Transaction?.Status;
                    DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
                    num1 = !(status2.GetValueOrDefault() == transactionStatus & status2.HasValue) ? 1 : 0;
                }
                if (num1 != 0)
                {
                    if (!CanCount && CurrentTransaction != null)
                    {
                        if (CurrentState != DeviceManagerState.NONE)
                        {
                            Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: handles lost TransactionStatusResponse>>Raising GetTransactionStatus", new object[1]
                            {
                 CurrentState
                            });
                            GetTransactionStatus();
                        }
                    }
                    else
                        ResetDevice(false);
                }
            }
        label_48:
            if (CurrentDeviceStatus.Transaction.Status != e.ControllerStatus.Transaction.Status)
            {
                Log.Info(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Transaction status has changed from {1} to {2}", new object[3]
                {
           CurrentState,
           CurrentDeviceStatus.Transaction.Status,
           e.ControllerStatus.Transaction.Status
                });
                switch (e.ControllerStatus.Transaction.Status)
                {
                    case DeviceTransactionStatus.NONE:
                        if (CurrentState == DeviceManagerState.TRANSACTION_ENDING)
                        {
                            CurrentState = DeviceManagerState.TRANSACTION_ENDED;
                            Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnTransactionEndEvent", new object[1]
                            {
                 CurrentState
                            });
                            OnTransactionEndEvent(this, new DeviceTransactionResult()
                            {
                                data = CurrentTransaction
                            });
                            break;
                        }
                        Log.Warning(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Invalid state, DeviceTransactionStatus changed to NONE while CurrentState is {0}", new object[1]
                        {
               CurrentState
                        });
                        break;
                }
            }
            if (CurrentState == DeviceManagerState.OUT_OF_ORDER && CurrentTransaction == null)
            {
                ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                if ((currentDeviceStatus4 != null ? (currentDeviceStatus4.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
                {
                    ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                    int num2;
                    if (currentDeviceStatus5 == null)
                    {
                        num2 = 0;
                    }
                    else
                    {
                        DeviceState? status3 = currentDeviceStatus5.NoteAcceptor?.Status;
                        DeviceState deviceState = DeviceState.IDLE;
                        num2 = status3.GetValueOrDefault() == deviceState & status3.HasValue ? 1 : 0;
                    }
                    if (num2 != 0)
                    {
                        ControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
                        int num3;
                        if (currentDeviceStatus6 == null)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            status1 = currentDeviceStatus6.Escrow?.Status;
                            EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                            num3 = status1.GetValueOrDefault() == escrowStatus & status1.HasValue ? 1 : 0;
                        }
                        if (num3 != 0)
                        {
                            ControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
                            int num4;
                            if (currentDeviceStatus7 == null)
                            {
                                num4 = 0;
                            }
                            else
                            {
                                status2 = currentDeviceStatus7.Transaction?.Status;
                                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                                num4 = status2.GetValueOrDefault() == transactionStatus & status2.HasValue ? 1 : 0;
                            }
                            if (num4 != 0)
                            {
                                Log.Debug(GetType().Name, nameof(OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Reset from out of order is complete", new object[1]
                                {
                   CurrentState
                                });
                                CurrentState = DeviceManagerState.NONE;
                            }
                        }
                    }
                }
            }
            CurrentDeviceStatus = e.ControllerStatus;
            e.DeviceManagerState = CurrentState;
            if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
                ResetDevice(false);
            NotifyCurrentTransactionStatusChanged();
            if (StatusReportEvent == null)
                return;
            StatusReportEvent(this, e);
        }

        public void Initialise() => CurrentTransaction = null;

        public event EventHandler<DeviceTransactionResult> TransactionStatusEvent;

        private void OnTransactionStatusEvent(
          object sender,
          TransactionStatusResponse TransactionStatusResponse)
        {
            if (CurrentTransaction != null)
            {
                if (TransactionStatusResponse != null)
                {
                    Log.Debug(GetType().Name, nameof(OnTransactionStatusEvent), "EventHandler", "CurrentState = {0}: Updating CurrentTransactionResult", new object[1]
                    {
             CurrentState
                    });
                    CurrentState = DeviceManagerState.NONE;
                    TransactionStatusResponseResult statusResponseResult = ProcessTransactionStatusResponse(TransactionStatusResponse);
                    if (statusResponseResult.data.SessionID == CurrentTransaction.SessionID && statusResponseResult.data.TransactionID == CurrentTransaction.TransactionID && statusResponseResult.data.Status != TransactionResultStatus.NONE)
                    {
                        CurrentTransaction.CurrentTransactionResult = statusResponseResult.data;
                    }
                    else
                    {
                        Log.Error(nameof(CashAccSysDeviceManager), "ERROR", nameof(OnTransactionStatusEvent), "Invalid TransactionStatusResult: " + JsonConvert.SerializeObject(statusResponseResult), Array.Empty<object>());
                        CurrentState = DeviceManagerState.OUT_OF_ORDER;
                        ResetDevice(false);
                    }
                }
                NotifyCurrentTransactionStatusChanged();
                if (TransactionStatusEvent == null)
                    return;
                EventHandler<DeviceTransactionResult> transactionStatusEvent = TransactionStatusEvent;
                DeviceTransactionResult e = new DeviceTransactionResult();
                e.level = ErrorLevel.SUCCESS;
                e.data = CurrentTransaction;
                transactionStatusEvent(this, e);
            }
            else
                Log.Warning(GetType().Name, nameof(OnTransactionStatusEvent), "EventHandler", "CurrentState = {0}: TransactionStatusResponse received outside of a transaction, ignore", new object[1]
                {
           CurrentState
                });
        }

        public event EventHandler<DeviceTransactionResult> TransactionEndEvent;

        private void OnTransactionEndEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: Running Handler", new object[1]
            {
         CurrentState
            });
            if (CurrentState == DeviceManagerState.TRANSACTION_ENDED)
            {
                Log.Debug(GetType().Name, nameof(OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: Transaction Complete", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.NONE;
                if (TransactionEndEvent == null)
                    return;
                TransactionEndEvent(this, e);
            }
            else
            {
                if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
                    return;
                Log.Warning(GetType().Name, nameof(OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: OnTransactionEndEvent outside DeviceManagerState.TRANSACTION_ENDING", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
        }

        public event EventHandler<DeviceTransactionResult> DropResultEvent;

        private void OnDropResultEvent(object sender, DropResult DropResult)
        {
            Log.Debug(GetType().Name, nameof(OnDropResultEvent), "EventHandler", "CurrentState = {0}: Running Handler", new object[1]
            {
         CurrentState
            });
            DropResultResult dropResultResult = ProcessDropResult(DropResult);
            if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
                return;
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.DropResults.CurrentDropID == dropResultResult.DropID)
                {
                    CurrentTransaction.DropResults.Drops[CurrentTransaction.DropResults.CurrentDropID] = dropResultResult;
                    Log.Debug(GetType().Name, nameof(OnDropResultEvent), "EventHandler", "CurrentState = {0}: invoking OnTransactionStatusEvent", new object[1]
                    {
             CurrentState
                    });
                    CurrentTransaction.DropResults.CurrentDropID = null;
                    OnTransactionStatusEvent(this, null);
                }
                else
                {
                    switch (CurrentTransaction.DropResults.Drops[dropResultResult.DropID]?.DropID)
                    {
                        case null:
                            Console.Error.WriteLine("ERROR in CashAccSysDeviceManager.OnDropResultEvent(): DropResult for a drop that is not part of the CurrentTranscation: DropID " + dropResultResult.DropID + " was not found");
                            Log.Warning(GetType().Name, nameof(OnDropResultEvent), "EventHandler", "CurrentState = {0}: ERROR in CashAccSysDeviceManager.OnDropResultEvent(): DropResult for a drop that is not part of the CurrentTranscation: DropID {2} was not found", new object[2]
                            {
                 CurrentState,
                 dropResultResult.DropID
                            });
                            CurrentState = DeviceManagerState.OUT_OF_ORDER;
                            ResetDevice(false);
                            break;
                        default:
                            Log.Warning(GetType().Name, nameof(OnDropResultEvent), "EventHandler", "CurrentState = {0}: ERROR in CashAccSysDeviceManager.OnDropResultEvent(): Expecting DropResult with DropID {1} but found DropID {2} instead", new object[3]
                            {
                 CurrentState,
                 CurrentTransaction.DropResults.CurrentDropID,
                 dropResultResult.DropID
                            });
                            break;
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.OnDropResultEvent()");
                Log.Warning(GetType().Name, nameof(OnDropResultEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnDropResultEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
        }

        public event EventHandler<DeviceTransactionResult> CashInStartedEvent;

        private void OnCashInStartedEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnCashInStartedEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CashInStartedEvent == null)
                return;
            CashInStartedEvent(this, e);
        }

        public event EventHandler<DeviceTransactionResult> CountEndEvent;

        private void OnCountEndEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnCountEndEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CurrentTransaction == null)
            {
                Log.Warning(GetType().Name, nameof(OnCountEndEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnCountEndEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            else
            {
                if (HasEscrow && CurrentState != DeviceManagerState.DROP_ESCROW_DONE)
                {
                    Log.Debug(GetType().Name, nameof(OnCountEndEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowOperationCompleteEvent", new object[1]
                    {
             CurrentState
                    });
                    OnEscrowOperationCompleteEvent(this, e);
                }
                if (CountEndEvent == null)
                    return;
                CountEndEvent(this, e);
            }
        }

        public event EventHandler<DeviceTransactionResult> CountStartedEvent;

        private void OnCountStartedEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnCountStartedEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CountStartedEvent == null)
                return;
            CountStartedEvent(this, e);
        }

        public event EventHandler<DeviceTransactionResult> CountPauseEvent;

        private void OnCountPauseEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnCountPauseEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
                return;
            if (CurrentTransaction == null)
            {
                Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.OnCountPauseEvent()");
                Log.Warning(GetType().Name, nameof(OnCountPauseEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnCountPauseEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            else if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTING)
                DeviceMessenger.EscrowDrop();
            else if (CurrentState == DeviceManagerState.DROP_ESCROW_REJECTING)
            {
                DeviceMessenger.EscrowReject();
            }
            else
            {
                if (CountPauseEvent == null)
                    return;
                CountPauseEvent(this, e);
            }
        }

        public event EventHandler<DeviceTransactionResult> EscrowDropEvent;

        private void OnEscrowDropEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnEscrowDropEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CurrentTransaction == null)
            {
                Log.Warning(GetType().Name, nameof(OnEscrowDropEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnEscrowDropEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            else
            {
                if (EscrowDropEvent == null)
                    return;
                EscrowDropEvent(this, e);
            }
        }

        public event EventHandler<DeviceTransactionResult> EscrowRejectEvent;

        private void OnEscrowRejectEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnEscrowRejectEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CurrentTransaction == null)
            {
                Log.Warning(GetType().Name, nameof(OnEscrowRejectEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnEscrowRejectEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            else
            {
                if (EscrowRejectEvent == null)
                    return;
                EscrowRejectEvent(this, e);
            }
        }

        public event EventHandler<DeviceTransactionResult> EscrowOperationCompleteEvent;

        private void OnEscrowOperationCompleteEvent(object sender, DeviceTransactionResult e)
        {
            Log.Debug(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
            {
         CurrentState
            });
            if (CurrentTransaction == null)
            {
                Log.Warning(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.OnEscrowOperationCompleteEvent()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
            }
            else
            {
                if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTING)
                {
                    CurrentState = DeviceManagerState.DROP_ESCROW_ACCEPTED;
                    Log.Info(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from DROP_ESCROW_ACCEPTING to DROP_ESCROW_ACCEPTED", new object[1]
                    {
             CurrentState
                    });
                    DeviceTransactionResult e1 = new DeviceTransactionResult();
                    e1.level = e.level;
                    e1.data = CurrentTransaction;
                    OnEscrowDropEvent(this, e1);
                }
                else if (CurrentState == DeviceManagerState.DROP_ESCROW_REJECTING || CurrentState == DeviceManagerState.OUT_OF_ORDER)
                {
                    CurrentState = DeviceManagerState.DROP_ESCROW_REJECTED;
                    Log.Info(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from DROP_ESCROW_REJECTING to DROP_ESCROW_REJECTED", new object[1]
                    {
             CurrentState
                    });
                    DeviceTransactionResult e2 = new DeviceTransactionResult();
                    e2.level = e.level;
                    e2.data = CurrentTransaction;
                    OnEscrowRejectEvent(this, e2);
                }
                if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTED || CurrentState == DeviceManagerState.DROP_ESCROW_REJECTED)
                {
                    Log.Info(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changing to DROP_ESCROW_DONE", new object[1]
                    {
             CurrentState
                    });
                    CurrentState = DeviceManagerState.DROP_ESCROW_DONE;
                    if (EscrowOperationCompleteEvent == null)
                        return;
                    EscrowOperationCompleteEvent(this, e);
                }
                else
                {
                    if (CurrentState == DeviceManagerState.DROP_ESCROW_DONE)
                        return;
                    Log.Warning(GetType().Name, nameof(OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: FAILED: CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTED|| CurrentState == DeviceManagerState.DROP_ESCROW_REJECTED", new object[1]
                    {
             CurrentState
                    });
                    CurrentState = DeviceManagerState.OUT_OF_ORDER;
                }
            }
        }

        public event EventHandler<CITResult> CITResultEvent;

        private void OnCITResultEvent(object sender, CITResult e)
        {
            if (CITResultEvent == null)
                return;
            CITResultEvent(this, e);
        }

        public event EventHandler<EventArgs> DoorOpenEvent;

        private void OnDoorOpenEvent(object sender, EventArgs e)
        {
            if (DoorOpenEvent == null)
                return;
            DoorOpenEvent(this, e);
        }

        public event EventHandler<EventArgs> DoorClosedEvent;

        private void OnDoorClosedEvent(object sender, EventArgs e)
        {
            if (DoorClosedEvent == null)
                return;
            DoorClosedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagRemovedEvent;

        private void OnBagRemovedEvent(object sender, EventArgs e)
        {
            if (BagRemovedEvent == null)
                return;
            BagRemovedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagPresentEvent;

        private void OnBagPresentEvent(object sender, EventArgs e)
        {
            if (BagPresentEvent == null)
                return;
            BagPresentEvent(this, e);
        }

        public event EventHandler<EventArgs> BagOpenedEvent;

        private void OnBagOpenedEvent(object sender, EventArgs e)
        {
            if (BagOpenedEvent == null)
                return;
            BagOpenedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagClosedEvent;

        private void OnBagClosedEvent(object sender, EventArgs e)
        {
            if (BagClosedEvent == null)
                return;
            BagClosedEvent(this, e);
        }

        public event EventHandler<ControllerStatus> BagFullAlertEvent;

        private void OnBagFullAlertEvent(object sender, ControllerStatus e)
        {
            if (BagFullAlertEvent == null)
                return;
            BagFullAlertEvent(this, e);
        }

        public event EventHandler<ControllerStatus> BagFullWarningEvent;

        private void OnBagFullWarningEvent(object sender, ControllerStatus e)
        {
            if (BagFullWarningEvent == null)
                return;
            BagFullWarningEvent(this, e);
        }

        public event EventHandler<EventArgs> BagReplacedEvent;

        private void OnBagReplacedEvent(object sender, EventArgs e)
        {
            if (BagReplacedEvent == null)
                return;
            BagReplacedEvent(this, e);
        }

        public event EventHandler<EventArgs> DeviceLockedEvent;

        private void OnDeviceLockedEvent(object sender, EventArgs e)
        {
            if (DeviceLockedEvent == null)
                return;
            DeviceLockedEvent(this, e);
        }

        public event EventHandler<EventArgs> DeviceUnlockedEvent;

        private void OnDeviceUnlockedEvent(object sender, EventArgs e)
        {
            if (DeviceUnlockedEvent == null)
                return;
            DeviceUnlockedEvent(this, e);
        }

        public event EventHandler<EventArgs> EscrowJamStartEvent;

        public void OnEscrowJamStartEvent(object sender, EventArgs e)
        {
            CurrentState = DeviceManagerState.ESCROWJAM_START;
            DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
            if (EscrowJamStartEvent == null)
                return;
            EscrowJamStartEvent(this, e);
        }

        public event EventHandler<EventArgs> EscrowJamClearWaitEvent;

        private void OnEscrowJamClearWaitEvent(object sender, EventArgs e)
        {
            CurrentState = DeviceManagerState.ESCROWJAM_CLEAR_WAIT;
            DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
            if (EscrowJamClearWaitEvent == null)
                return;
            EscrowJamClearWaitEvent(this, e);
        }

        public event EventHandler<EventArgs> EscrowJamEndRequestEvent;

        private void OnEscrowJamEndRequestEvent(object sender, EventArgs e)
        {
            if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM || CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST)
                return;
            CurrentState = DeviceManagerState.ESCROWJAM_END_REQUEST;
            DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
            if (EscrowJamEndRequestEvent == null)
                return;
            EscrowJamEndRequestEvent(this, e);
        }

        public event EventHandler<EventArgs> EscrowJamEndEvent;

        private void OnEscrowJamEndEvent(object sender, EventArgs e)
        {
            CurrentState = DeviceManagerState.NONE;
            DeviceManagerMode = DeviceManagerMode.NONE;
            if (EscrowJamEndEvent == null)
                return;
            EscrowJamEndEvent(this, e);
        }

        public override void CountNotes() => throw new NotImplementedException();

        public override void CountCoins() => throw new NotImplementedException();

        public override void CountBoth() => throw new NotImplementedException();

        public override void ResetDevice(bool openEscrow = false)
        {
            DeviceMessenger.ClearOutgoingMessageQueue();
            if (openEscrow || HasEscrow)
            {
                switch (CurrentDeviceStatus.ControllerState)
                {
                    case ControllerState.IDLE:
                    case ControllerState.ESCROW_DROP:
                    case ControllerState.ESCROW_REJECT:
                        ControllerStatus currentDeviceStatus = CurrentDeviceStatus;
                        int num;
                        if (currentDeviceStatus == null)
                        {
                            num = 1;
                        }
                        else
                        {
                            DeviceTransactionStatus? status = currentDeviceStatus.Transaction?.Status;
                            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                            num = !(status.GetValueOrDefault() == transactionStatus & status.HasValue) ? 1 : 0;
                        }
                        if (num == 0)
                            break;
                        TransactionEnd();
                        break;
                    case ControllerState.DROP:
                        PauseCount();
                        break;
                    case ControllerState.DROP_PAUSED:
                        if (!HasEscrow)
                            break;
                        EscrowReject();
                        break;
                }
            }
            else if (CurrentDeviceStatus.ControllerState == ControllerState.IDLE)
                ;
        }

        public override void SetCurrency(string currency) => DeviceMessenger.SetCurrency(currency);

        public bool CanTransactionStart
        {
            get
            {
                if (HasEscrow)
                {
                    if (CurrentTransaction == null && CurrentState == DeviceManagerState.NONE)
                    {
                        ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                        if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
                        {
                            ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                            int num1;
                            if (currentDeviceStatus2 == null)
                            {
                                num1 = 0;
                            }
                            else
                            {
                                DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
                                DeviceState deviceState = DeviceState.IDLE;
                                num1 = status.GetValueOrDefault() == deviceState & status.HasValue ? 1 : 0;
                            }
                            if (num1 != 0)
                            {
                                ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                                int num2;
                                if (currentDeviceStatus3 == null)
                                {
                                    num2 = 0;
                                }
                                else
                                {
                                    EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                                    EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                                    num2 = status.GetValueOrDefault() == escrowStatus & status.HasValue ? 1 : 0;
                                }
                                if (num2 != 0)
                                {
                                    ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                                    if (currentDeviceStatus4 == null)
                                        return false;
                                    DeviceTransactionStatus? status = currentDeviceStatus4.Transaction?.Status;
                                    DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                                    return status.GetValueOrDefault() == transactionStatus & status.HasValue;
                                }
                            }
                        }
                    }
                    return false;
                }
                ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                if ((currentDeviceStatus5 != null ? (currentDeviceStatus5.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) == 0)
                    return false;
                ControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
                if (currentDeviceStatus6 == null)
                    return false;
                DeviceTransactionStatus? status1 = currentDeviceStatus6.Transaction?.Status;
                DeviceTransactionStatus transactionStatus1 = DeviceTransactionStatus.NONE;
                return status1.GetValueOrDefault() == transactionStatus1 & status1.HasValue;
            }
        }

        public void TransactionStart(
          string currency,
          string accountNumber,
          string sessionID,
          string transactionID,
          long transactionLimitCents = 9223372036854775807,
          long transactionValueCents = 0)
        {
            Log.Debug(GetType().Name, nameof(TransactionStart), "Command", "CurrentState = {0}: StartingTransaction", new object[1]
            {
         CurrentState
            });
            if (CanTransactionStart)
            {
                if (CurrentState == DeviceManagerState.NONE)
                {
                    CurrentState = DeviceManagerState.TRANSACTION_STARTING;
                    try
                    {
                        if (CurrentTransaction == null)
                        {
                            CurrentTransaction = new DeviceTransaction(accountNumber, sessionID, transactionID, currency, transactionValueCents, transactionValueCents);
                            OnTransactionStartedEvent(this, CurrentTransaction);
                        }
                        else
                        {
                            if (CurrentTransaction?.SessionID == sessionID)
                            {
                                if (CurrentTransaction?.TransactionID == transactionID)
                                {
                                    Console.Error.WriteLine("This session and transaction is currently transacting");
                                    Log.Warning(GetType().Name, nameof(TransactionStart), "InvalidOperation", "DeviceManager is currently processing Session={0} and Transaction={1}.", new object[2]
                                    {
                     CurrentTransaction?.SessionID,
                     CurrentTransaction?.TransactionID
                                    });
                                }
                                else
                                    Log.Warning(GetType().Name, nameof(TransactionStart), "InvalidOperation", "DeviceManager is currently processing Session={0} and Transaction={1}. Please end the current transaction before starting Session={2}; Transaction={3};", new object[4]
                                    {
                     CurrentTransaction?.SessionID,
                     CurrentTransaction?.TransactionID,
                     sessionID,
                     transactionID
                                    });
                            }
                            else
                                Log.Warning(GetType().Name, nameof(TransactionStart), "InvalidOperation", "CurrentTransaction.TransactionID {0} != transactionID {1}", new object[2]
                                {
                   CurrentTransaction?.SessionID,
                   sessionID
                                });
                            ResetDevice(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    Log.Warning(GetType().Name, nameof(TransactionStart), "InvalidOperation", "CurrentState = {0}: TransactionStart outside DeviceManagerState.NONE", new object[1]
                    {
             CurrentState
                    });
                    CurrentState = DeviceManagerState.OUT_OF_ORDER;
                    ResetDevice(false);
                }
            }
            else
                Log.Warning(GetType().Name, nameof(TransactionStart), "InvalidOperation", "FAILED: CanTransactionStart", Array.Empty<object>());
        }

        public bool CanTransactionEnd
        {
            get
            {
                if (HasEscrow)
                {
                    if (CurrentState == DeviceManagerState.NONE && CurrentTransaction?.CurrentTransactionResult != null)
                    {
                        ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                        DeviceState? status1;
                        if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) == 0)
                        {
                            ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                            int num;
                            if (currentDeviceStatus2 == null)
                            {
                                num = 0;
                            }
                            else
                            {
                                status1 = currentDeviceStatus2.NoteAcceptor?.Status;
                                DeviceState deviceState = DeviceState.OK;
                                num = status1.GetValueOrDefault() == deviceState & status1.HasValue ? 1 : 0;
                            }
                            if (num == 0)
                                goto label_23;
                        }
                        ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                        int num1;
                        if (currentDeviceStatus3 == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            status1 = currentDeviceStatus3.NoteAcceptor?.Status;
                            DeviceState deviceState = DeviceState.IDLE;
                            num1 = status1.GetValueOrDefault() == deviceState & status1.HasValue ? 1 : 0;
                        }
                        if (num1 == 0)
                        {
                            ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                            int num2;
                            if (currentDeviceStatus4 == null)
                            {
                                num2 = 0;
                            }
                            else
                            {
                                status1 = currentDeviceStatus4.NoteAcceptor?.Status;
                                DeviceState deviceState = DeviceState.OK;
                                num2 = status1.GetValueOrDefault() == deviceState & status1.HasValue ? 1 : 0;
                            }
                            if (num2 == 0)
                                goto label_23;
                        }
                        if (HasEscrow)
                        {
                            ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                            int num3;
                            if (currentDeviceStatus5 == null)
                            {
                                num3 = 0;
                            }
                            else
                            {
                                EscrowStatus? status2 = currentDeviceStatus5.Escrow?.Status;
                                EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                                num3 = status2.GetValueOrDefault() == escrowStatus & status2.HasValue ? 1 : 0;
                            }
                            if (num3 == 0)
                                goto label_23;
                        }
                        ControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
                        if (currentDeviceStatus6 == null)
                            return false;
                        DeviceTransactionStatus? status3 = currentDeviceStatus6.Transaction?.Status;
                        DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
                        return status3.GetValueOrDefault() == transactionStatus & status3.HasValue;
                    }
                label_23:
                    return false;
                }
                if (CurrentTransaction?.CurrentTransactionResult != null && CurrentState != DeviceManagerState.TRANSACTION_ENDING)
                {
                    ControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
                    DeviceState? status;
                    int num4;
                    if (currentDeviceStatus7 == null)
                    {
                        num4 = 0;
                    }
                    else
                    {
                        status = currentDeviceStatus7.NoteAcceptor?.Status;
                        DeviceState deviceState = DeviceState.IDLE;
                        num4 = status.GetValueOrDefault() == deviceState & status.HasValue ? 1 : 0;
                    }
                    if (num4 == 0)
                    {
                        ControllerStatus currentDeviceStatus8 = CurrentDeviceStatus;
                        int num5;
                        if (currentDeviceStatus8 == null)
                        {
                            num5 = 0;
                        }
                        else
                        {
                            status = currentDeviceStatus8.NoteAcceptor?.Status;
                            DeviceState deviceState = DeviceState.OK;
                            num5 = status.GetValueOrDefault() == deviceState & status.HasValue ? 1 : 0;
                        }
                        if (num5 == 0)
                            goto label_36;
                    }
                    ControllerStatus currentDeviceStatus9 = CurrentDeviceStatus;
                    return currentDeviceStatus9 != null && currentDeviceStatus9.ControllerState == ControllerState.DROP;
                }
            label_36:
                return false;
            }
        }

        public void TransactionEnd()
        {
            if (CurrentState != DeviceManagerState.OUT_OF_ORDER)
                CurrentState = DeviceManagerState.TRANSACTION_ENDING;
            if (HasEscrow)
                DeviceMessenger.TransactionEnd();
            else
                DeviceMessenger.EndCount();
        }

        public override void CashInStart()
        {
            DeviceMessenger.CashInStart(CurrentTransaction.Currency, CurrentTransaction.AccountNumber, CurrentTransaction.SessionID, CurrentTransaction.TransactionID, HasEscrow ? DropMode.MULTIDROP_NOTES : DropMode.DROP_NOTES);
            DeviceTransactionResult e = new DeviceTransactionResult();
            e.level = ErrorLevel.SUCCESS;
            e.data = CurrentTransaction;
            OnCashInStartedEvent(this, e);
        }

        public bool CanCount
        {
            get
            {
                if (CurrentTransaction != null && !ClearHopperRequest && CurrentState != DeviceManagerState.OUT_OF_ORDER)
                {
                    DeviceTransaction currentTransaction = CurrentTransaction;
                    if ((currentTransaction != null ? (currentTransaction.TransactionLimitCents <= 0L ? 1 : 0) : 0) == 0)
                    {
                        long? droppedTotalCents = CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents;
                        long? transactionLimitCents = CurrentTransaction?.TransactionLimitCents;
                        if (!(droppedTotalCents.GetValueOrDefault() <= transactionLimitCents.GetValueOrDefault() & droppedTotalCents.HasValue & transactionLimitCents.HasValue))
                            goto label_21;
                    }
                    ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                    if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
                    {
                        ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                        int num1;
                        if (currentDeviceStatus2 == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
                            DeviceState deviceState = DeviceState.IDLE;
                            num1 = status.GetValueOrDefault() == deviceState & status.HasValue ? 1 : 0;
                        }
                        if (num1 != 0)
                        {
                            if (HasEscrow)
                            {
                                ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                                int num2;
                                if (currentDeviceStatus3 == null)
                                {
                                    num2 = 0;
                                }
                                else
                                {
                                    EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                                    EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                                    num2 = status.GetValueOrDefault() == escrowStatus & status.HasValue ? 1 : 0;
                                }
                                if (num2 == 0)
                                    goto label_21;
                            }
                            ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                            int num3;
                            if (currentDeviceStatus4 == null)
                            {
                                num3 = 0;
                            }
                            else
                            {
                                DeviceTransactionStatus? status = currentDeviceStatus4.Transaction?.Status;
                                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                                num3 = status.GetValueOrDefault() == transactionStatus & status.HasValue ? 1 : 0;
                            }
                            if (num3 != 0)
                                return true;
                            ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                            if (currentDeviceStatus5 == null)
                                return false;
                            DeviceTransactionStatus? status1 = currentDeviceStatus5.Transaction?.Status;
                            DeviceTransactionStatus transactionStatus1 = DeviceTransactionStatus.ACTIVE;
                            return status1.GetValueOrDefault() == transactionStatus1 & status1.HasValue;
                        }
                    }
                }
            label_21:
                return false;
            }
        }

        public void Count()
        {
            if (CurrentTransaction != null)
            {
                if (CurrentTransaction.TransactionValueCents == 0L || CurrentTransaction.TransactionValueCentsLeft > 0L)
                {
                    if (CurrentDeviceStatus == null)
                    {
                        Console.Error.WriteLine("CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()");
                        Log.Warning(GetType().Name, nameof(Count), "Command", "CurrentState = {0}: CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()", new object[1]
                        {
               CurrentState
                        });
                        CurrentState = DeviceManagerState.OUT_OF_ORDER;
                        throw new NullReferenceException("CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()");
                    }
                    if (CanCount && (CurrentState == DeviceManagerState.DROP_STARTING || CurrentState == DeviceManagerState.TRANSACTION_STARTED || CurrentState == DeviceManagerState.NONE))
                    {
                        if (CurrentState != DeviceManagerState.DROP_STARTING)
                        {
                            CurrentState = DeviceManagerState.DROP_STARTING;
                            CurrentTransaction.DropResults.CurrentDropID = Guid.NewGuid().ToString().ToUpperInvariant();
                        }
                        DeviceMessenger.Count(CurrentTransaction.DropResults.CurrentDropID, CurrentTransaction.TransactionValueCentsLeft);
                    }
                    else
                    {
                        if (CurrentState == DeviceManagerState.DROP_STARTED)
                            return;
                        Console.Error.WriteLine(string.Format("Controller not ready for count: CurrentDeviceStatus?.ControllerState={0}: CurrentDeviceStatus?.NoteAcceptor?.Status={1}: CurrentDeviceStatus?.Escrow?.Status={2}: CurrentDeviceStatus?.Transaction?.Status={3}", CurrentDeviceStatus?.ControllerState, CurrentDeviceStatus?.NoteAcceptor?.Status, CurrentDeviceStatus?.Escrow?.Status, CurrentDeviceStatus?.Transaction?.Status));
                        Log.Warning(GetType().Name, nameof(Count), "Command", "CurrentState = {0}: Controller not ready for count: CurrentDeviceStatus?.ControllerState={1}: CurrentDeviceStatus?.NoteAcceptor?.Status={2}: CurrentDeviceStatus?.Escrow?.Status={3}: CurrentDeviceStatus?.Transaction?.Status={4}", new object[5]
                        {
               CurrentState,
               CurrentDeviceStatus?.ControllerState,
               CurrentDeviceStatus?.NoteAcceptor?.Status,
               CurrentDeviceStatus?.Escrow?.Status,
               CurrentDeviceStatus?.Transaction?.Status
                        });
                        CurrentState = DeviceManagerState.OUT_OF_ORDER;
                    }
                }
                else
                {
                    Console.Error.WriteLine(string.Format("transaction limit reached: TotalValue={0}: Limit={1}", CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents, CurrentTransaction?.TransactionLimitCents));
                    Log.Warning(GetType().Name, nameof(Count), "Command", "CurrentState = {0}: transaction limit reached: TotalValue={1}: Limit={2}", new object[3]
                    {
             CurrentState,
             CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents,
             CurrentTransaction?.TransactionLimitCents
                    });
                }
            }
            else
            {
                Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()");
                Log.Warning(GetType().Name, nameof(Count), "Command", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()", new object[1]
                {
           CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                throw new NullReferenceException("CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()");
            }
        }

        public bool CanPauseCount
        {
            get
            {
                if (CurrentState != DeviceManagerState.OUT_OF_ORDER && CurrentTransaction != null)
                {
                    ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                    if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP ? 1 : 0) : 0) != 0)
                    {
                        ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                        int num1;
                        if (currentDeviceStatus2 == null)
                        {
                            num1 = 0;
                        }
                        else
                        {
                            DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
                            DeviceState deviceState = DeviceState.OK;
                            num1 = status.GetValueOrDefault() == deviceState & status.HasValue ? 1 : 0;
                        }
                        if (num1 != 0)
                        {
                            if (HasEscrow)
                            {
                                ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                                int num2;
                                if (currentDeviceStatus3 == null)
                                {
                                    num2 = 0;
                                }
                                else
                                {
                                    EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                                    EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                                    num2 = status.GetValueOrDefault() == escrowStatus & status.HasValue ? 1 : 0;
                                }
                                if (num2 == 0)
                                    goto label_22;
                            }
                            ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                            DeviceTransactionStatus? status1;
                            int num3;
                            if (currentDeviceStatus4 == null)
                            {
                                num3 = 0;
                            }
                            else
                            {
                                status1 = currentDeviceStatus4.Transaction?.Status;
                                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.COUNTING;
                                num3 = status1.GetValueOrDefault() == transactionStatus & status1.HasValue ? 1 : 0;
                            }
                            if (num3 == 0)
                            {
                                ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                                int num4;
                                if (currentDeviceStatus5 == null)
                                {
                                    num4 = 0;
                                }
                                else
                                {
                                    status1 = currentDeviceStatus5.Transaction?.Status;
                                    DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
                                    num4 = status1.GetValueOrDefault() == transactionStatus & status1.HasValue ? 1 : 0;
                                }
                                if (num4 == 0)
                                    goto label_22;
                            }
                            DeviceTransaction currentTransaction = CurrentTransaction;
                            if (currentTransaction == null)
                                return false;
                            DropStatusResultStatus? statusResultStatus1 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                            DropStatusResultStatus statusResultStatus2 = DropStatusResultStatus.DROPPING;
                            return statusResultStatus1.GetValueOrDefault() == statusResultStatus2 & statusResultStatus1.HasValue;
                        }
                    }
                }
            label_22:
                return false;
            }
        }

        public void PauseCount()
        {
            if (CurrentTransaction != null)
            {
                ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                DeviceState? nullable1;
                EscrowStatus? nullable2;
                DeviceTransactionStatus? nullable3;
                if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP ? 1 : 0) : 0) != 0)
                {
                    ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                    int num1;
                    if (currentDeviceStatus2 == null)
                    {
                        num1 = 0;
                    }
                    else
                    {
                        nullable1 = currentDeviceStatus2.NoteAcceptor?.Status;
                        DeviceState deviceState = DeviceState.OK;
                        num1 = nullable1.GetValueOrDefault() == deviceState & nullable1.HasValue ? 1 : 0;
                    }
                    if (num1 != 0)
                    {
                        if (HasEscrow)
                        {
                            ControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                            int num2;
                            if (currentDeviceStatus3 == null)
                            {
                                num2 = 0;
                            }
                            else
                            {
                                nullable2 = currentDeviceStatus3.Escrow?.Status;
                                EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                                num2 = nullable2.GetValueOrDefault() == escrowStatus & nullable2.HasValue ? 1 : 0;
                            }
                            if (num2 == 0)
                                goto label_25;
                        }
                        ControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                        int num3;
                        if (currentDeviceStatus4 == null)
                        {
                            num3 = 0;
                        }
                        else
                        {
                            nullable3 = currentDeviceStatus4.Transaction?.Status;
                            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.COUNTING;
                            num3 = nullable3.GetValueOrDefault() == transactionStatus & nullable3.HasValue ? 1 : 0;
                        }
                        if (num3 == 0)
                        {
                            ControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                            int num4;
                            if (currentDeviceStatus5 == null)
                            {
                                num4 = 0;
                            }
                            else
                            {
                                nullable3 = currentDeviceStatus5.Transaction?.Status;
                                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                                num4 = nullable3.GetValueOrDefault() == transactionStatus & nullable3.HasValue ? 1 : 0;
                            }
                            if (num4 == 0)
                                goto label_25;
                        }
                        DeviceTransaction currentTransaction = CurrentTransaction;
                        int num5;
                        if (currentTransaction == null)
                        {
                            num5 = 0;
                        }
                        else
                        {
                            DropStatusResultStatus? statusResultStatus1 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                            DropStatusResultStatus statusResultStatus2 = DropStatusResultStatus.DROPPING;
                            num5 = statusResultStatus1.GetValueOrDefault() == statusResultStatus2 & statusResultStatus1.HasValue ? 1 : 0;
                        }
                        if (num5 != 0)
                        {
                            DeviceMessenger.RequestPause();
                            return;
                        }
                        Console.Error.WriteLine("DropStatus is " + CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus.ToString() + ", expecting DropStatusResultStatus.DROPPING");
                        return;
                    }
                }
            label_25:
                TextWriter error = Console.Error;
                object[] objArray = new object[4]
                {
           CurrentDeviceStatus?.ControllerState,
          null,
          null,
          null
                };
                ControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
                DeviceState? nullable4;
                if (currentDeviceStatus6 == null)
                {
                    nullable1 = new DeviceState?();
                    nullable4 = nullable1;
                }
                else
                {
                    DeviceNoteAcceptor noteAcceptor = currentDeviceStatus6.NoteAcceptor;
                    if (noteAcceptor == null)
                    {
                        nullable1 = new DeviceState?();
                        nullable4 = nullable1;
                    }
                    else
                        nullable4 = new DeviceState?(noteAcceptor.Status);
                }
                objArray[1] = nullable4;
                ControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
                EscrowStatus? nullable5;
                if (currentDeviceStatus7 == null)
                {
                    nullable2 = new EscrowStatus?();
                    nullable5 = nullable2;
                }
                else
                {
                    DeviceEscrow escrow = currentDeviceStatus7.Escrow;
                    if (escrow == null)
                    {
                        nullable2 = new EscrowStatus?();
                        nullable5 = nullable2;
                    }
                    else
                        nullable5 = new EscrowStatus?(escrow.Status);
                }
                objArray[2] = nullable5;
                ControllerStatus currentDeviceStatus8 = CurrentDeviceStatus;
                DeviceTransactionStatus? nullable6;
                if (currentDeviceStatus8 == null)
                {
                    nullable3 = new DeviceTransactionStatus?();
                    nullable6 = nullable3;
                }
                else
                {
                    ControllerDeviceTransaction transaction = currentDeviceStatus8.Transaction;
                    if (transaction == null)
                    {
                        nullable3 = new DeviceTransactionStatus?();
                        nullable6 = nullable3;
                    }
                    else
                        nullable6 = new DeviceTransactionStatus?(transaction.Status);
                }
                objArray[3] = nullable6;
                string str = string.Format("Controller not ready for pause: CurrentDeviceStatus?.ControllerState={0}: CurrentDeviceStatus?.NoteAcceptor?.Status={1}: CurrentDeviceStatus?.Escrow?.Status={2}: CurrentDeviceStatus?.Transaction?.Status={3}", objArray);
                error.WriteLine(str);
            }
            else
            {
                Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.PauseCount()");
                DeviceMessenger.RequestPause();
            }
        }

        public void StartCIT(string sealNumber) => DeviceMessenger.StartCIT(sealNumber);

        public void EndCIT(string bagnumber) => DeviceMessenger.EndCIT(bagnumber);

        public bool CanEndCount
        {
            get
            {
                if (HasEscrow)
                    return false;
                int num1 = CurrentState != DeviceManagerState.OUT_OF_ORDER ? 1 : 0;
                ControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
                int num2 = currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP_PAUSED ? 1 : 0) : 0;
                if ((num1 & num2) != 0)
                {
                    ControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
                    int num3;
                    if (currentDeviceStatus2 == null)
                    {
                        num3 = 0;
                    }
                    else
                    {
                        DeviceTransactionStatus? status = currentDeviceStatus2.Transaction?.Status;
                        DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.PAUSED;
                        num3 = status.GetValueOrDefault() == transactionStatus & status.HasValue ? 1 : 0;
                    }
                    if (num3 != 0)
                    {
                        DeviceTransaction currentTransaction = CurrentTransaction;
                        if (currentTransaction == null)
                            return false;
                        DropStatusResultStatus? statusResultStatus1 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                        DropStatusResultStatus statusResultStatus2 = DropStatusResultStatus.DONE;
                        return statusResultStatus1.GetValueOrDefault() == statusResultStatus2 & statusResultStatus1.HasValue;
                    }
                }
                return false;
            }
        }

        public bool CanEscrowDrop => HasEscrow && CurrentState == DeviceManagerState.DROP_STOPPED && !ClearHopperRequest && CurrentTransaction?.CurrentTransactionResult != null && CurrentTransaction.CurrentTransactionResult.EscrowTotalCents > 0L && CanPauseCount;

        public void EscrowDrop()
        {
            if (CurrentState != DeviceManagerState.DROP_STOPPED)
                return;
            CurrentState = DeviceManagerState.DROP_ESCROW_ACCEPTING;
            DeviceMessenger.EscrowDrop();
        }

        public bool CanEscrowReject => HasEscrow && CurrentState == DeviceManagerState.DROP_STOPPED && !ClearHopperRequest && CurrentTransaction?.CurrentTransactionResult != null && CanPauseCount;

        public void EscrowReject()
        {
            if (CurrentState != DeviceManagerState.DROP_STOPPED && CurrentState != DeviceManagerState.OUT_OF_ORDER)
                return;
            CurrentState = DeviceManagerState.DROP_ESCROW_REJECTING;
            DeviceMessenger.EscrowReject();
        }

        public void GetTransactionStatus()
        {
            if (!HasEscrow)
                return;
            DeviceMessenger.GetTransactionStatus();
        }

        public void GetStatus() => DeviceMessenger.GetStatus();

        private void NotifyCurrentTransactionStatusChanged()
        {
            NotifyPropertyChanged("CanCount");
            NotifyPropertyChanged("CanPauseCount");
            NotifyPropertyChanged("CanEscrowDrop");
            NotifyPropertyChanged("CanEscrowReject");
            NotifyPropertyChanged("CanTransactionEnd");
            OnNotifyCurrentTransactionStatusChangedEvent(this, EventArgs.Empty);
        }

        public override void ClearEscrowJam() => CashAccSysSerialFix.ClearEscrowJam();

        public override void EndEscrowJam()
        {
            if (CurrentState != DeviceManagerState.ESCROWJAM_END_REQUEST)
                return;
            OnEscrowJamEndEvent(this, EventArgs.Empty);
        }

        private DeviceStatusChangedEventArgs ProcessStatusReport(
          StatusReport statusReport)
        {
            string status1 = statusReport.Status.Bag.Status;
            BagState bagState = status1 == "OK" ? BagState.OK : (status1 == "FULL" ? BagState.FULL : (status1 == "CAPACITY" ? BagState.CAPACITY : (status1 == "CLOSED" ? BagState.CLOSED : (status1 == "BAG_REMOVED" ? BagState.BAG_REMOVED : BagState.ERROR))));
            string position = statusReport.Status.Escrow.Position;
            EscrowPosition escrowPosition = position == "IDLE" ? EscrowPosition.IDLE : (position == "DROP" ? EscrowPosition.DROP : (position == "REJECT" ? EscrowPosition.REJECT : EscrowPosition.NONE));
            string status2 = statusReport.Status.Escrow.Status;
            EscrowStatus escrowStatus = status2 == "IDLE_POS" ? EscrowStatus.IDLE_POS : (status2 == "DROP_POS" ? EscrowStatus.DROP_POS : (status2 == "REJECT_POS" ? EscrowStatus.REJECT_POS : (status2 == "MOVING" ? EscrowStatus.MOVING : EscrowStatus.NONE)));
            string type1 = statusReport.Status.Escrow.Type;
            EscrowType escrowType = type1 == "NO_DEVICE" ? EscrowType.NO_DEVICE : (type1 == "VIRTUAL" ? EscrowType.VIRTUAL : (type1 == "DE50" ? EscrowType.DE50 : (type1 == "GPIO" ? EscrowType.GPIO : EscrowType.NONE)));
            DeviceNoteAcceptorType noteAcceptorType;
            switch (statusReport.Status.Acceptors.BA[0]?.Type)
            {
                case "ATR900":
                    noteAcceptorType = DeviceNoteAcceptorType.ATR900;
                    break;
                case "DE50":
                    noteAcceptorType = DeviceNoteAcceptorType.DE50;
                    break;
                case "DP8120":
                    noteAcceptorType = DeviceNoteAcceptorType.DP8120;
                    break;
                case "GFS120":
                    noteAcceptorType = DeviceNoteAcceptorType.GFS120;
                    break;
                case "JCM":
                    noteAcceptorType = DeviceNoteAcceptorType.JCM;
                    break;
                case "MONIRON":
                    noteAcceptorType = DeviceNoteAcceptorType.MONIRON;
                    break;
                case "NEWTON":
                    noteAcceptorType = DeviceNoteAcceptorType.NEWTON;
                    break;
                case "SHINWOO":
                    noteAcceptorType = DeviceNoteAcceptorType.SHINWOO;
                    break;
                case "VIRTUAL":
                    noteAcceptorType = DeviceNoteAcceptorType.VIRTUAL;
                    break;
                default:
                    noteAcceptorType = DeviceNoteAcceptorType.NONE;
                    break;
            }
            string status3 = statusReport.Status.Acceptors.BA[0]?.Status;
            DeviceState deviceState = status3 == "IDLE" ? DeviceState.IDLE : (status3 == "OK" ? DeviceState.OK : (status3 == "COUNTING" ? DeviceState.COUNTING : (status3 == "JAM" ? DeviceState.JAM : (status3 == "NO_COMMS" ? DeviceState.NO_COMMS : (status3 == "NO_DEVICE" ? DeviceState.NO_DEVICE : DeviceState.NONE)))));
            string status4 = statusReport.Status.Sensors.Status;
            DeviceSensorState deviceSensorState = status4 == "OK" ? DeviceSensorState.OK : (status4 == "NO_COMMS" ? DeviceSensorState.NO_COMMS : (status4 == "NO_DEVICE" ? DeviceSensorState.NO_DEVICE : DeviceSensorState.NONE));
            int result;
            int num = int.TryParse(statusReport.Status.Sensors.Value, NumberStyles.HexNumber, new CultureInfo("en-US"), out result) ? result : 0;
            string type2 = statusReport.Status.Sensors.Type;
            DeviceSensorType deviceSensorType = type2 == "SITECH" ? DeviceSensorType.SITECH : (type2 == "VIRTUAL" ? DeviceSensorType.VIRTUAL : (type2 == "NUMATO" ? DeviceSensorType.NUMATO : DeviceSensorType.NONE));
            DeviceSensorDoor deviceSensorDoor;
            if (SENSOR_INVERT_DOOR)
            {
                string door = statusReport.Status.Sensors.Door;
                deviceSensorDoor = door == "CLOSED" ? DeviceSensorDoor.OPEN : (door == "OPEN" ? DeviceSensorDoor.CLOSED : DeviceSensorDoor.NONE);
            }
            else
            {
                string door = statusReport.Status.Sensors.Door;
                deviceSensorDoor = door == "OPEN" ? DeviceSensorDoor.OPEN : (door == "CLOSED" ? DeviceSensorDoor.CLOSED : DeviceSensorDoor.NONE);
            }
            string bag = statusReport.Status.Sensors.Bag;
            DeviceSensorBag deviceSensorBag = bag == "PRESENT" ? DeviceSensorBag.PRESENT : (bag == "REMOVED" ? DeviceSensorBag.REMOVED : DeviceSensorBag.NONE);
            ControllerState controllerState;
            switch (statusReport.Status.ControllerState)
            {
                case "DISPENSE":
                    controllerState = ControllerState.DISPENSE;
                    break;
                case "DROP":
                    controllerState = ControllerState.DROP;
                    break;
                case "DROP_PAUSED":
                    controllerState = ControllerState.DROP_PAUSED;
                    break;
                case "ESCROW_DROP":
                    controllerState = ControllerState.ESCROW_DROP;
                    break;
                case "ESCROW_REJECT":
                    controllerState = ControllerState.ESCROW_REJECT;
                    break;
                case "IDLE":
                    controllerState = ControllerState.IDLE;
                    break;
                case "INIT":
                    controllerState = ControllerState.INIT;
                    break;
                case "OUT_OF_ORDER":
                    controllerState = ControllerState.OUT_OF_ORDER;
                    break;
                default:
                    controllerState = ControllerState.NONE;
                    break;
            }
            string status5 = statusReport.Status.Transaction.Status;
            DeviceTransactionStatus transactionStatus;
            if (!(status5 == "NONE"))
            {
                switch (status5)
                {
                    case "":
                    case null:
                        break;
                    case "ACTIVE":
                        transactionStatus = DeviceTransactionStatus.ACTIVE;
                        goto label_34;
                    case "COUNTING":
                        transactionStatus = DeviceTransactionStatus.COUNTING;
                        goto label_34;
                    case "DISPENSING":
                        transactionStatus = DeviceTransactionStatus.DISPENSING;
                        goto label_34;
                    case "ESCROW_DROP":
                        transactionStatus = DeviceTransactionStatus.ESCROW_DROP;
                        goto label_34;
                    case "ESCROW_REJECT":
                        transactionStatus = DeviceTransactionStatus.ESCROW_REJECT;
                        goto label_34;
                    case "PAUSED":
                        transactionStatus = DeviceTransactionStatus.PAUSED;
                        goto label_34;
                    default:
                        transactionStatus = DeviceTransactionStatus.ERROR;
                        goto label_34;
                }
            }
            transactionStatus = DeviceTransactionStatus.NONE;
        label_34:
            string type3 = statusReport.Status.Transaction.Type;
            DeviceTransactionType deviceTransactionType = type3 == "NONE" ? DeviceTransactionType.NONE : (type3 == "DROP" ? DeviceTransactionType.DROP : (type3 == "EXCHANGE" ? DeviceTransactionType.EXCHANGE : (type3 == "MULTIDROP" ? DeviceTransactionType.MULTIDROP : (type3 == "DISPENSE:AMOUNT" ? DeviceTransactionType.DISPENSE_AMOUNT : (type3 == "DISPENSE:NOTES" ? DeviceTransactionType.DISPENSE_NOTES : DeviceTransactionType.NONE)))));
            ControllerDeviceTransaction deviceTransaction = new ControllerDeviceTransaction()
            {
                Status = transactionStatus,
                Type = deviceTransactionType
            };
            return new DeviceStatusChangedEventArgs(new ControllerStatus()
            {
                Bag = new DeviceBag()
                {
                    NoteLevel = statusReport.Status.Bag.NoteLevel,
                    NoteCapacity = statusReport.Status.Bag.NoteCapacity,
                    BagNumber = statusReport.Status.Bag.Number,
                    BagState = bagState,
                    PercentFull = statusReport.Status.Bag.PercentFull,
                    ValueCapacity = statusReport.Status.Bag.ValueCapacity,
                    ValueLevel = statusReport.Status.Bag.ValueLevel
                },
                Escrow = new DeviceEscrow()
                {
                    Type = escrowType,
                    Position = escrowPosition,
                    Status = escrowStatus
                },
                NoteAcceptor = new DeviceNoteAcceptor()
                {
                    Currency = statusReport.Status.Acceptors.BA[0]?.Currency,
                    Type = noteAcceptorType,
                    Status = deviceState
                },
                Sensor = new DeviceSensor()
                {
                    Status = deviceSensorState,
                    Value = num,
                    Type = deviceSensorType,
                    Bag = deviceSensorBag,
                    Door = deviceSensorDoor
                },
                ControllerState = controllerState,
                Transaction = deviceTransaction
            });
        }

        private DropStatusResult ProcessDropStatus(DropStatus dropStatus)
        {
            DropStatusResultStatus statusResultStatus1;
            switch (dropStatus?.Body?.Status)
            {
                case "COUNTING":
                    statusResultStatus1 = DropStatusResultStatus.COUNTING;
                    break;
                case "DONE":
                    statusResultStatus1 = DropStatusResultStatus.DONE;
                    break;
                case "DROPPING":
                    statusResultStatus1 = DropStatusResultStatus.DROPPING;
                    break;
                case "ESCROW_DONE":
                    statusResultStatus1 = DropStatusResultStatus.ESCROW_DONE;
                    break;
                case "ESCROW_DROP":
                    statusResultStatus1 = DropStatusResultStatus.ESCROW_DROP;
                    break;
                case "ESCROW_REJECT":
                    statusResultStatus1 = DropStatusResultStatus.ESCROW_REJECT;
                    break;
                case "ESCROW_WAIT":
                    statusResultStatus1 = DropStatusResultStatus.ESCROW_WAIT;
                    break;
                case "WAIT_ACCEPTOR":
                    statusResultStatus1 = DropStatusResultStatus.WAIT_ACCEPTOR;
                    break;
                default:
                    statusResultStatus1 = DropStatusResultStatus.ERROR;
                    break;
            }
            try
            {
                DeviceTransaction currentTransaction = CurrentTransaction;
                int num;
                if (currentTransaction == null)
                {
                    num = 0;
                }
                else
                {
                    DropStatusResultStatus? statusResultStatus2 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                    DropStatusResultStatus statusResultStatus3 = statusResultStatus1;
                    num = statusResultStatus2.GetValueOrDefault() == statusResultStatus3 & statusResultStatus2.HasValue ? 1 : 0;
                }
                if (num != 0)
                {
                    long? totalCount1 = CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DenominationResult?.data?.TotalCount;
                    int? totalCount2 = dropStatus?.Body?.TotalCount;
                    long? nullable = totalCount2.HasValue ? new long?(totalCount2.GetValueOrDefault()) : new long?();
                    if (totalCount1.GetValueOrDefault() == nullable.GetValueOrDefault() & totalCount1.HasValue == nullable.HasValue)
                        return CurrentTransaction.CurrentTransactionResult.CurrentDropStatus;
                }
            }
            catch (Exception ex)
            {
            }
            List<DenominationItem> denominationItemList = new List<DenominationItem>();
            foreach (NoteCount noteCount in dropStatus.Body.NoteCounts.NoteCount)
                denominationItemList.Add(new DenominationItem()
                {
                    count = noteCount.Count,
                    Currency = noteCount.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = noteCount.Denomination
                });
            DenominationResult denominationResult1 = new DenominationResult();
            denominationResult1.level = ErrorLevel.SUCCESS;
            denominationResult1.resultCode = 0;
            denominationResult1.data = new Denomination()
            {
                DenominationItems = denominationItemList
            };
            DenominationResult denominationResult2 = denominationResult1;
            DropStatusResult dropStatusResult1 = new DropStatusResult()
            {
                data = new DropStatusResultData()
                {
                    DenominationResult = denominationResult2,
                    DropStatusResultStatus = statusResultStatus1
                }
            };
            DropStatusResult dropStatusResult2 = dropStatusResult1;
            long totalValue = dropStatusResult1.data.DenominationResult.data.TotalValue;
            long? nullable1 = dropStatus?.Body?.TotalValue;
            long valueOrDefault1 = nullable1.GetValueOrDefault();
            int num1;
            if (totalValue == valueOrDefault1 & nullable1.HasValue)
            {
                long totalCount = dropStatusResult1.data.DenominationResult.data.TotalCount;
                int? nullable2 = dropStatus?.Body?.TotalCount;
                nullable1 = nullable2.HasValue ? new long?(nullable2.GetValueOrDefault()) : new long?();
                long valueOrDefault2 = nullable1.GetValueOrDefault();
                if (totalCount == valueOrDefault2 & nullable1.HasValue)
                {
                    int? nullable3;
                    if (dropStatus == null)
                    {
                        nullable2 = new int?();
                        nullable3 = nullable2;
                    }
                    else
                    {
                        DropStatusBody body = dropStatus.Body;
                        if (body == null)
                        {
                            nullable2 = new int?();
                            nullable3 = nullable2;
                        }
                        else
                        {
                            NoteCounts noteCounts = body.NoteCounts;
                            if (noteCounts == null)
                            {
                                nullable2 = new int?();
                                nullable3 = nullable2;
                            }
                            else
                            {
                                List<NoteCount> noteCount = noteCounts.NoteCount;
                                if (noteCount == null)
                                {
                                    nullable2 = new int?();
                                    nullable3 = nullable2;
                                }
                                else
                                {
                                    nullable3 = noteCount.Count;
                                }
                            }
                        }
                    }
                    nullable2 = nullable3;
                    if (nullable2.GetValueOrDefault() > 0)
                    {
                        num1 = 0;
                        goto label_37;
                    }
                }
            }
            num1 = 2;
        label_37:
            dropStatusResult2.level = (ErrorLevel)num1;
            return dropStatusResult1;
        }

        private DropResultResult ProcessDropResult(DropResult dropResult)
        {
            List<DenominationItem> denominationItemList = new List<DenominationItem>();
            foreach (NoteCount noteCount in dropResult.Body.NoteCounts.NoteCount)
                denominationItemList.Add(new DenominationItem()
                {
                    count = noteCount.Count,
                    Currency = noteCount.Currency,
                    type = DenominationItemType.NOTE,
                    denominationValue = noteCount.Denomination
                });
            DropMode dropMode;
            switch (dropResult?.Body?.DropMode)
            {
                case "DROP:COINS":
                    dropMode = DropMode.DROP_COINS;
                    break;
                case "DROP:NOTES":
                    dropMode = DropMode.DROP_NOTES;
                    break;
                case "EXCHANGE:COINS":
                    dropMode = DropMode.EXCHANGE_COINS;
                    break;
                case "EXCHANGE:NOTES":
                    dropMode = DropMode.EXCHANGE_NOTES;
                    break;
                case "MULTIDROP:COINS":
                    dropMode = DropMode.MULTIDROP_COINS;
                    break;
                case "MULTIDROP:NOTES":
                    dropMode = DropMode.MULTIDROP_NOTES;
                    break;
                case "PAYMENT:NOTES":
                    dropMode = DropMode.PAYMENT_NOTES;
                    break;
                default:
                    dropMode = DropMode.NONE;
                    break;
            }
            DropResultResult dropResultResult = new DropResultResult();
            dropResultResult.DropDeviceID = dropResult?.Body?.DeviceSerialNumber;
            dropResultResult.SessionID = dropResult?.Body?.InputNumber;
            dropResultResult.TransactionID = dropResult?.Body?.Reference;
            dropResultResult.DropID = dropResult?.Body?.InputSubNumber;
            dropResultResult.level = dropResult.Body.NoteJam > 0 ? ErrorLevel.ERROR : ErrorLevel.SUCCESS;
            int? nullable1 = dropResult?.Body?.TotalNumberOfNotes;
            dropResultResult.TotalNumberOfNotes = nullable1.GetValueOrDefault();
            dropResultResult.DroppedAmountCents = (long)(dropResult?.Body?.TranAmount);
            int? nullable2;
            if (dropResult == null)
            {
                nullable1 = new int?();
                nullable2 = nullable1;
            }
            else
            {
                DropResultBody body = dropResult.Body;
                if (body == null)
                {
                    nullable1 = new int?();
                    nullable2 = nullable1;
                }
                else
                    nullable2 = new int?(body.TranCycle);
            }
            nullable1 = nullable2;
            dropResultResult.TransactionNumber = nullable1.ToString() + string.Empty;
            dropResultResult.DropMode = dropMode;
            dropResultResult.isMultiDrop = dropMode == DropMode.DROP_NOTES || dropMode == DropMode.DROP_COINS;
            DenominationResult denominationResult = new DenominationResult();
            denominationResult.level = dropResult.Body.NoteJam > 0 ? ErrorLevel.ERROR : ErrorLevel.SUCCESS;
            denominationResult.resultCode = 0;
            denominationResult.NoteJamDetected = dropResult.Body.NoteJam > 0;
            denominationResult.NotesRejected = dropResult.Body.Rejected > 0;
            denominationResult.data = new Denomination()
            {
                DenominationItems = denominationItemList
            };
            dropResultResult.DroppedDenomination = denominationResult;
            return dropResultResult;
        }

        private TransactionStatusResponseResult ProcessTransactionStatusResponse(
          TransactionStatusResponse transactionStatusResponse)
        {
            string result = transactionStatusResponse?.Body?.Result;
            TransactionResultResult transactionResultResult = result == "SUCCESS" ? TransactionResultResult.SUCCESS : (result == "REJECTED" ? TransactionResultResult.REJECTED : TransactionResultResult.NONE);
            string transactionStatus = transactionStatusResponse?.Body?.TransactionStatus;
            TransactionResultStatus transactionResultStatus = transactionStatus == "NONE" ? TransactionResultStatus.NONE : (transactionStatus == "ACTIVE" ? TransactionResultStatus.IDLE : (transactionStatus == "COUNTING" ? TransactionResultStatus.COUNTING : (transactionStatus == "PAUSED" ? TransactionResultStatus.PAUSED : (transactionStatus == "ESCROW_DROP" ? TransactionResultStatus.ESCROW_DROP : (transactionStatus == "ESCROW_REJECT" ? TransactionResultStatus.ESCROW_REJECT : TransactionResultStatus.ERROR)))));
            TransactionResultType transactionResultType = !(transactionStatusResponse?.Body?.TransactionType == "MULTIDROP") ? TransactionResultType.ERROR : TransactionResultType.MULTIDROP;
            TransactionStatusResponseResult statusResponseResult = new TransactionStatusResponseResult();
            TransactionStatusResponseData statusResponseData = new TransactionStatusResponseData(transactionStatusResponse?.Body?.InputNumber, transactionStatusResponse?.Body?.Reference);
            long? nullable1 = transactionStatusResponse?.Body?.DispensedAmount;
            statusResponseData.DispensedAmountCents = nullable1.GetValueOrDefault();
            Denomination denomination1;
            if (transactionStatusResponse == null)
            {
                denomination1 = null;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                denomination1 = body != null ? body.DispensedNotes.CreateDenomination() : null;
            }
            statusResponseData.DispensedNotes = denomination1;
            long? nullable2;
            if (transactionStatusResponse == null)
            {
                nullable1 = new long?();
                nullable2 = nullable1;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                if (body == null)
                {
                    nullable1 = new long?();
                    nullable2 = nullable1;
                }
                else
                    nullable2 = new long?(body.LastDroppedAmount);
            }
            nullable1 = nullable2;
            statusResponseData.LastDroppedAmountCents = nullable1.GetValueOrDefault();
            Denomination denomination2;
            if (transactionStatusResponse == null)
            {
                denomination2 = null;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                denomination2 = body != null ? body.LastDroppedNotes.CreateDenomination() : null;
            }
            statusResponseData.LastDroppedNotes = denomination2;
            statusResponseData.NumberOfDrops = (int)(transactionStatusResponse?.Body?.NumberOfDrops);
            long? nullable3;
            if (transactionStatusResponse == null)
            {
                nullable1 = new long?();
                nullable3 = nullable1;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                if (body == null)
                {
                    nullable1 = new long?();
                    nullable3 = nullable1;
                }
                else
                    nullable3 = new long?(body.RequestedDispenseAmount);
            }
            nullable1 = nullable3;
            statusResponseData.RequestedDispenseAmount = nullable1.GetValueOrDefault();
            Denomination denomination3;
            if (transactionStatusResponse == null)
            {
                denomination3 = null;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                denomination3 = body != null ? body.RequestedDispenseNotes.CreateDenomination() : null;
            }
            statusResponseData.RequestedDispenseNotes = denomination3;
            long? nullable4;
            if (transactionStatusResponse == null)
            {
                nullable1 = new long?();
                nullable4 = nullable1;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                if (body == null)
                {
                    nullable1 = new long?();
                    nullable4 = nullable1;
                }
                else
                    nullable4 = new long?(body.RequestedDropAmount);
            }
            nullable1 = nullable4;
            statusResponseData.RequestedDropAmount = nullable1.GetValueOrDefault();
            long? nullable5;
            if (transactionStatusResponse == null)
            {
                nullable1 = new long?();
                nullable5 = nullable1;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                if (body == null)
                {
                    nullable1 = new long?();
                    nullable5 = nullable1;
                }
                else
                    nullable5 = new long?(body.TotalDroppedAmount);
            }
            nullable1 = nullable5;
            statusResponseData.TotalDroppedAmountCents = nullable1.GetValueOrDefault();
            Denomination denomination4;
            if (transactionStatusResponse == null)
            {
                denomination4 = null;
            }
            else
            {
                Body body = transactionStatusResponse.Body;
                denomination4 = body != null ? body.TotalDroppedNotes.CreateDenomination() : null;
            }
            statusResponseData.TotalDroppedNotes = denomination4;
            statusResponseData.Result = transactionResultResult;
            statusResponseData.Status = transactionResultStatus;
            statusResponseData.Type = transactionResultType;
            statusResponseResult.data = statusResponseData;
            return statusResponseResult;
        }
        /*
            private List<(DateTime FileDate, FileInfo File)> GetCashAccSysLogFiles(
              DateTime StartDate,
              DateTime EndDate)
            {
              List<string> list = Directory.GetFiles(CONTROLLER_LOG_DIRECTORY, "TRACE*.log", SearchOption.TopDirectoryOnly).Reverse<string>().ToList<string>();
              List<(DateTime, FileInfo)> source = new List<(DateTime, FileInfo)>();
              foreach (string fileName in list)
              {
                FileInfo fileInfo = new FileInfo(fileName);
                DateTime exact = DateTime.ParseExact(fileInfo.Name.Substring(5, 14), "yyyyMMddHHmmss", new CultureInfo("en-US"), DateTimeStyles.None);
                if (StartDate <= exact)
                {
                  source.Add((exact, fileInfo));
                }
                else
                {
                  source.Add((exact, fileInfo));
                  break;
                }
              }
              DateTime startLogDate = source.Where(x => x.FileDate <= StartDate)).OrderByDescending<(DateTime, FileInfo), DateTime>((Func<(DateTime, FileInfo), DateTime>) (y => y.FileDate)).FirstOrDefault<(DateTime, FileInfo)>().Item1;
              DateTime endLogDate = source.Where<(DateTime, FileInfo)>((Func<(DateTime, FileInfo), bool>) (x => x.FileDate <= EndDate)).OrderByDescending<(DateTime, FileInfo), DateTime>((Func<(DateTime, FileInfo), DateTime>) (y => y.FileDate)).FirstOrDefault<(DateTime, FileInfo)>().Item1;
              return source.Where<(DateTime, FileInfo)>((Func<(DateTime, FileInfo), bool>) (x => x.FileDate >= startLogDate && x.FileDate <= endLogDate)).ToList<(DateTime, FileInfo)>();
            }

            private bool IsEscrowJam(List<(DateTime FileDate, FileInfo File)> logFiles, DateTime startDate)
            {
              foreach (FileInfo fileInfo in logFiles.Select<(DateTime, FileInfo), FileInfo>((Func<(DateTime, FileInfo), FileInfo>) (x => x.File)).ToList<FileInfo>())
              {
                string str = File.ReadAllLines(fileInfo.FullName).Reverse<string>().ToList<string>().FirstOrDefault<string>(x => x.Contains("JamOpenEscrowWait"));
                if (str != null && DateTime.ParseExact(fileInfo.Name.Substring(5, 8) + str.Substring(1, 12), "yyyyMMddHH:mm:ss.fff", new CultureInfo("en-US"), DateTimeStyles.None) >= startDate)
                  return true;
              }
              return false;
            }
        */
        private void CashAccSysSerialFix_DE50StatusChangedEvent(
          object sender,
          DE50StatusChangedResult e)
        {
            if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM && e.Status[4] == 'R')
                OnEscrowJamStartEvent(this, EventArgs.Empty);
            else if (DeviceManagerMode == DeviceManagerMode.ESCROW_JAM && CurrentState != DeviceManagerState.ESCROWJAM_CLEAR_WAIT && e.Status[4] == 'I')
            {
                OnEscrowJamClearWaitEvent(this, EventArgs.Empty);
            }
            else
            {
                if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM || CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST || e.Status[4] != '@')
                    return;
                OnEscrowJamEndRequestEvent(this, EventArgs.Empty);
            }
        }
    }
}
