// DropStatusResultStatus


namespace Cashmere.Library.Standard.Statuses
{
    public enum DropStatusResultStatus
    {
        ERROR = -1, // 0xFFFFFFFF
        NONE = 0,
        COUNTING = 1,
        DROPPING = 2,
        WAIT_ACCEPTOR = 3,
        ESCROW_WAIT = 4,
        ESCROW_REJECT = 5,
        ESCROW_DROP = 6,
        ESCROW_DONE = 7,
        DONE = 8,
    }
}
