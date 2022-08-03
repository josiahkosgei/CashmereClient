//ControllerStateChangedEventArgs

using System;

namespace Cashmere.Library.Standard.Statuses
{
  public class ControllerStateChangedEventArgs : EventArgs
  {
    private ControllerState _data;

    public ControllerStateChangedEventArgs(ControllerState data) => this._data = data;

    public ControllerState ControllerState => this._data;
  }
}
