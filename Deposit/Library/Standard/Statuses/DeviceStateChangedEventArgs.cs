// DeviceStateChangedEventArgs


using System;

namespace Cashmere.Library.Standard.Statuses
{
    public class DeviceStateChangedEventArgs : EventArgs
    {
        private DeviceState _data;

        public DeviceStateChangedEventArgs(DeviceState data) => this._data = data;

        public DeviceState Data => this._data;
    }
}
