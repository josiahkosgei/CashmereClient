//DeviceBag

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceBag
  {
    public string BagNumber { get; set; } = "";

    public BagState BagState { get; set; }

    public int NoteLevel { get; set; } = -1;

    public long NoteCapacity { get; set; } = -1;

    public long ValueLevel { get; set; } = -1;

    public long ValueCapacity { get; set; } = -1;

    public int PercentFull { get; set; } = -1;
  }
}
