
// Type: CashmereDeposit.Models.DeviceManagerTest




using System;

namespace CashmereDeposit.Models
{
  public class DeviceManagerTest
  {
    public double Count(bool countNotes = true)
    {
      Random random = new Random();
      return countNotes ? random.Next(50, 100000) : random.Next(50, 100000) + random.NextDouble() % 5.0;
    }
  }
}
