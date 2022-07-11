
// Type: Cashmere.Library.Standard.Statuses.DeviceSensor


namespace Cashmere.Library.Standard.Statuses
{
  public class DeviceSensor
  {
    public DeviceSensorType Type { get; set; }

    public DeviceSensorState Status { get; set; }

    public int Value { get; set; }

    public DeviceSensorDoor Door { get; set; }

    public DeviceSensorBag Bag { get; set; }
  }
}
