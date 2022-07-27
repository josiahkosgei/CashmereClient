using Cashmere.Library.Standard.Statuses;

namespace DeviceManager
{
    public interface IDeviceMessenger
    {
        StandardResult CreateConnection();

        StandardResult TerminateConnection();

        string SendMessage(string message, bool expectACK = true);
    }
}
