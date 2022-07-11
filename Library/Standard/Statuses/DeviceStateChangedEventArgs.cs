
// Type: Cashmere.Library.Standard.Statuses.DeviceStateChangedEventArgs


using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceStateChangedEventArgs : EventArgs
  {
    private DeviceState _data;

    public DeviceStateChangedEventArgs(DeviceState data) => _data = data;

    public DeviceState Data => _data;
  }
}
