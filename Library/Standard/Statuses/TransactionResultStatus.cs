
// Type: Cashmere.Library.Standard.Statuses.TransactionResultStatus


namespace Cashmere.Library.Standard.Statuses
{
  public enum TransactionResultStatus
  {
    ERROR = -1, // 0xFFFFFFFF
    NONE = 0,
    IDLE = 1,
    COUNTING = 2,
    PAUSED = 3,
    ESCROW_DROP = 4,
    ESCROW_REJECT = 5,
  }
}
