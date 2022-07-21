using Cashmere.Library.Standard.Statuses;
using System;

namespace DeviceManager
{
    public class CountChangedEventArgs : EventArgs
    {
        public CountChangedEventArgs(DenominationResult data) => Data = data;

        public DenominationResult Data { get; }
    }
}
