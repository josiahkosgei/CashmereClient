using Cashmere.Library.Standard.Statuses;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeviceManager
{
  public abstract class DeviceManagerBase : IDeviceManager, INotifyPropertyChanged
  {
    public ControllerState _controllerState;
    public DeviceState _deviceState;

    public DeviceMessengerBase DeviceMessenger { get; set; }

    public ControllerState ControllerState
    {
      get => this._controllerState;
      set
      {
        if (this._controllerState == value)
          return;
        this._controllerState = value;
        this.OnRaiseControllerStateChangedEvent(new ControllerStateChangedEventArgs(value));
        this.NotifyPropertyChanged(nameof (ControllerState));
      }
    }

    public DeviceState DeviceState
    {
      get => this._deviceState;
      set
      {
        if (this._deviceState == value)
          return;
        this._deviceState = value;
        this.OnRaiseDeviceStateChangedEvent(new DeviceStateChangedEventArgs(value));
        this.NotifyPropertyChanged(nameof (DeviceState));
      }
    }

    public virtual string Currency { get; set; }

    private int ConnectionID { get; set; }

    public event EventHandler<DeviceStatusChangedEventArgs> RaiseDeviceStatusChangedEvent;

    private void OnRaiseDeviceStatusChangedEvent(DeviceStatusChangedEventArgs e)
    {
      EventHandler<DeviceStatusChangedEventArgs> statusChangedEvent = this.RaiseDeviceStatusChangedEvent;
      if (statusChangedEvent == null)
        return;
      statusChangedEvent((object) this, e);
    }

    public event EventHandler<ControllerStateChangedEventArgs> RaiseControllerStateChangedEvent;

    private void OnRaiseControllerStateChangedEvent(ControllerStateChangedEventArgs e)
    {
      EventHandler<ControllerStateChangedEventArgs> stateChangedEvent = this.RaiseControllerStateChangedEvent;
      if (stateChangedEvent == null)
        return;
      stateChangedEvent((object) this, e);
    }

    public event EventHandler<DeviceStateChangedEventArgs> RaiseDeviceStateChangedEvent;

    public void OnRaiseDeviceStateChangedEvent(DeviceStateChangedEventArgs e)
    {
      EventHandler<DeviceStateChangedEventArgs> stateChangedEvent = this.RaiseDeviceStateChangedEvent;
      if (stateChangedEvent == null)
        return;
      stateChangedEvent((object) this, e);
    }

    public event EventHandler<CountChangedEventArgs> RaiseCountChangedEvent;

    public void OnRaiseCountChangedEvent(CountChangedEventArgs e)
    {
      EventHandler<CountChangedEventArgs> countChangedEvent = this.RaiseCountChangedEvent;
      if (countChangedEvent == null)
        return;
      countChangedEvent((object) this, e);
    }

    public event EventHandler<ConnectionEventArgs> RaiseConnectionEvent;

    public void OnRaiseConnectionEvent(ConnectionEventArgs e)
    {
      EventHandler<ConnectionEventArgs> raiseConnectionEvent = this.RaiseConnectionEvent;
      if (raiseConnectionEvent == null)
        return;
      raiseConnectionEvent((object) this, e);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void CashInStart() => throw new NotImplementedException();

    public virtual void CountNotes() => throw new NotImplementedException();

    public virtual void CountCoins() => throw new NotImplementedException();

    public virtual void CountBoth() => throw new NotImplementedException();

    public virtual void Connect() => throw new NotImplementedException();

    public virtual void Disconnect() => throw new NotImplementedException();

    public virtual void ResetDevice(bool openEscrow = false) => throw new NotImplementedException();

    public virtual void SetCurrency(string currency) => throw new NotImplementedException();

    public abstract void ClearEscrowJam();

    public abstract void EndEscrowJam();
  }
}
