//DeviceNoteAcceptor

namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceNoteAcceptor
  {
    public DeviceNoteAcceptorType Type { get; set; }

    public DeviceState Status { get; set; }

    public string Currency { get; set; } = "";
  }
}
