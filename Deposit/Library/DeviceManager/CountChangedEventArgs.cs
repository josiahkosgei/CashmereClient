using Cashmere.Library.Standard.Statuses;
using System;

namespace DeviceManager
{
  public class CountChangedEventArgs : EventArgs
  {
    private DenominationResult _data;

    public CountChangedEventArgs(DenominationResult data) => this._data = data;

    public DenominationResult Data => this._data;
  }
}
