//DropResultResult

namespace Cashmere.Library.Standard.Statuses
{
  public class DropResultResult : StandardResult
  {
    public new object data { get; }

    public string DropDeviceID { get; set; }

    public string SessionID { get; set; }

    public string TransactionID { get; set; }

    public string DropID { get; set; }

    public int TotalNumberOfNotes { get; set; }

    public string TransactionNumber { get; set; }

    public DropMode DropMode { get; set; }

    public bool isMultiDrop { get; set; }

    public long DroppedAmountCents { get; set; }

    public DenominationResult DroppedDenomination { get; set; }
  }
}
