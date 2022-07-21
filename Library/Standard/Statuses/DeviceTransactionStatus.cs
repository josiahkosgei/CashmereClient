// DeviceTransactionStatus


namespace Cashmere.Library.Standard.Statuses
{
    public enum DeviceTransactionStatus
    {
        NONE = 0,
        ACTIVE = 1,
        COUNTING = 4,
        PAUSED = 16, // 0x00000010
        ESCROW_DROP = 32, // 0x00000020
        ESCROW_REJECT = 64, // 0x00000040
        DISPENSING = 128, // 0x00000080
        ERROR = 256, // 0x00000100
    }
}
