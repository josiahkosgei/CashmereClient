using Cashmere.Library.Standard.Statuses;
using System;

namespace DeviceManager
{
  public class ConnectionEventArgs : EventArgs
  {
    private StandardResult _data;

    public ConnectionEventArgs(StandardResult data) => this._data = data;

    public StandardResult Data => this._data;
  }
}
