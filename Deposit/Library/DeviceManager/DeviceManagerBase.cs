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
      get => _controllerState;
      set
      {
        if (_controllerState == value)
          return;
        _controllerState = value;
        OnRaiseControllerStateChangedEvent(new ControllerStateChangedEventArgs(value));
        NotifyPropertyChanged(nameof (ControllerState));
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
        NotifyPropertyChanged(nameof (DeviceState));
      }
    }

    public virtual string Currency { get; set; }

    private int ConnectionID { get; set; }

    public event EventHandler<DeviceStatusChangedEventArgs> RaiseDeviceStatusChangedEvent;

    private void OnRaiseDeviceStatusChangedEvent(DeviceStatusChangedEventArgs e)
    {
      EventHandler<DeviceStatusChangedEventArgs> statusChangedEvent = RaiseDeviceStatusChangedEvent;
      if (statusChangedEvent == null)
        return;
      statusChangedEvent(this, e);
    }

    public event EventHandler<ControllerStateChangedEventArgs> RaiseControllerStateChangedEvent;

    private void OnRaiseControllerStateChangedEvent(ControllerStateChangedEventArgs e)
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      PropertyChangedEventHandler propertyChanged = PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
