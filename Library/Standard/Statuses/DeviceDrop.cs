
// Type: Cashmere.Library.Standard.Statuses.DeviceDrop


using System.Collections.Generic;

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceDrop
  {
    private string _currentDropID;

    public string CurrentDropID
    {
      get => _currentDropID;
      set
      {
        if (!(_currentDropID != value))
          return;
        _currentDropID = value;
        if (value == null)
          return;
        Drops.Add(value, null);
      }
    }

    public Dictionary<string, DropResultResult> Drops { get; set; } = new Dictionary<string, DropResultResult>(5);
  }
}
