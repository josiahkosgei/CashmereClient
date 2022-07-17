
//CashAccSysDeviceManager
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
using CashAccSysDeviceManager.MessageClasses;
using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;


namespace CashAccSysDeviceManager
{
  public class CashAccSysDeviceManager : DeviceManagerBase
  {
    private bool clearHopperRequest;
    private string DEVICE_PORT;
    private string CONTROLLER_PORT;
    private int BAGFULL_WARN_PERCENT;
    private bool SENSOR_INVERT_DOOR;
    private string CONTROLLER_LOG_DIRECTORY;
    public CDCMessenger DeviceMessenger;
    private string _connectionStatus = "Disconnected";
    private bool _acceptNotes = true;
    private bool _acceptCoins = false;
    private long _requestAmount;
    private int _userID;
    private bool _multiDrop;
    private string _tcpStatus = "Disconnected";
    private StringBuilder _comLog = new StringBuilder();
    private string _currentCurrency;
    private bool _canTransactionEnd = false;

    public NoteCounts CurrentCount { get; set; } = new NoteCounts()
    {
      NoteCount = new List<NoteCount>()
      {
        new NoteCount()
        {
          Denomination = 5000,
          Count = 0,
          Currency = "KES"
        },
        new NoteCount()
        {
          Denomination = 10000,
          Count = 0,
          Currency = "KES"
        },
        new NoteCount()
        {
          Denomination = 20000,
          Count = 0,
          Currency = "KES"
        },
        new NoteCount()
        {
          Denomination = 50000,
          Count = 0,
          Currency = "KES"
        },
        new NoteCount()
        {
          Denomination = 100000,
          Count = 0,
          Currency = "KES"
        }
      }
    };

    public bool ClearHopperRequest
    {
      get => clearHopperRequest;
      set
      {
        if (clearHopperRequest == value)
          return;
        clearHopperRequest = value;
        NotifyPropertyChanged(nameof (ClearHopperRequest));
      }
    }

    public override Version DeviceManagerVersion => Assembly.GetExecutingAssembly().GetName().Version;

    protected new CashmereLogger Log { get; set; }

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
      Log = new CashmereLogger(Assembly.GetAssembly(typeof (DeviceMessageBase)).GetName().Version.ToString(), "DeviceManagerLog", (IConfiguration) null);
      Log.Debug(GetType().Name, "Constructor", "Initialisation", "Creating CashAccSysDeviceManager", Array.Empty<object>());
      this.DEVICE_PORT = DEVICE_PORT;
      this.CONTROLLER_PORT = CONTROLLER_PORT;
      this.BAGFULL_WARN_PERCENT = BAGFULL_WARN_PERCENT;
      this.SENSOR_INVERT_DOOR = SENSOR_INVERT_DOOR;
      this.CONTROLLER_LOG_DIRECTORY = CONTROLLER_LOG_DIRECTORY;
      DeviceMessenger = new CDCMessenger(host, port, macAddress, clientID, clientType, messagSendInterval);
      DeviceMessenger.ConnectionEvent += OnConnectionEvent;
      DeviceMessenger.StatusReportEvent += OnStatusReportEvent;
      DeviceMessenger.DropStatusResultEvent += this.OnDropStatusResultEvent;
      DeviceMessenger.TransactionStatusEvent += OnTransactionStatusEvent;
      DeviceMessenger.DropResultEvent += OnDropResultEvent;
      DeviceMessenger.CITResultEvent += OnCITResultEvent;
      CashAccSysSerialFix = CashAccSysSerialFix.GetInstance(DEVICE_PORT, CONTROLLER_PORT);
      CashAccSysSerialFix.DE50StatusChangedEvent += CashAccSysSerialFix_DE50StatusChangedEvent;
      CashAccSysSerialFix.PropertyChanged += CashAccSysSerialFix_PropertyChanged;
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

    public string ConnectionStatus
    {
      get => _connectionStatus;
      set
      {
        _connectionStatus = value;
        NotifyPropertyChanged(nameof (ConnectionStatus));
      }
    }

    public override void Connect() => DeviceMessenger.Connect();

    public bool AcceptNotes
    {
      get => _acceptNotes;
      set
      {
        _acceptNotes = value;
        NotifyPropertyChanged(nameof (AcceptNotes));
      }
    }

    public bool AcceptCoins
    {
      get => _acceptCoins;
      set
      {
        _acceptCoins = value;
        NotifyPropertyChanged(nameof (AcceptCoins));
      }
    }

    public long RequestAmount
    {
      get => _requestAmount;
      set
      {
        _requestAmount = value;
        NotifyPropertyChanged(nameof (RequestAmount));
      }
    }

    public int UserID
    {
      get => _userID;
      set
      {
        _userID = value;
        NotifyPropertyChanged(nameof (UserID));
      }
    }

    public bool MultiDrop
    {
      get => _multiDrop;
      set
      {
        _multiDrop = value;
        NotifyPropertyChanged(nameof (MultiDrop));
      }
    }

    public DropStatusResult DropStatus => CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus;

    public string TCPStatus
    {
      get => _tcpStatus;
      set
      {
        _tcpStatus = value;
        NotifyPropertyChanged(nameof (TCPStatus));
      }
    }

    public string ComLog
    {
      get => _comLog.ToString();
      set
      {
        _comLog.AppendLine(value);
        Console.WriteLine(string.Format("[{0}] COMLOG {1}", (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), (object) value));
        NotifyPropertyChanged(nameof (ComLog));
      }
    }

    public string CurrentCurrency
    {
      get => _currentCurrency;
      set
      {
        _currentCurrency = value;
        NotifyPropertyChanged(nameof (CurrentCurrency));
      }
    }

    private void SelectCurrency(string currency) => DeviceMessenger.SetCurrency(currency);

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
      OnConnectionEvent((object) this, e);
    }

    public new void OnTransactionStartedEvent(object sender, DeviceTransaction e)
    {
      Log.Debug(GetType().Name, nameof (OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: Handling TransactionStartedEvent", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentState == DeviceManagerState.TRANSACTION_STARTING)
      {
        CurrentState = DeviceManagerState.TRANSACTION_STARTED;
        Log.Info(GetType().Name, nameof (OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: TransactionStartedEvent complete, re-raising event", new object[1]
        {
          (object) CurrentState
        });
        base.OnTransactionStartedEvent((object) this, e);
      }
      else
      {
        Log.Warning(GetType().Name, nameof (OnTransactionStartedEvent), "EventHandler", "CurrentState = {0}: TransactionStartedEvent outside DeviceManagerState.TRANSACTION_STARTING", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
    }

    private void OnDropStatusResultEvent(object sender, MessageClasses.DropStatus dropStatus)
    {
      Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Handling DropStatusResultEvent", new object[1]
      {
        (object) CurrentState
      });
      DropStatusResult dropStatusResult = ProcessDropStatus(dropStatus);
      CashmereLogger log = Log;
      string name = GetType().Name;
      object[] objArray = new object[3]
      {
        (object) CurrentState,
        null,
        null
      };
      DropStatusResultStatus statusResultStatus1;
      string str;
      if (dropStatusResult == null)
      {
        str = (string) null;
      }
      else
      {
        DropStatusResultData data = dropStatusResult.data;
        if (data == null)
        {
          str = (string) null;
        }
        else
        {
          statusResultStatus1 = data.DropStatusResultStatus;
          str = statusResultStatus1.ToString();
        }
      }
      objArray[1] = (object) str;
      objArray[2] = (object) dropStatusResult?.data?.DenominationResult?.ToString();
      log.Info(name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: DropStatusResultStatus = {1} Denomination = {2}", objArray);
      if (CurrentState == DeviceManagerState.DROP_STARTING)
      {
        CurrentState = DeviceManagerState.DROP_STARTED;
        Log.Info(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState changed from DROP_STARTING to DROP_STARTED", Array.Empty<object>());
      }
      if (CurrentTransaction != null)
      {
        if (CurrentTransaction.DropResults.CurrentDropID != null)
        {
          if (dropStatusResult.level != ErrorLevel.ERROR)
          {
            DropStatusResultStatus? statusResultStatus2 = CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
            CurrentTransaction.CurrentTransactionResult.CurrentDropStatus = dropStatusResult;
            DropStatusResultStatus? nullable = statusResultStatus2;
            statusResultStatus1 = dropStatusResult.data.DropStatusResultStatus;
            if (!(nullable == statusResultStatus1 & nullable.HasValue))
            {
              Log.Info(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: DropStatus has changed from {1} to {2}", new object[3]
              {
                (object) CurrentState,
                (object) statusResultStatus2,
                (object) dropStatusResult.data.DropStatusResultStatus
              });
              switch (dropStatusResult.data.DropStatusResultStatus)
              {
                case DropStatusResultStatus.DROPPING:
                  CurrentState = DeviceManagerState.DROP_STOPPED;
                  break;
                case DropStatusResultStatus.ESCROW_REJECT:
                  Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowRejectEvent", new object[1]
                  {
                    (object) CurrentState
                  });
                  DeviceTransactionResult e1 = new DeviceTransactionResult
                  {
                      level = dropStatusResult.level,
                      data = CurrentTransaction
                  };
                  OnEscrowRejectEvent((object) this, e1);
                  break;
                case DropStatusResultStatus.ESCROW_DROP:
                  Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowDropEvent", new object[1]
                  {
                    (object) CurrentState
                  });
                  DeviceTransactionResult e2 = new DeviceTransactionResult
                  {
                      level = dropStatusResult.level,
                      data = CurrentTransaction
                  };
                  OnEscrowDropEvent((object) this, e2);
                  break;
                case DropStatusResultStatus.ESCROW_DONE:
                  Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowOperationCompleteEvent", new object[1]
                  {
                    (object) CurrentState
                  });
                  DeviceTransactionResult e3 = new DeviceTransactionResult
                  {
                      level = dropStatusResult.level,
                      data = CurrentTransaction
                  };
                  OnEscrowOperationCompleteEvent((object) this, e3);
                  break;
                case DropStatusResultStatus.DONE:
                  GetTransactionStatus();
                  Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnCountEndEvent", new object[1]
                  {
                    (object) CurrentState
                  });
                  DeviceTransactionResult e4 = new DeviceTransactionResult
                  {
                      level = dropStatusResult.level,
                      data = CurrentTransaction
                  };
                  OnCountEndEvent((object) this, e4);
                  break;
              }
            }
            Log.Debug(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: Raising OnTransactionStatusEvent", new object[1]
            {
              (object) CurrentState
            });
            OnTransactionStatusEvent((object) this, (TransactionStatusResponse) null);
          }
        }
        else
          Log.Warning(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: dropstatus received outside of a drop", new object[1]
          {
            (object) CurrentState
          });
      }
      else
      {
        Log.Warning(GetType().Name, nameof (OnDropStatusResultEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnDropStatusResultEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      NotifyCurrentTransactionStatusChanged();
    }

    protected void OnStatusReportEvent(object sender, StatusReport statusReport)
    {
      lock (StatusChangeLock)
      {
        if (CurrentState == DeviceManagerState.INIT)
        {
          CurrentState = DeviceManagerState.NONE;
          Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from INIT to NONE", new object[1]
          {
            (object) CurrentState
          });
        }
        DeviceStatusChangedEventArgs deviceStatusResult = ProcessStatusReport(statusReport);
        DeviceState = deviceStatusResult.ControllerStatus.NoteAcceptor.Status;
        ControllerState = deviceStatusResult.ControllerStatus.ControllerState;
        HasEscrow = deviceStatusResult.ControllerStatus.Escrow.Type != EscrowType.NONE && deviceStatusResult.ControllerStatus.Escrow.Type != EscrowType.NO_DEVICE;
        if (CurrentDeviceStatus.Bag.BagState != deviceStatusResult.ControllerStatus.Bag.BagState)
        {
          if (deviceStatusResult.ControllerStatus.Bag.BagState == BagState.CLOSED)
            OnBagClosedEvent((object) this, EventArgs.Empty);
          else if (CurrentDeviceStatus.Bag.BagState != BagState.NONE && deviceStatusResult.ControllerStatus.Bag.BagState == BagState.OK)
            OnBagOpenedEvent((object) this, EventArgs.Empty);
        }
        if (CurrentDeviceStatus.Sensor.Bag != deviceStatusResult.ControllerStatus.Sensor.Bag)
        {
          if (CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.PRESENT && deviceStatusResult.ControllerStatus.Sensor.Bag == DeviceSensorBag.REMOVED)
            OnBagRemovedEvent((object) this, EventArgs.Empty);
          else if (CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.REMOVED && deviceStatusResult.ControllerStatus.Sensor.Bag == DeviceSensorBag.PRESENT)
            OnBagPresentEvent((object) this, EventArgs.Empty);
        }
        if (CurrentDeviceStatus.Bag.BagState == BagState.BAG_REMOVED && CurrentDeviceStatus.Sensor.Bag == DeviceSensorBag.PRESENT)
          EndCIT("12345");
        if (CurrentDeviceStatus.Bag.PercentFull != deviceStatusResult.ControllerStatus.Bag.PercentFull)
        {
          if (deviceStatusResult.ControllerStatus.Bag.PercentFull >= 100)
            OnBagFullAlertEvent((object) this, deviceStatusResult.ControllerStatus);
          else if (deviceStatusResult.ControllerStatus.Bag.PercentFull >= BAGFULL_WARN_PERCENT)
            OnBagFullWarningEvent((object) this, deviceStatusResult.ControllerStatus);
        }
        if (CurrentDeviceStatus.Sensor.Door != DeviceSensorDoor.NONE && CurrentDeviceStatus.Sensor.Door != deviceStatusResult.ControllerStatus.Sensor.Door)
        {
          if (deviceStatusResult.ControllerStatus.Sensor.Door == DeviceSensorDoor.OPEN)
            OnDoorOpenEvent((object) this, EventArgs.Empty);
          else if (deviceStatusResult.ControllerStatus.Sensor.Door == DeviceSensorDoor.CLOSED)
            OnDoorClosedEvent((object) this, EventArgs.Empty);
        }
        if (CurrentDeviceStatus.ControllerState != deviceStatusResult.ControllerStatus.ControllerState)
        {
          switch (deviceStatusResult.ControllerStatus.ControllerState)
          {
            case ControllerState.IDLE:
              Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: ControllerState changed from {1} to {2}", new object[3]
              {
                (object) CurrentState,
                (object) CurrentDeviceStatus.ControllerState,
                (object) deviceStatusResult.ControllerStatus.ControllerState
              });
              if (deviceStatusResult.ControllerStatus.Transaction.Status != 0 && CurrentTransaction == null && DeviceManagerMode == DeviceManagerMode.NONE)
              {
                Log.Warning(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to IDLE", new object[1]
                {
                  (object) CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
                break;
              }
              break;
            case ControllerState.DROP:
              Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnCountStartedEvent", new object[1]
              {
                (object) CurrentState
              });
              OnCountStartedEvent((object) this, new DeviceTransactionResult()
              {
                data = CurrentTransaction
              });
              break;
            case ControllerState.DROP_PAUSED:
              Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnCountPauseEvent", new object[1]
              {
                (object) CurrentState
              });
              OnCountPauseEvent((object) this, new DeviceTransactionResult()
              {
                data = CurrentTransaction
              });
              break;
            case ControllerState.ESCROW_DROP:
              if (CurrentTransaction == null)
              {
                Log.Warning(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to ESCROW_DROP", new object[1]
                {
                  (object) CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
                break;
              }
              break;
            case ControllerState.ESCROW_REJECT:
              if (CurrentTransaction == null)
              {
                Log.Warning(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction is null during change to ESCROW_REJECT", new object[1]
                {
                  (object) CurrentState
                });
                CurrentState = DeviceManagerState.OUT_OF_ORDER;
                ResetDevice(false);
                break;
              }
              break;
          }
        }
        IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
        EscrowStatus? status1;
        DeviceTransactionStatus? status2;
        int num1;
        if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
        {
          if (HasEscrow)
          {
            IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
            int num2;
            if (currentDeviceStatus2 == null)
            {
              num2 = 0;
            }
            else
            {
              status1 = currentDeviceStatus2.Escrow?.Status;
              EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
              num2 = status1 == escrowStatus & status1.HasValue ? 1 : 0;
            }
            if (num2 == 0)
              goto label_49;
          }
          IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
          if (currentDeviceStatus3 == null)
          {
            num1 = 1;
            goto label_50;
          }
          else
          {
            status2 = currentDeviceStatus3.Transaction?.Status;
            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
            num1 = !(status2 == transactionStatus & status2.HasValue) ? 1 : 0;
            goto label_50;
          }
        }
label_49:
        num1 = 0;
label_50:
        if (num1 != 0)
        {
          if (!CanCount && CurrentTransaction != null)
          {
            if (CurrentState != 0)
            {
              Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: handles lost TransactionStatusResponse>>Raising GetTransactionStatus", new object[1]
              {
                (object) CurrentState
              });
              DeviceTransactionResult e = new DeviceTransactionResult
              {
                  level = ErrorLevel.SUCCESS,
                  data = CurrentTransaction
              };
              OnEscrowOperationCompleteEvent((object) this, e);
            }
          }
          else
            ResetDevice(false);
        }
        if (CurrentDeviceStatus.Transaction.Status != deviceStatusResult.ControllerStatus.Transaction.Status)
        {
          Log.Info(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Transaction status has changed from {1} to {2}", new object[3]
          {
            (object) CurrentState,
            (object) CurrentDeviceStatus.Transaction.Status,
            (object) deviceStatusResult.ControllerStatus.Transaction.Status
          });
          switch (deviceStatusResult.ControllerStatus.Transaction.Status)
          {
            case DeviceTransactionStatus.NONE:
              if (CurrentState == DeviceManagerState.TRANSACTION_ENDING)
              {
                CurrentState = DeviceManagerState.TRANSACTION_ENDED;
                Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Raising OnTransactionEndEvent", new object[1]
                {
                  (object) CurrentState
                });
                OnTransactionEndEvent((object) this, new DeviceTransactionResult()
                {
                  data = CurrentTransaction
                });
                break;
              }
              Log.Warning(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Invalid state, DeviceTransactionStatus changed to NONE while CurrentState is {0}", new object[1]
              {
                (object) CurrentState
              });
              break;
          }
        }
        if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
        {
          int num3;
          if (CurrentTransaction == null)
          {
            IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
            if ((currentDeviceStatus4 != null ? (currentDeviceStatus4.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
            {
              IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
              int num4;
              if (currentDeviceStatus5 == null)
              {
                num4 = 0;
              }
              else
              {
                DeviceState? status3 = currentDeviceStatus5.NoteAcceptor?.Status;
                DeviceState deviceState = DeviceState.IDLE;
                num4 = status3 == deviceState & status3.HasValue ? 1 : 0;
              }
              if (num4 != 0)
              {
                IControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
                int num5;
                if (currentDeviceStatus6 == null)
                {
                  num5 = 0;
                }
                else
                {
                  status1 = currentDeviceStatus6.Escrow?.Status;
                  EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                  num5 = status1 == escrowStatus & status1.HasValue ? 1 : 0;
                }
                if (num5 != 0)
                {
                  IControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
                  if (currentDeviceStatus7 == null)
                  {
                    num3 = 0;
                    goto label_77;
                  }
                  else
                  {
                    status2 = currentDeviceStatus7.Transaction?.Status;
                    DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                    num3 = status2 == transactionStatus & status2.HasValue ? 1 : 0;
                    goto label_77;
                  }
                }
              }
            }
          }
          num3 = 0;
label_77:
          if (num3 != 0)
          {
            Log.Debug(GetType().Name, nameof (OnStatusReportEvent), "EventHandler", "CurrentState = {0}: Reset from out of order is complete", new object[1]
            {
              (object) CurrentState
            });
            CurrentState = DeviceManagerState.NONE;
          }
        }
        if (deviceStatusResult.ControllerStatus.NoteAcceptor.Status == DeviceState.JAM && DeviceManagerMode != DeviceManagerMode.NOTEJAM)
          OnNoteJamStartEvent((object) this, EventArgs.Empty);
        else if (deviceStatusResult.ControllerStatus.NoteAcceptor.Status != DeviceState.JAM && DeviceManagerMode == DeviceManagerMode.NOTEJAM && CurrentState == DeviceManagerState.NOTEJAM_START)
          OnNoteJamEndEvent((object) this, EventArgs.Empty);
        CurrentDeviceStatus = (IControllerStatus) deviceStatusResult.ControllerStatus;
        deviceStatusResult.DeviceManagerState = CurrentState;
        if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
          ResetDevice(false);
        NotifyCurrentTransactionStatusChanged();
        OnStatusReportEvent((object) this, deviceStatusResult);
      }
    }

    public override void Initialise() => CurrentTransaction = (DeviceTransaction) null;

    private void OnTransactionStatusEvent(
      object sender,
      TransactionStatusResponse TransactionStatusResponse)
    {
      if (CurrentTransaction != null)
      {
        if (TransactionStatusResponse != null)
        {
          Log.Debug(GetType().Name, nameof (OnTransactionStatusEvent), "EventHandler", "CurrentState = {0}: Updating CurrentTransactionResult", new object[1]
          {
            (object) CurrentState
          });
          CurrentState = DeviceManagerState.NONE;
          TransactionStatusResponseResult statusResponseResult = ProcessTransactionStatusResponse(TransactionStatusResponse);
          if (statusResponseResult.data.SessionID == CurrentTransaction.SessionID && statusResponseResult.data.TransactionID == CurrentTransaction.TransactionID && statusResponseResult.data.Status != 0)
          {
            CurrentTransaction.CurrentTransactionResult = statusResponseResult.data;
          }
          else
          {
            Log.Error(nameof (CashAccSysDeviceManager), "ERROR", nameof (OnTransactionStatusEvent), "Invalid TransactionStatusResult: " + JsonConvert.SerializeObject((object) statusResponseResult), Array.Empty<object>());
            CurrentState = DeviceManagerState.OUT_OF_ORDER;
            ResetDevice(false);
          }
        }
        NotifyCurrentTransactionStatusChanged();
        DeviceTransactionResult deviceTransactionResult = new DeviceTransactionResult
        {
            level = ErrorLevel.SUCCESS,
            data = CurrentTransaction
        };
        OnTransactionStatusEvent((object) this, deviceTransactionResult);
      }
      else
        Log.Warning(GetType().Name, nameof (OnTransactionStatusEvent), "EventHandler", "CurrentState = {0}: TransactionStatusResponse received outside of a transaction, ignore", new object[1]
        {
          (object) CurrentState
        });
    }

    public override void OnTransactionEndEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: Running Handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentState == DeviceManagerState.TRANSACTION_ENDED)
      {
        Log.Debug(GetType().Name, nameof (OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: Transaction Complete", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.NONE;
        base.OnTransactionEndEvent((object) this, e);
      }
      else
      {
        if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
          return;
        Log.Warning(GetType().Name, nameof (OnTransactionEndEvent), "EventHandler", "CurrentState = {0}: OnTransactionEndEvent outside DeviceManagerState.TRANSACTION_ENDING", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
    }

    private void OnDropResultEvent(object sender, DropResult DropResult)
    {
      Log.Debug(GetType().Name, nameof (OnDropResultEvent), "EventHandler", "CurrentState = {0}: Running Handler", new object[1]
      {
        (object) CurrentState
      });
      DropResultResult dropResultResult = ProcessDropResult(DropResult);
      if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
        return;
      if (CurrentTransaction != null)
      {
        if (CurrentTransaction.DropResults.CurrentDropID == dropResultResult.DropID)
        {
          CurrentTransaction.DropResults.Drops[CurrentTransaction.DropResults.CurrentDropID] = dropResultResult;
          Log.Debug(GetType().Name, nameof (OnDropResultEvent), "EventHandler", "CurrentState = {0}: invoking OnTransactionStatusEvent", new object[1]
          {
            (object) CurrentState
          });
          CurrentTransaction.DropResults.CurrentDropID = (string) null;
          OnTransactionStatusEvent((object) this, (TransactionStatusResponse) null);
        }
        else if (CurrentTransaction.DropResults.Drops[dropResultResult.DropID]?.DropID != null)
        {
          Log.Warning(GetType().Name, nameof (OnDropResultEvent), "EventHandler", "CurrentState = {0}: ERROR in OnDropResultEvent(): Expecting DropResult with DropID {1} but found DropID {2} instead", new object[3]
          {
            (object) CurrentState,
            (object) CurrentTransaction.DropResults.CurrentDropID,
            (object) dropResultResult.DropID
          });
        }
        else
        {
          Console.Error.WriteLine("ERROR in OnDropResultEvent(): DropResult for a drop that is not part of the CurrentTranscation: DropID " + dropResultResult.DropID + " was not found");
          Log.Warning(GetType().Name, nameof (OnDropResultEvent), "EventHandler", "CurrentState = {0}: ERROR in OnDropResultEvent(): DropResult for a drop that is not part of the CurrentTranscation: DropID {2} was not found", new object[2]
          {
            (object) CurrentState,
            (object) dropResultResult.DropID
          });
          CurrentState = DeviceManagerState.OUT_OF_ORDER;
          ResetDevice(false);
        }
      }
      else
      {
        Console.Error.WriteLine("CurrentTransaction cannot be null in OnDropResultEvent()");
        Log.Warning(GetType().Name, nameof (OnDropResultEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnDropResultEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
    }

    public override void OnCashInStartedEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnCashInStartedEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      base.OnCashInStartedEvent((object) this, e);
    }

    public override void OnCountEndEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnCountEndEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentTransaction == null)
      {
        Log.Warning(GetType().Name, nameof (OnCountEndEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnCountEndEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      else
      {
        if (HasEscrow && CurrentState != DeviceManagerState.DROP_ESCROW_DONE)
        {
          Log.Debug(GetType().Name, nameof (OnCountEndEvent), "EventHandler", "CurrentState = {0}: Raising OnEscrowOperationCompleteEvent", new object[1]
          {
            (object) CurrentState
          });
          OnEscrowOperationCompleteEvent((object) this, e);
        }
        base.OnCountEndEvent((object) this, e);
      }
    }

    public override void OnCountStartedEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnCountStartedEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      base.OnCountStartedEvent((object) this, e);
    }

    public override void OnCountPauseEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnCountPauseEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentState == DeviceManagerState.OUT_OF_ORDER)
        return;
      if (CurrentTransaction == null)
      {
        Console.Error.WriteLine("CurrentTransaction cannot be null in OnCountPauseEvent()");
        Log.Warning(GetType().Name, nameof (OnCountPauseEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnCountPauseEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      else if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTING)
        DeviceMessenger.EscrowDrop();
      else if (CurrentState == DeviceManagerState.DROP_ESCROW_REJECTING)
        DeviceMessenger.EscrowReject();
      else
        base.OnCountPauseEvent((object) this, e);
    }

    public override void OnNoteJamStartEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.NOTEJAM_START;
      DeviceManagerMode = DeviceManagerMode.NOTEJAM;
      base.OnNoteJamStartEvent((object) this, e);
    }

    public override void OnNoteJamClearWaitEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.NOTEJAM_CLEAR_WAIT;
      DeviceManagerMode = DeviceManagerMode.NOTEJAM;
      base.OnNoteJamClearWaitEvent((object) this, e);
    }

    public override void OnNoteJamEndRequestEvent(object sender, EventArgs e)
    {
      if (DeviceManagerMode != DeviceManagerMode.NOTEJAM || CurrentState == DeviceManagerState.NOTEJAM_END_REQUEST)
        return;
      CurrentState = DeviceManagerState.NOTEJAM_END_REQUEST;
      DeviceManagerMode = DeviceManagerMode.NOTEJAM;
      base.OnNoteJamEndRequestEvent((object) this, e);
    }

    public override void OnNoteJamEndEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.NONE;
      DeviceManagerMode = DeviceManagerMode.NONE;
      base.OnNoteJamEndEvent((object) this, e);
    }

    public override void OnEscrowDropEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnEscrowDropEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentTransaction == null)
      {
        Log.Warning(GetType().Name, nameof (OnEscrowDropEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnEscrowDropEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      else
        base.OnEscrowDropEvent((object) this, e);
    }

    public override void OnEscrowRejectEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnEscrowRejectEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentTransaction == null)
      {
        Log.Warning(GetType().Name, nameof (OnEscrowRejectEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnEscrowRejectEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      else
        base.OnEscrowRejectEvent((object) this, e);
    }

    public override void OnEscrowOperationCompleteEvent(object sender, DeviceTransactionResult e)
    {
      Log.Debug(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: Running handler", new object[1]
      {
        (object) CurrentState
      });
      if (CurrentTransaction == null)
      {
        Log.Warning(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentTransaction cannot be null in OnEscrowOperationCompleteEvent()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        ResetDevice(false);
      }
      else
      {
        if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTING)
        {
          CurrentState = DeviceManagerState.DROP_ESCROW_ACCEPTED;
          Log.Info(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from DROP_ESCROW_ACCEPTING to DROP_ESCROW_ACCEPTED", new object[1]
          {
            (object) CurrentState
          });
          DeviceTransactionResult e1 = new DeviceTransactionResult
          {
              level = e.level,
              data = CurrentTransaction
          };
          OnEscrowDropEvent((object) this, e1);
        }
        else if (CurrentState == DeviceManagerState.DROP_ESCROW_REJECTING || CurrentState == DeviceManagerState.OUT_OF_ORDER)
        {
          CurrentState = DeviceManagerState.DROP_ESCROW_REJECTED;
          Log.Info(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changed from DROP_ESCROW_REJECTING to DROP_ESCROW_REJECTED", new object[1]
          {
            (object) CurrentState
          });
          DeviceTransactionResult e2 = new DeviceTransactionResult
          {
              level = e.level,
              data = CurrentTransaction
          };
          OnEscrowRejectEvent((object) this, e2);
        }
        if (CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTED || CurrentState == DeviceManagerState.DROP_ESCROW_REJECTED)
        {
          Log.Info(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: CurrentState changing to DROP_ESCROW_DONE", new object[1]
          {
            (object) CurrentState
          });
          CurrentState = DeviceManagerState.DROP_ESCROW_DONE;
          base.OnEscrowOperationCompleteEvent((object) this, e);
        }
        else if (CurrentState != DeviceManagerState.DROP_ESCROW_DONE)
        {
          Log.Warning(GetType().Name, nameof (OnEscrowOperationCompleteEvent), "EventHandler", "CurrentState = {0}: FAILED: CurrentState == DeviceManagerState.DROP_ESCROW_ACCEPTED|| CurrentState == DeviceManagerState.DROP_ESCROW_REJECTED", new object[1]
          {
            (object) CurrentState
          });
          CurrentState = DeviceManagerState.OUT_OF_ORDER;
        }
      }
      GetTransactionStatus();
    }

    public override void OnEscrowJamStartEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.ESCROWJAM_START;
      DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
      base.OnEscrowJamStartEvent((object) this, e);
    }

    public override void OnEscrowJamClearWaitEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.ESCROWJAM_CLEAR_WAIT;
      DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
      base.OnEscrowJamClearWaitEvent((object) this, e);
    }

    public override void OnEscrowJamEndRequestEvent(object sender, EventArgs e)
    {
      if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM || CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST)
        return;
      CurrentState = DeviceManagerState.ESCROWJAM_END_REQUEST;
      DeviceManagerMode = DeviceManagerMode.ESCROW_JAM;
      base.OnEscrowJamEndRequestEvent((object) this, e);
    }

    public override void OnEscrowJamEndEvent(object sender, EventArgs e)
    {
      CurrentState = DeviceManagerState.NONE;
      DeviceManagerMode = DeviceManagerMode.NONE;
      base.OnEscrowJamEndEvent((object) this, e);
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
            IControllerStatus currentDeviceStatus = CurrentDeviceStatus;
            int num;
            if (currentDeviceStatus == null)
            {
              num = 1;
            }
            else
            {
              DeviceTransactionStatus? status = currentDeviceStatus.Transaction?.Status;
              DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
              num = !(status == transactionStatus & status.HasValue) ? 1 : 0;
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
      else
      {
        switch (CurrentDeviceStatus.ControllerState)
        {
        }
      }
    }

    public override void SetCurrency(string currency) => DeviceMessenger.SetCurrency(currency);

    public bool CanTransactionStart
    {
      get
      {
        if (HasEscrow)
        {
          int num1;
          if (CurrentTransaction == null && CurrentState == DeviceManagerState.NONE)
          {
            IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
            if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
            {
              IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
              int num2;
              if (currentDeviceStatus2 == null)
              {
                num2 = 0;
              }
              else
              {
                DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
                DeviceState deviceState = DeviceState.IDLE;
                num2 = status == deviceState & status.HasValue ? 1 : 0;
              }
              if (num2 != 0)
              {
                IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                int num3;
                if (currentDeviceStatus3 == null)
                {
                  num3 = 0;
                }
                else
                {
                  EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                  EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                  num3 = status == escrowStatus & status.HasValue ? 1 : 0;
                }
                if (num3 != 0)
                {
                  IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
                  if (currentDeviceStatus4 == null)
                  {
                    num1 = 0;
                    goto label_15;
                  }
                  else
                  {
                    DeviceTransactionStatus? status = currentDeviceStatus4.Transaction?.Status;
                    DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                    num1 = status == transactionStatus & status.HasValue ? 1 : 0;
                    goto label_15;
                  }
                }
              }
            }
          }
          num1 = 0;
label_15:
          return num1 != 0;
        }
        IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
        int num;
        if ((currentDeviceStatus5 != null ? (currentDeviceStatus5.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
        {
          IControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
          if (currentDeviceStatus6 == null)
          {
            num = 0;
          }
          else
          {
            DeviceTransactionStatus? status = currentDeviceStatus6.Transaction?.Status;
            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
            num = status == transactionStatus & status.HasValue ? 1 : 0;
          }
        }
        else
          num = 0;
        return num != 0;
      }
    }

    public override void TransactionStart(
      string currency,
      string accountNumber,
      string sessionID,
      string transactionID,
      long transactionLimitCents = 9223372036854775807,
      long transactionValueCents = 0)
    {
      Log.Debug(GetType().Name, nameof (TransactionStart), "Command", "CurrentState = {0}: StartingTransaction", new object[1]
      {
        (object) CurrentState
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
              OnTransactionStartedEvent((object) this, CurrentTransaction);
            }
            else
            {
              if (CurrentTransaction?.SessionID == sessionID)
              {
                if (CurrentTransaction?.TransactionID == transactionID)
                {
                  Console.Error.WriteLine("This session and transaction is currently transacting");
                  Log.Warning(GetType().Name, nameof (TransactionStart), "InvalidOperation", "DeviceManager is currently processing Session={0} and Transaction={1}.", new object[2]
                  {
                    (object) CurrentTransaction?.SessionID,
                    (object) CurrentTransaction?.TransactionID
                  });
                }
                else
                  Log.Warning(GetType().Name, nameof (TransactionStart), "InvalidOperation", "DeviceManager is currently processing Session={0} and Transaction={1}. Please end the current transaction before starting Session={2}; Transaction={3};", new object[4]
                  {
                    (object) CurrentTransaction?.SessionID,
                    (object) CurrentTransaction?.TransactionID,
                    (object) sessionID,
                    (object) transactionID
                  });
              }
              else
                Log.Warning(GetType().Name, nameof (TransactionStart), "InvalidOperation", "CurrentTransaction.TransactionID {0} != transactionID {1}", new object[2]
                {
                  (object) CurrentTransaction?.SessionID,
                  (object) sessionID
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
          Log.Warning(GetType().Name, nameof (TransactionStart), "InvalidOperation", "CurrentState = {0}: TransactionStart outside DeviceManagerState.NONE", new object[1]
          {
            (object) CurrentState
          });
          CurrentState = DeviceManagerState.OUT_OF_ORDER;
          ResetDevice(false);
        }
      }
      else
        Log.Warning(GetType().Name, nameof (TransactionStart), "InvalidOperation", "FAILED: CanTransactionStart", Array.Empty<object>());
    }

    public override bool CanTransactionEnd
    {
      get
      {
        if (HasEscrow)
        {
          int num1;
          if (CurrentState == DeviceManagerState.NONE && CurrentTransaction?.CurrentTransactionResult != null)
          {
            IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
            DeviceState? status1;
            if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) == 0)
            {
              IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
              int num2;
              if (currentDeviceStatus2 == null)
              {
                num2 = 0;
              }
              else
              {
                status1 = currentDeviceStatus2.NoteAcceptor?.Status;
                DeviceState deviceState = DeviceState.OK;
                num2 = status1 == deviceState & status1.HasValue ? 1 : 0;
              }
              if (num2 == 0)
                goto label_23;
            }
            IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
            int num3;
            if (currentDeviceStatus3 == null)
            {
              num3 = 0;
            }
            else
            {
              status1 = currentDeviceStatus3.NoteAcceptor?.Status;
              DeviceState deviceState = DeviceState.IDLE;
              num3 = status1 == deviceState & status1.HasValue ? 1 : 0;
            }
            if (num3 == 0)
            {
              IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
              int num4;
              if (currentDeviceStatus4 == null)
              {
                num4 = 0;
              }
              else
              {
                status1 = currentDeviceStatus4.NoteAcceptor?.Status;
                DeviceState deviceState = DeviceState.OK;
                num4 = status1 == deviceState & status1.HasValue ? 1 : 0;
              }
              if (num4 == 0)
                goto label_23;
            }
            if (HasEscrow)
            {
              IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
              int num5;
              if (currentDeviceStatus5 == null)
              {
                num5 = 0;
              }
              else
              {
                EscrowStatus? status2 = currentDeviceStatus5.Escrow?.Status;
                EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                num5 = status2 == escrowStatus & status2.HasValue ? 1 : 0;
              }
              if (num5 == 0)
                goto label_23;
            }
            IControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
            if (currentDeviceStatus6 == null)
            {
              num1 = 0;
              goto label_24;
            }
            else
            {
              DeviceTransactionStatus? status3 = currentDeviceStatus6.Transaction?.Status;
              DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
              num1 = status3 == transactionStatus & status3.HasValue ? 1 : 0;
              goto label_24;
            }
          }
label_23:
          num1 = 0;
label_24:
          return num1 != 0;
        }
        int num6;
        if (CurrentTransaction?.CurrentTransactionResult != null && CurrentState != DeviceManagerState.TRANSACTION_ENDING)
        {
          IControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
          DeviceState? status;
          int num7;
          if (currentDeviceStatus7 == null)
          {
            num7 = 0;
          }
          else
          {
            status = currentDeviceStatus7.NoteAcceptor?.Status;
            DeviceState deviceState = DeviceState.IDLE;
            num7 = status == deviceState & status.HasValue ? 1 : 0;
          }
          if (num7 == 0)
          {
            IControllerStatus currentDeviceStatus8 = CurrentDeviceStatus;
            int num8;
            if (currentDeviceStatus8 == null)
            {
              num8 = 0;
            }
            else
            {
              status = currentDeviceStatus8.NoteAcceptor?.Status;
              DeviceState deviceState = DeviceState.OK;
              num8 = status == deviceState & status.HasValue ? 1 : 0;
            }
            if (num8 == 0)
              goto label_35;
          }
          IControllerStatus currentDeviceStatus9 = CurrentDeviceStatus;
          num6 = currentDeviceStatus9 != null ? (currentDeviceStatus9.ControllerState == ControllerState.DROP ? 1 : 0) : 0;
          goto label_36;
        }
label_35:
        num6 = 0;
label_36:
        return num6 != 0;
      }
    }

    public override void TransactionEnd()
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
      DeviceTransactionResult e = new DeviceTransactionResult
      {
          level = ErrorLevel.SUCCESS,
          data = CurrentTransaction
      };
      OnCashInStartedEvent((object) this, e);
    }

    public override bool CanCount
    {
      get
      {
        int num1;
        if (CurrentTransaction != null && !ClearHopperRequest && CurrentState != DeviceManagerState.OUT_OF_ORDER)
        {
          DeviceTransaction currentTransaction = CurrentTransaction;
          if ((currentTransaction != null ? (currentTransaction.TransactionLimitCents <= 0L ? 1 : 0) : 0) == 0)
          {
            long? droppedTotalCents = CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents;
            long? transactionLimitCents = CurrentTransaction?.TransactionLimitCents;
            if (!(droppedTotalCents <= transactionLimitCents & droppedTotalCents.HasValue & transactionLimitCents.HasValue))
              goto label_21;
          }
          IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
          if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.IDLE ? 1 : 0) : 0) != 0)
          {
            IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
            int num2;
            if (currentDeviceStatus2 == null)
            {
              num2 = 0;
            }
            else
            {
              DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
              DeviceState deviceState = DeviceState.IDLE;
              num2 = status == deviceState & status.HasValue ? 1 : 0;
            }
            if (num2 != 0)
            {
              if (HasEscrow)
              {
                IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                int num3;
                if (currentDeviceStatus3 == null)
                {
                  num3 = 0;
                }
                else
                {
                  EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                  EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                  num3 = status == escrowStatus & status.HasValue ? 1 : 0;
                }
                if (num3 == 0)
                  goto label_21;
              }
              IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
              DeviceTransactionStatus? status1;
              int num4;
              if (currentDeviceStatus4 == null)
              {
                num4 = 0;
              }
              else
              {
                status1 = currentDeviceStatus4.Transaction?.Status;
                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                num4 = status1 == transactionStatus & status1.HasValue ? 1 : 0;
              }
              if (num4 == 0)
              {
                IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                if (currentDeviceStatus5 == null)
                {
                  num1 = 0;
                  goto label_22;
                }
                else
                {
                  status1 = currentDeviceStatus5.Transaction?.Status;
                  DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
                  num1 = status1 == transactionStatus & status1.HasValue ? 1 : 0;
                  goto label_22;
                }
              }
              else
              {
                num1 = 1;
                goto label_22;
              }
            }
          }
        }
label_21:
        num1 = 0;
label_22:
        return num1 != 0;
      }
    }

    public override void Count()
    {
      if (!CanCount)
        return;
      if (CurrentTransaction != null)
      {
        if (CurrentTransaction.TransactionValueCents == 0L || CurrentTransaction.TransactionValueCentsLeft > 0L)
        {
          if (CurrentDeviceStatus == null)
          {
            Console.Error.WriteLine("CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()");
            Log.Warning(GetType().Name, nameof (Count), "Command", "CurrentState = {0}: CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()", new object[1]
            {
              (object) CurrentState
            });
            CurrentState = DeviceManagerState.OUT_OF_ORDER;
            throw new NullReferenceException("CurrentDeviceStatus cannot be null in CashAccSysDeviceManager.Count()");
          }
          if (CurrentState == DeviceManagerState.DROP_STARTING || CurrentState == DeviceManagerState.TRANSACTION_STARTED || CurrentState == DeviceManagerState.NONE)
          {
            if (CurrentState != DeviceManagerState.DROP_STARTING)
            {
              CurrentState = DeviceManagerState.DROP_STARTING;
              CurrentTransaction.DropResults.CurrentDropID = Guid.NewGuid().ToString().ToUpperInvariant();
            }
            DeviceMessenger.Count(CurrentTransaction.DropResults.CurrentDropID, CurrentTransaction.TransactionValueCentsLeft);
          }
          else if (CurrentState != DeviceManagerState.DROP_STARTED)
          {
            Console.Error.WriteLine(string.Format("Controller not ready for count: CurrentDeviceStatus?.ControllerState={0}: CurrentDeviceStatus?.NoteAcceptor?.Status={1}: CurrentDeviceStatus?.Escrow?.Status={2}: CurrentDeviceStatus?.Transaction?.Status={3}", (object) CurrentDeviceStatus?.ControllerState, (object) CurrentDeviceStatus?.NoteAcceptor?.Status, (object) CurrentDeviceStatus?.Escrow?.Status, (object) CurrentDeviceStatus?.Transaction?.Status));
            Log.Warning(GetType().Name, nameof (Count), "Command", "CurrentState = {0}: Controller not ready for count: CurrentDeviceStatus?.ControllerState={1}: CurrentDeviceStatus?.NoteAcceptor?.Status={2}: CurrentDeviceStatus?.Escrow?.Status={3}: CurrentDeviceStatus?.Transaction?.Status={4}", new object[5]
            {
              (object) CurrentState,
              (object) CurrentDeviceStatus?.ControllerState,
              (object) CurrentDeviceStatus?.NoteAcceptor?.Status,
              (object) CurrentDeviceStatus?.Escrow?.Status,
              (object) CurrentDeviceStatus?.Transaction?.Status
            });
            CurrentState = DeviceManagerState.OUT_OF_ORDER;
          }
        }
        else
        {
          Console.Error.WriteLine(string.Format("transaction limit reached: TotalValue={0}: Limit={1}", (object) CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents, (object) CurrentTransaction?.TransactionLimitCents));
          Log.Warning(GetType().Name, nameof (Count), "Command", "CurrentState = {0}: transaction limit reached: TotalValue={1}: Limit={2}", new object[3]
          {
            (object) CurrentState,
            (object) CurrentTransaction?.CurrentTransactionResult?.EscrowPlusDroppedTotalCents,
            (object) CurrentTransaction?.TransactionLimitCents
          });
        }
      }
      else
      {
        Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()");
        Log.Warning(GetType().Name, nameof (Count), "Command", "CurrentState = {0}: CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()", new object[1]
        {
          (object) CurrentState
        });
        CurrentState = DeviceManagerState.OUT_OF_ORDER;
        throw new NullReferenceException("CurrentTransaction cannot be null in CashAccSysDeviceManager.Count()");
      }
    }

    public override bool CanPauseCount => false;

    public bool CanStopCount
    {
      get
      {
        int num1;
        if (CurrentState != DeviceManagerState.OUT_OF_ORDER && CurrentTransaction != null)
        {
          IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
          if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP ? 1 : 0) : 0) != 0)
          {
            IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
            int num2;
            if (currentDeviceStatus2 == null)
            {
              num2 = 0;
            }
            else
            {
              DeviceState? status = currentDeviceStatus2.NoteAcceptor?.Status;
              DeviceState deviceState = DeviceState.OK;
              num2 = status == deviceState & status.HasValue ? 1 : 0;
            }
            if (num2 != 0)
            {
              if (HasEscrow)
              {
                IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
                int num3;
                if (currentDeviceStatus3 == null)
                {
                  num3 = 0;
                }
                else
                {
                  EscrowStatus? status = currentDeviceStatus3.Escrow?.Status;
                  EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                  num3 = status == escrowStatus & status.HasValue ? 1 : 0;
                }
                if (num3 == 0)
                  goto label_22;
              }
              IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
              DeviceTransactionStatus? status1;
              int num4;
              if (currentDeviceStatus4 == null)
              {
                num4 = 0;
              }
              else
              {
                status1 = currentDeviceStatus4.Transaction?.Status;
                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.COUNTING;
                num4 = status1 == transactionStatus & status1.HasValue ? 1 : 0;
              }
              if (num4 == 0)
              {
                IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
                int num5;
                if (currentDeviceStatus5 == null)
                {
                  num5 = 0;
                }
                else
                {
                  status1 = currentDeviceStatus5.Transaction?.Status;
                  DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.ACTIVE;
                  num5 = status1 == transactionStatus & status1.HasValue ? 1 : 0;
                }
                if (num5 == 0)
                  goto label_22;
              }
              DeviceTransaction currentTransaction = CurrentTransaction;
              if (currentTransaction == null)
              {
                num1 = 0;
                goto label_23;
              }
              else
              {
                DropStatusResultStatus? statusResultStatus1 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
                DropStatusResultStatus statusResultStatus2 = DropStatusResultStatus.DROPPING;
                num1 = statusResultStatus1 == statusResultStatus2 & statusResultStatus1.HasValue ? 1 : 0;
                goto label_23;
              }
            }
          }
        }
label_22:
        num1 = 0;
label_23:
        return num1 != 0;
      }
    }

    public override void PauseCount()
    {
      if (CurrentTransaction != null)
      {
        IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
        DeviceState? nullable1;
        EscrowStatus? nullable2;
        DeviceTransactionStatus? nullable3;
        int num1;
        if ((currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP ? 1 : 0) : 0) != 0)
        {
          IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
          int num2;
          if (currentDeviceStatus2 == null)
          {
            num2 = 0;
          }
          else
          {
            nullable1 = currentDeviceStatus2.NoteAcceptor?.Status;
            DeviceState deviceState = DeviceState.OK;
            num2 = nullable1 == deviceState & nullable1.HasValue ? 1 : 0;
          }
          if (num2 != 0)
          {
            if (HasEscrow)
            {
              IControllerStatus currentDeviceStatus3 = CurrentDeviceStatus;
              int num3;
              if (currentDeviceStatus3 == null)
              {
                num3 = 0;
              }
              else
              {
                nullable2 = currentDeviceStatus3.Escrow?.Status;
                EscrowStatus escrowStatus = EscrowStatus.IDLE_POS;
                num3 = nullable2 == escrowStatus & nullable2.HasValue ? 1 : 0;
              }
              if (num3 == 0)
                goto label_19;
            }
            IControllerStatus currentDeviceStatus4 = CurrentDeviceStatus;
            int num4;
            if (currentDeviceStatus4 == null)
            {
              num4 = 0;
            }
            else
            {
              nullable3 = currentDeviceStatus4.Transaction?.Status;
              DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.COUNTING;
              num4 = nullable3 == transactionStatus & nullable3.HasValue ? 1 : 0;
            }
            if (num4 == 0)
            {
              IControllerStatus currentDeviceStatus5 = CurrentDeviceStatus;
              if (currentDeviceStatus5 == null)
              {
                num1 = 0;
                goto label_20;
              }
              else
              {
                nullable3 = currentDeviceStatus5.Transaction?.Status;
                DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.NONE;
                num1 = nullable3 == transactionStatus & nullable3.HasValue ? 1 : 0;
                goto label_20;
              }
            }
            else
            {
              num1 = 1;
              goto label_20;
            }
          }
        }
label_19:
        num1 = 0;
label_20:
        if (num1 != 0)
        {
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
            num5 = statusResultStatus1 == statusResultStatus2 & statusResultStatus1.HasValue ? 1 : 0;
          }
          if (num5 != 0)
            DeviceMessenger.RequestPause();
          else
            Console.Error.WriteLine("DropStatus is " + CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus.ToString() + ", expecting DropStatusResultStatus.DROPPING");
        }
        else
        {
          TextWriter error = Console.Error;
          object[] objArray = new object[4]
          {
            (object) CurrentDeviceStatus?.ControllerState,
            null,
            null,
            null
          };
          IControllerStatus currentDeviceStatus6 = CurrentDeviceStatus;
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
          objArray[1] = (object) nullable4;
          IControllerStatus currentDeviceStatus7 = CurrentDeviceStatus;
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
          objArray[2] = (object) nullable5;
          IControllerStatus currentDeviceStatus8 = CurrentDeviceStatus;
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
          objArray[3] = (object) nullable6;
          string str = string.Format("Controller not ready for pause: CurrentDeviceStatus?.ControllerState={0}: CurrentDeviceStatus?.NoteAcceptor?.Status={1}: CurrentDeviceStatus?.Escrow?.Status={2}: CurrentDeviceStatus?.Transaction?.Status={3}", objArray);
          error.WriteLine(str);
        }
      }
      else
      {
        Console.Error.WriteLine("CurrentTransaction cannot be null in CashAccSysDeviceManager.PauseCount()");
        DeviceMessenger.RequestPause();
      }
    }

    public override void StartCIT(string sealNumber) => DeviceMessenger.StartCIT(sealNumber);

    public override void EndCIT(string bagnumber) => DeviceMessenger.EndCIT(bagnumber);

    public override bool CanEndCount
    {
      get
      {
        if (HasEscrow)
          return false;
        int num1 = CurrentState != DeviceManagerState.OUT_OF_ORDER ? 1 : 0;
        IControllerStatus currentDeviceStatus1 = CurrentDeviceStatus;
        int num2 = currentDeviceStatus1 != null ? (currentDeviceStatus1.ControllerState == ControllerState.DROP_PAUSED ? 1 : 0) : 0;
        int num3;
        if ((num1 & num2) != 0)
        {
          IControllerStatus currentDeviceStatus2 = CurrentDeviceStatus;
          int num4;
          if (currentDeviceStatus2 == null)
          {
            num4 = 0;
          }
          else
          {
            DeviceTransactionStatus? status = currentDeviceStatus2.Transaction?.Status;
            DeviceTransactionStatus transactionStatus = DeviceTransactionStatus.PAUSED;
            num4 = status == transactionStatus & status.HasValue ? 1 : 0;
          }
          if (num4 != 0)
          {
            DeviceTransaction currentTransaction = CurrentTransaction;
            if (currentTransaction == null)
            {
              num3 = 0;
              goto label_11;
            }
            else
            {
              DropStatusResultStatus? statusResultStatus1 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
              DropStatusResultStatus statusResultStatus2 = DropStatusResultStatus.DONE;
              num3 = statusResultStatus1 == statusResultStatus2 & statusResultStatus1.HasValue ? 1 : 0;
              goto label_11;
            }
          }
        }
        num3 = 0;
label_11:
        return num3 != 0;
      }
    }

    public override bool CanEscrowDrop => HasEscrow && CurrentState == DeviceManagerState.DROP_STOPPED && !ClearHopperRequest && CurrentTransaction?.CurrentTransactionResult != null && CurrentTransaction.CurrentTransactionResult.EscrowTotalCents > 0L && CanStopCount;

    public override void EscrowDrop()
    {
      if (CurrentState != DeviceManagerState.DROP_STOPPED)
        return;
      CurrentState = DeviceManagerState.DROP_ESCROW_ACCEPTING;
      DeviceMessenger.EscrowDrop();
    }

    public override bool CanEscrowReject => HasEscrow && CurrentState == DeviceManagerState.DROP_STOPPED && !ClearHopperRequest && CurrentTransaction?.CurrentTransactionResult != null && CanStopCount;

    public override bool CanClearNoteJam => false;

    public object StatusChangeLock { get; private set; } = new object();

    public override void EscrowReject()
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

    public override void ShowDeviceController()
    {
      Log.Info(GetType().Name, "ShowDeviceController()", "Command", "Showing the device controller", Array.Empty<object>());
      DeviceMessenger.ShowDeviceController();
    }

    public override void ClearEscrowJam() => CashAccSysSerialFix.ClearEscrowJam();

    public override void EndEscrowJam()
    {
      if (CurrentState != DeviceManagerState.ESCROWJAM_END_REQUEST)
        return;
      OnEscrowJamEndEvent((object) this, EventArgs.Empty);
    }

    private DeviceStatusChangedEventArgs ProcessStatusReport(
      StatusReport statusReport)
    {
      string status1 = statusReport.Status.Bag.Status;
      BagState bagState = status1 == "OK" ? BagState.OK : (status1 == "FULL" ? BagState.FULL : (status1 == "CAPACITY" ? BagState.CAPACITY : (status1 == "CLOSED" ? BagState.CLOSED : (status1 == "BAG_REMOVED" ? BagState.BAG_REMOVED : BagState.ERROR))));
      string position = statusReport.Status.Escrow.Position;
      EscrowPosition escrowPosition = position == "IDLE" ? EscrowPosition.IDLE : (position == "DROP" ? EscrowPosition.DROP : (position == "REJECT" ? EscrowPosition.REJECT : EscrowPosition.NONE));
      string status2 = statusReport.Status.Escrow.Status;
      EscrowStatus escrowStatus = status2 == "IDLE_POS" ? EscrowStatus.IDLE_POS : (status2 == "DROP_POS" ? EscrowStatus.DROP_POS : (status2 == "REJECT_POS" ? EscrowStatus.REJECT_POS : (status2 == "MOVING" ? EscrowStatus.ESCROW_JAM : EscrowStatus.NONE)));
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
      int result = int.TryParse(statusReport.Status.Sensors.Value, NumberStyles.HexNumber, (IFormatProvider) new CultureInfo("en-US"), out result) ? result : 0;
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
          Value = result,
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
        int num1;
        if (currentTransaction == null)
        {
          num1 = 0;
        }
        else
        {
          DropStatusResultStatus? statusResultStatus2 = currentTransaction.CurrentTransactionResult?.CurrentDropStatus?.data?.DropStatusResultStatus;
          DropStatusResultStatus statusResultStatus3 = statusResultStatus1;
          num1 = statusResultStatus2 == statusResultStatus3 & statusResultStatus2.HasValue ? 1 : 0;
        }
        int num2;
        if (num1 != 0)
        {
          long? totalCount1 = CurrentTransaction?.CurrentTransactionResult?.CurrentDropStatus?.data?.DenominationResult?.data?.TotalCount;
          int? totalCount2 = dropStatus?.Body?.TotalCount;
          long? nullable = totalCount2.HasValue ? new long?((long) totalCount2) : new long?();
          num2 = totalCount1 == nullable & totalCount1.HasValue == nullable.HasValue ? 1 : 0;
        }
        else
          num2 = 0;
        if (num2 != 0)
          return CurrentTransaction.CurrentTransactionResult.CurrentDropStatus;
      }
      catch (Exception ex)
      {
      }
      List<DenominationItem> denominationItemList = new List<DenominationItem>();
      foreach (NoteCount noteCount in dropStatus.Body.NoteCounts.NoteCount)
        denominationItemList.Add(new DenominationItem()
        {
          count = (long) noteCount.Count,
          Currency = noteCount.Currency,
          type = DenominationItemType.NOTE,
          denominationValue = noteCount.Denomination
        });
      DenominationResult denominationResult1 = new DenominationResult
      {
          level = ErrorLevel.SUCCESS,
          resultCode = 0,
          data = new Denomination()
          {
              DenominationItems = denominationItemList
          }
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
      long valueOrDefault1 = (long)nullable1;
      int num;
      if (totalValue == valueOrDefault1 & nullable1.HasValue)
      {
        long totalCount3 = dropStatusResult1.data.DenominationResult.data.TotalCount;
        int? totalCount4 = dropStatus?.Body?.TotalCount;
        nullable1 = totalCount4.HasValue ? new long?((long) totalCount4) : new long?();
        long valueOrDefault2 = (long)nullable1;
        if (totalCount3 == valueOrDefault2 & nullable1.HasValue)
        {
          num = 0;
          goto label_29;
        }
      }
      num = 2;
label_29:
      dropStatusResult2.level = (ErrorLevel) num;
      return dropStatusResult1;
    }

    private DropResultResult ProcessDropResult(DropResult dropResult)
    {
      List<DenominationItem> denominationItemList = new List<DenominationItem>();
      foreach (NoteCount noteCount in dropResult.Body.NoteCounts.NoteCount)
        denominationItemList.Add(new DenominationItem()
        {
          count = (long) noteCount.Count,
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
      DropResultResult dropResultResult = new DropResultResult
      {
          DropDeviceID = dropResult?.Body?.DeviceSerialNumber,
          SessionID = dropResult?.Body?.InputNumber,
          TransactionID = dropResult?.Body?.Reference,
          DropID = dropResult?.Body?.InputSubNumber,
          level = dropResult.Body.NoteJam > 0 ? ErrorLevel.ERROR : ErrorLevel.SUCCESS,
          TotalNumberOfNotes = (int)dropResult?.Body?.TotalNumberOfNotes,
          DroppedAmountCents = (long)dropResult?.Body?.TranAmount,
          TransactionNumber = dropResult?.Body?.TranCycle.ToString() + string.Empty,
          DropMode = dropMode,
          isMultiDrop = dropMode == DropMode.DROP_NOTES || dropMode == DropMode.DROP_COINS
      };
      DenominationResult denominationResult = new DenominationResult
      {
          level = dropResult.Body.NoteJam > 0 ? ErrorLevel.ERROR : ErrorLevel.SUCCESS,
          resultCode = 0,
          NoteJamDetected = dropResult.Body.NoteJam > 0,
          NotesRejected = dropResult.Body.Rejected > 0,
          data = new Denomination()
          {
              DenominationItems = denominationItemList
          }
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
      TransactionResultType transactionResultType = transactionStatusResponse?.Body?.TransactionType == "MULTIDROP" ? TransactionResultType.MULTIDROP : TransactionResultType.ERROR;
      TransactionStatusResponseResult statusResponseResult = new TransactionStatusResponseResult();
      TransactionStatusResponseData statusResponseData = new TransactionStatusResponseData(transactionStatusResponse?.Body?.InputNumber, transactionStatusResponse?.Body?.Reference)
          {
              DispensedAmountCents = (long)transactionStatusResponse?.Body?.DispensedAmount
          };
      Denomination denomination1;
      if (transactionStatusResponse == null)
      {
        denomination1 = (Denomination) null;
      }
      else
      {
        Body body = transactionStatusResponse.Body;
        denomination1 = body != null ? body.DispensedNotes.CreateDenomination() : (Denomination) null;
      }
      statusResponseData.DispensedNotes = denomination1;
      statusResponseData.NumberOfDrops = (int)transactionStatusResponse?.Body?.NumberOfDrops;
      statusResponseData.RequestedDispenseAmount = (long)transactionStatusResponse?.Body?.RequestedDispenseAmount;
      Denomination denomination2;
      if (transactionStatusResponse == null)
      {
        denomination2 = (Denomination) null;
      }
      else
      {
        Body body = transactionStatusResponse.Body;
        denomination2 = body != null ? body.RequestedDispenseNotes.CreateDenomination() : (Denomination) null;
      }
      statusResponseData.RequestedDispenseNotes = denomination2;
      statusResponseData.RequestedDropAmount = (long)transactionStatusResponse?.Body?.RequestedDropAmount;
      statusResponseData.TotalDroppedAmountCents = (long)transactionStatusResponse?.Body?.TotalDroppedAmount;
      Denomination denomination3;
      if (transactionStatusResponse == null)
      {
        denomination3 = (Denomination) null;
      }
      else
      {
        Body body = transactionStatusResponse.Body;
        denomination3 = body != null ? body.TotalDroppedNotes.CreateDenomination() : (Denomination) null;
      }
      statusResponseData.TotalDroppedNotes = denomination3;
      statusResponseData.Result = transactionResultResult;
      statusResponseData.Status = transactionResultStatus;
      statusResponseData.Type = transactionResultType;
      statusResponseResult.data = statusResponseData;
      return statusResponseResult;
    }

    //private List<(DateTime FileDate, FileInfo File)> GetCashAccSysLogFiles(
    //  DateTime StartDate,
    //  DateTime EndDate)
    //{
    //  List<string> list = (Directory.GetFiles(CONTROLLER_LOG_DIRECTORY, "TRACE*.log", SearchOption.TopDirectoryOnly)).Reverse<string>().ToList<string>();
    //  var source = new List();
    //  foreach (string fileName in list)
    //  {
    //    FileInfo fileInfo = new FileInfo(fileName);
    //    DateTime exact = DateTime.ParseExact(fileInfo.Name.Substring(5, 14), "yyyyMMddHHmmss", (IFormatProvider) new CultureInfo("en-US"), DateTimeStyles.None);
    //    if (StartDate <= exact)
    //    {
    //      source.Add((exact, fileInfo));
    //    }
    //    else
    //    {
    //      source.Add((exact, fileInfo));
    //      break;
    //    }
    //  }
    //  DateTime startLogDate = source.Where(x => x.FileDate <= StartDate).OrderByDescending((y => y.FileDate)).FirstOrDefault().Item1;
    //  DateTime endLogDate = source.Where(x => x.FileDate <= EndDate).OrderByDescending( (y => y.FileDate)).FirstOrDefault().Item1;
    //  return source.Where(x => x.FileDate >= startLogDate && x.FileDate <= endLogDate).ToList();
    //}

    //private bool IsEscrowJam(List<(DateTime FileDate, FileInfo File)> logFiles, DateTime startDate)
    //{
    //  foreach (FileInfo fileInfo in logFiles.Select<(DateTime, FileInfo), FileInfo>((Func<(DateTime, FileInfo), FileInfo>) (x => x.File)).ToList<FileInfo>())
    //  {
    //    string str = ((IEnumerable<string>) File.ReadAllLines(fileInfo.FullName)).Reverse<string>().ToList<string>().FirstOrDefault<string>((Func<string, bool>) (x => x.Contains("JamOpenEscrowWait")));
    //    if (str != null && DateTime.ParseExact(fileInfo.Name.Substring(5, 8) + str.Substring(1, 12), "yyyyMMddHH:mm:ss.fff", (IFormatProvider) new CultureInfo("en-US"), DateTimeStyles.None) >= startDate)
    //      return true;
    //  }
    //  return false;
    //}

    private void CashAccSysSerialFix_DE50StatusChangedEvent(
      object sender,
      DE50StatusChangedResult e)
    {
      if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM && e.Status[4] == 'R')
        OnEscrowJamStartEvent((object) this, EventArgs.Empty);
      else if (DeviceManagerMode == DeviceManagerMode.ESCROW_JAM && CurrentState != DeviceManagerState.ESCROWJAM_CLEAR_WAIT && e.Status[4] == 'I')
      {
        OnEscrowJamClearWaitEvent((object) this, EventArgs.Empty);
      }
      else
      {
        if (DeviceManagerMode != DeviceManagerMode.ESCROW_JAM || CurrentState == DeviceManagerState.ESCROWJAM_END_REQUEST || e.Status[4] != '@')
          return;
        OnEscrowJamEndRequestEvent((object) this, EventArgs.Empty);
      }
    }

    public override void ClearNotesinEscrowWithDrop() => throw new NotImplementedException();

    public override void ClearNoteJam() => throw new NotImplementedException();
  }
}

