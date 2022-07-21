using Cashmere.Library.Standard.Logging;
using Cashmere.Library.Standard.Statuses;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DeviceManager
{
    public abstract class DeviceManagerBase : IDeviceManager, INotifyPropertyChanged
    {
        private DeviceTransaction currentTransaction;
        public ControllerState _controllerState = ControllerState.NONE;
        public DeviceState _deviceState;
        private bool escrowBillPresent;
        private bool _hasEscrow = true;
        private bool _isEnabled = true;
        private DeviceManagerState _currentState = DeviceManagerState.INIT;

        protected CashmereLogger Log { get; set; } = new CashmereLogger(Assembly.GetExecutingAssembly().GetName().Version.ToString(), nameof(DeviceManagerBase), null);

        public DeviceMessengerBase DeviceMessenger { get; set; }

        public DeviceTransaction CurrentTransaction
        {
            get => currentTransaction;
            set
            {
                currentTransaction = value;
                if (value != null)
                    ;
            }
        }

        public abstract Version DeviceManagerVersion { get; }

        public ControllerState ControllerState
        {
            get => _controllerState;
            set
            {
                if (_controllerState == value)
                    return;
                _controllerState = value;
                OnRaiseControllerStateChangedEvent(new ControllerStateChangedEventArgs(value));
                NotifyPropertyChanged(nameof(ControllerState));
            }
        }

        public DeviceState DeviceState
        {
            get => _deviceState;
            set
            {
                if (_deviceState == value)
                    return;
                _deviceState = value;
                OnRaiseDeviceStateChangedEvent(new DeviceStateChangedEventArgs(value));
                NotifyPropertyChanged(nameof(DeviceState));
            }
        }

        public virtual string Currency { get; set; }

        private int ConnectionID { get; set; }

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

        public DeviceManagerMode DeviceManagerMode { get; set; } = DeviceManagerMode.NONE;

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
                NotifyPropertyChanged(nameof(Enabled));
            }
        }

        public DeviceManagerState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;
                if (value == DeviceManagerState.OUT_OF_ORDER)
                    Log.Debug(GetType().Name, nameof(CurrentState), "CurrentStateProperty", "CurrentState OUT_OF_ORDER is set", new object[2]
                    {
            CurrentState,
            value
                    });
                Log.Debug(GetType().Name, nameof(CurrentState), "CurrentStateProperty", "CurrentState changing from {0} to {1}", new object[2]
                {
          CurrentState,
          value
                });
                _currentState = value;
                NotifyCurrentTransactionStatusChanged();
            }
        }

        public virtual IControllerStatus CurrentDeviceStatus { get; set; } = new ControllerStatus();

        public event EventHandler<StringResult> ConnectionEvent;

        public void OnConnectionEvent(object sender, StringResult e)
        {
            if (ConnectionEvent == null)
                return;
            ConnectionEvent(this, e);
        }

        public event EventHandler<EventArgs> NotifyCurrentTransactionStatusChangedEvent;

        protected void OnNotifyCurrentTransactionStatusChangedEvent(object sender, EventArgs e)
        {
            if (NotifyCurrentTransactionStatusChangedEvent == null)
                return;
            NotifyCurrentTransactionStatusChangedEvent(this, e);
        }

        public event EventHandler<DeviceTransaction> TransactionStartedEvent;

        public event EventHandler<DeviceStatusChangedEventArgs> RaiseDeviceStatusChangedEvent;

        protected void OnRaiseDeviceStatusChangedEvent(DeviceStatusChangedEventArgs e)
        {
            EventHandler<DeviceStatusChangedEventArgs> statusChangedEvent = RaiseDeviceStatusChangedEvent;
            if (statusChangedEvent == null)
                return;
            statusChangedEvent(this, e);
        }

        public event EventHandler<ControllerStateChangedEventArgs> RaiseControllerStateChangedEvent;

        protected void OnRaiseControllerStateChangedEvent(ControllerStateChangedEventArgs e)
        {
            EventHandler<ControllerStateChangedEventArgs> stateChangedEvent = RaiseControllerStateChangedEvent;
            if (stateChangedEvent == null)
                return;
            stateChangedEvent(this, e);
        }

        public event EventHandler<DeviceStateChangedEventArgs> RaiseDeviceStateChangedEvent;

        public void OnRaiseDeviceStateChangedEvent(DeviceStateChangedEventArgs e)
        {
            EventHandler<DeviceStateChangedEventArgs> stateChangedEvent = RaiseDeviceStateChangedEvent;
            if (stateChangedEvent == null)
                return;
            stateChangedEvent(this, e);
        }

        public event EventHandler<CountChangedEventArgs> RaiseCountChangedEvent;

        public void OnRaiseCountChangedEvent(CountChangedEventArgs e)
        {
            EventHandler<CountChangedEventArgs> countChangedEvent = RaiseCountChangedEvent;
            if (countChangedEvent == null)
                return;
            countChangedEvent(this, e);
        }

        public event EventHandler<ConnectionEventArgs> RaiseConnectionEvent;

        public void OnRaiseConnectionEvent(ConnectionEventArgs e)
        {
            EventHandler<ConnectionEventArgs> raiseConnectionEvent = RaiseConnectionEvent;
            if (raiseConnectionEvent == null)
                return;
            raiseConnectionEvent(this, e);
        }

        public event EventHandler<CITResult> CITResultEvent;

        protected void OnCITResultEvent(object sender, CITResult e)
        {
            if (CITResultEvent == null)
                return;
            CITResultEvent(this, e);
        }

        public event EventHandler<EventArgs> DoorOpenEvent;

        protected void OnDoorOpenEvent(object sender, EventArgs e)
        {
            if (DoorOpenEvent == null)
                return;
            DoorOpenEvent(this, e);
        }

        public event EventHandler<EventArgs> DoorClosedEvent;

        protected void OnDoorClosedEvent(object sender, EventArgs e)
        {
            if (DoorClosedEvent == null)
                return;
            DoorClosedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagRemovedEvent;

        protected void OnBagRemovedEvent(object sender, EventArgs e)
        {
            if (BagRemovedEvent == null)
                return;
            BagRemovedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagPresentEvent;

        protected void OnBagPresentEvent(object sender, EventArgs e)
        {
            if (BagPresentEvent == null)
                return;
            BagPresentEvent(this, e);
        }

        public event EventHandler<EventArgs> BagOpenedEvent;

        protected void OnBagOpenedEvent(object sender, EventArgs e)
        {
            if (BagOpenedEvent == null)
                return;
            BagOpenedEvent(this, e);
        }

        public event EventHandler<EventArgs> BagClosedEvent;

        protected void OnBagClosedEvent(object sender, EventArgs e)
        {
            if (BagClosedEvent == null)
                return;
            BagClosedEvent(this, e);
        }

        public event EventHandler<ControllerStatus> BagFullAlertEvent;

        protected void OnBagFullAlertEvent(object sender, ControllerStatus e)
        {
            if (BagFullAlertEvent == null)
                return;
            BagFullAlertEvent(this, e);
        }

        public event EventHandler<ControllerStatus> BagFullWarningEvent;

        protected void OnBagFullWarningEvent(object sender, ControllerStatus e)
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

        public virtual void OnTransactionStartedEvent(object sender, DeviceTransaction e)
        {
            if (TransactionStartedEvent == null)
                return;
            TransactionStartedEvent(this, e);
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DeviceStatusChangedEventArgs> StatusReportEvent;

        public event EventHandler<DeviceTransactionResult> TransactionStatusEvent;

        public event EventHandler<DeviceTransactionResult> TransactionEndEvent;

        public event EventHandler<DeviceTransactionResult> DropResultEvent;

        public event EventHandler<DeviceTransactionResult> CashInStartedEvent;

        public event EventHandler<DeviceTransactionResult> CountEndEvent;

        public event EventHandler<DeviceTransactionResult> CountStartedEvent;

        public event EventHandler<DeviceTransactionResult> CountPauseEvent;

        public event EventHandler<DeviceTransactionResult> EscrowDropEvent;

        public event EventHandler<DeviceTransactionResult> EscrowRejectEvent;

        public event EventHandler<DeviceTransactionResult> EscrowOperationCompleteEvent;

        public event EventHandler<EventArgs> EscrowJamStartEvent;

        public event EventHandler<EventArgs> EscrowJamClearWaitEvent;

        public event EventHandler<EventArgs> EscrowJamEndRequestEvent;

        public event EventHandler<EventArgs> EscrowJamEndEvent;

        public event EventHandler<EventArgs> NoteJamStartEvent;

        public event EventHandler<EventArgs> NoteJamClearWaitEvent;

        public event EventHandler<EventArgs> NoteJamEndRequestEvent;

        public event EventHandler<EventArgs> NoteJamEndEvent;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Initialise() => throw new NotImplementedException();

        public virtual void CashInStart() => throw new NotImplementedException();

        public virtual void CountNotes() => throw new NotImplementedException();

        public virtual void CountCoins() => throw new NotImplementedException();

        public virtual void CountBoth() => throw new NotImplementedException();

        public virtual void Connect() => throw new NotImplementedException();

        public virtual void Disconnect() => throw new NotImplementedException();

        public virtual void ResetDevice(bool openEscrow = false) => throw new NotImplementedException();

        public virtual void SetCurrency(string currency) => throw new NotImplementedException();

        public abstract void ClearEscrowJam();

        public abstract void ClearNotesinEscrowWithDrop();

        public abstract void EndEscrowJam();

        protected void NotifyCurrentTransactionStatusChanged()
        {
            NotifyPropertyChanged("CanCount");
            NotifyPropertyChanged("CanPauseCount");
            NotifyPropertyChanged("CanEscrowDrop");
            NotifyPropertyChanged("CanEscrowReject");
            NotifyPropertyChanged("CanTransactionEnd");
            OnNotifyCurrentTransactionStatusChangedEvent(this, EventArgs.Empty);
        }

        protected void OnStatusReportEvent(
          object sender,
          DeviceStatusChangedEventArgs deviceStatusResult)
        {
            if (StatusReportEvent == null)
                return;
            StatusReportEvent(this, deviceStatusResult);
        }

        public virtual void OnTransactionEndEvent(object sender, DeviceTransactionResult e)
        {
            if (TransactionEndEvent == null)
                return;
            TransactionEndEvent(this, e);
        }

        public virtual void OnCashInStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (CashInStartedEvent == null)
                return;
            CashInStartedEvent(this, e);
        }

        public void OnTransactionStatusEvent(
          object sender,
          DeviceTransactionResult deviceTransactionResult)
        {
            if (TransactionStatusEvent == null)
                return;
            TransactionStatusEvent(this, deviceTransactionResult);
        }

        public virtual void OnCountEndEvent(object sender, DeviceTransactionResult e)
        {
            if (CountEndEvent == null)
                return;
            CountEndEvent(this, e);
        }

        public virtual void OnCountStartedEvent(object sender, DeviceTransactionResult e)
        {
            if (CountStartedEvent == null)
                return;
            CountStartedEvent(this, e);
        }

        public virtual void OnCountPauseEvent(object sender, DeviceTransactionResult e)
        {
            if (CountPauseEvent == null)
                return;
            CountPauseEvent(this, e);
        }

        public virtual void OnEscrowDropEvent(object sender, DeviceTransactionResult e)
        {
            if (EscrowDropEvent == null)
                return;
            EscrowDropEvent(this, e);
        }

        public virtual void OnEscrowRejectEvent(object sender, DeviceTransactionResult e)
        {
            if (EscrowRejectEvent == null)
                return;
            EscrowRejectEvent(this, e);
        }

        public virtual void OnEscrowOperationCompleteEvent(object sender, DeviceTransactionResult e)
        {
            if (EscrowOperationCompleteEvent == null)
                return;
            EscrowOperationCompleteEvent(this, e);
        }

        public virtual void OnNoteJamStartEvent(object sender, EventArgs e)
        {
            if (NoteJamStartEvent == null)
                return;
            NoteJamStartEvent(this, e);
        }

        public virtual void OnNoteJamClearWaitEvent(object sender, EventArgs e)
        {
            if (NoteJamClearWaitEvent == null)
                return;
            NoteJamClearWaitEvent(this, e);
        }

        public virtual void OnNoteJamEndRequestEvent(object sender, EventArgs e)
        {
            if (NoteJamEndRequestEvent == null)
                return;
            NoteJamEndRequestEvent(this, e);
        }

        public virtual void OnNoteJamEndEvent(object sender, EventArgs e)
        {
            if (NoteJamEndEvent == null)
                return;
            NoteJamEndEvent(this, e);
        }

        public virtual void OnEscrowJamStartEvent(object sender, EventArgs e)
        {
            if (EscrowJamStartEvent == null)
                return;
            EscrowJamStartEvent(this, e);
        }

        public virtual void OnEscrowJamClearWaitEvent(object sender, EventArgs e)
        {
            if (EscrowJamClearWaitEvent == null)
                return;
            EscrowJamClearWaitEvent(this, e);
        }

        public virtual void OnEscrowJamEndRequestEvent(object sender, EventArgs e)
        {
            if (EscrowJamEndRequestEvent == null)
                return;
            EscrowJamEndRequestEvent(this, e);
        }

        public virtual void OnEscrowJamEndEvent(object sender, EventArgs e)
        {
            if (EscrowJamEndEvent == null)
                return;
            EscrowJamEndEvent(this, e);
        }

        public abstract void ShowDeviceController();

        public abstract void StartCIT(string sealNumber);

        public abstract void EndCIT(string bagnumber);

        public abstract void TransactionStart(
          string currency,
          string accountNumber,
          string sessionID,
          string transactionID,
          long transactionLimitCents = 9223372036854775807,
          long transactionValueCents = 0);

        public abstract bool CanEndCount { get; }

        public abstract bool CanEscrowDrop { get; }

        public abstract bool CanCount { get; }

        public abstract void Count();

        public abstract bool CanPauseCount { get; }

        public abstract void PauseCount();

        public abstract void EscrowDrop();

        public abstract void EscrowReject();

        public abstract bool CanTransactionEnd { get; }

        public abstract void TransactionEnd();

        public abstract void ClearNoteJam();

        public abstract bool CanEscrowReject { get; }

        public long DroppedAmountCents => (long)CurrentTransaction?.CurrentTransactionResult?.TotalDroppedAmountCents;

        public double DroppedAmountMajorCurrency => DroppedAmountCents / 100.0;

        public abstract bool CanClearNoteJam { get; }
    }
}
