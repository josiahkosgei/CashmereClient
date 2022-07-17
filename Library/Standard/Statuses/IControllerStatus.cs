// IControllerStatus


namespace Cashmere.Library.Standard.Statuses
{
  public interface IControllerStatus
  {
    DeviceBag Bag { get; set; }

    ControllerState ControllerState { get; set; }

    DeviceEscrow Escrow { get; set; }

    DeviceNoteAcceptor NoteAcceptor { get; set; }

    DeviceSensor Sensor { get; set; }

    ControllerDeviceTransaction Transaction { get; set; }
  }
}
