using Cashmere.Library.Standard.Statuses;
using System;

namespace DeviceManager
{
  public class DeviceMessengerBase : IDeviceMessenger
  {
    public virtual StandardResult CreateConnection() => throw new NotImplementedException();

    public virtual string SendMessage(string message, bool expectACK = true) => throw new NotImplementedException();

    public virtual StandardResult TerminateConnection() => throw new NotImplementedException();

    public virtual StandardResult BeginCount() => throw new NotImplementedException();

    public virtual void SetCurrency(string currency) => throw new NotImplementedException();
  }
}
