//CashmereDeviceState

using System;

namespace Cashmere.Library.Standard.Statuses
{
  [Flags]
  public enum CashmereDeviceState
  {
    NONE = 0,
    DEVICE_MANAGER = 1,
    DATABASE = 2,
    PRINTER = 4,
    SERVER_CONNECTION = 8,
    CONTROLLER = 16, // 0x00000010
    COUNTING_DEVICE = 32, // 0x00000020
    BAG = 64, // 0x00000040
    SAFE = 128, // 0x00000080
    DEVICE_LOCK = 256, // 0x00000100
    ESCROW_JAM = 512, // 0x00000200
    GUI_STARTUP_FAILED = 1024, // 0x00000400
    LICENSE = 2048, // 0x00000800
    HDD_FULL = 4096, // 0x00001000
  }
}
