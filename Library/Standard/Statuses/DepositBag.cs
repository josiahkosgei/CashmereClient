// DepositBag


namespace Cashmere.Library.Standard.Statuses
{
  public class DepositBag
  {
    public string BagNumber { get; set; }

    public string Status { get; set; }

    public int NoteLevel { get; set; }

    public int NoteCapacity { get; set; }

    public int ValueLevel { get; set; }

    public int ValueCapacity { get; set; }

    public int PercentFull { get; set; }
  }
}
