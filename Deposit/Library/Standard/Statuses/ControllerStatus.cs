// ControllerStatus


namespace Cashmere.Library.Standard.Statuses
{
    public class ControllerStatus : IControllerStatus
    {
        public ControllerState ControllerState { get; set; }

        public DeviceNoteAcceptor NoteAcceptor { get; set; } = new DeviceNoteAcceptor();

        public DeviceEscrow Escrow { get; set; } = new DeviceEscrow();

        public DeviceBag Bag { get; set; } = new DeviceBag();

        public DeviceSensor Sensor { get; set; } = new DeviceSensor();

        public ControllerDeviceTransaction Transaction { get; set; } = new ControllerDeviceTransaction();
    }
}
