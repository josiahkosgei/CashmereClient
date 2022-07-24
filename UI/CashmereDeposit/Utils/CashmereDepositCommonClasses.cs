
// Type: CashmereDeposit.Utils.CashmereDepositCommonClasses




using System;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;


namespace CashmereDeposit.Utils
{
    internal static class CashmereDepositCommonClasses
    {
        internal static DeviceStatus GenerateDeviceStatus(Guid deviceID)
        {
            return new DeviceStatus()
            {
                DeviceId = deviceID,
                MachineName = Environment.MachineName.ToUpperInvariant(),
                BagNoteCapacity = 0.ToString() ?? "",
                BagNoteLevel = 0,
                BagNumber = "N/A",
                BagPercentFull = 0,
                BagStatus = "N/A",
                BagValueCapacity = new long?(0L),
                BagValueLevel = new long?(0L),
                BaCurrency = "N/A",
                BaStatus = "N/A",
                BaType = "N/A",
                ControllerState = "N/A",
                CurrentStatus = 1024,
                EscrowPosition = "N/A",
                EscrowStatus = "N/A",
                EscrowType = "N/A",
                Id = Guid.NewGuid(),
                Modified = new DateTime?(DateTime.Now),
                SensorsStatus = "N/A",
                SensorsType = "N/A",
                SensorsValue = 0,
                SensorsBag = "N/A",
                SensorsDoor = "N/A"
            };
        }
    }
}
