// DeviceDrop


using System.Collections.Generic;

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceDrop
  {
    private string _currentDropID;

    public string CurrentDropID
    {
      get => this._currentDropID;
      set
      {
        if (!(this._currentDropID != value))
          return;
        this._currentDropID = value;
        if (value != null)
          this.Drops.Add(value, (DropResultResult) null);
      }
    }

    public Dictionary<string, DropResultResult> Drops { get; set; } = new Dictionary<string, DropResultResult>(5);
  }
}
