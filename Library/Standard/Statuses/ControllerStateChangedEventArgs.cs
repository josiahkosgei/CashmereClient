
// Type: Cashmere.Library.Standard.Statuses.ControllerStateChangedEventArgs


using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class ControllerStateChangedEventArgs : EventArgs
  {
    private ControllerState _data;

    public ControllerStateChangedEventArgs(ControllerState data) => _data = data;

    public ControllerState ControllerState => _data;
  }
}
