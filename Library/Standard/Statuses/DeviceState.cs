// DeviceState


namespace Cashmere.Library.Standard.Statuses
{
  public enum DeviceState
  {
    NONE = 0,
    OK = 2,
    IDLE = 4,
    COUNTING = 8,
    NO_DEVICE = 16, // 0x00000010
    NO_COMMS = 32, // 0x00000020
    JAM = 64, // 0x00000040
  }
}
