//DeviceStatusChangedEventArgs

using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceStatusChangedEventArgs : EventArgs
  {
    private ControllerStatus _controllerStatus;

    public DeviceStatusChangedEventArgs(
      ControllerStatus data,
      DeviceManagerState deviceManagerState = DeviceManagerState.NONE)
    {
      this.DeviceManagerState = deviceManagerState;
      this._controllerStatus = data;
    }

    public DeviceManagerState DeviceManagerState { get; set; }

    public ControllerStatus ControllerStatus => this._controllerStatus;
  }
}
