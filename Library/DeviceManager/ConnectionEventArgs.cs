
using System;
using Cashmere.Library.Standard.Statuses;

namespace DeviceManager
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(StandardResult data) => Data = data;

        public StandardResult Data { get; }
    }
}
