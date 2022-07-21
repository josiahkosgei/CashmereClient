// DeviceTransactionType


namespace Cashmere.Library.Standard.Statuses
{
    public enum DeviceTransactionType
    {
        NONE = 0,
        DROP = 1,
        EXCHANGE = 2,
        MULTIDROP = 4,
        DISPENSE_AMOUNT = 8,
        DISPENSE_NOTES = 16, // 0x00000010
    }
}
