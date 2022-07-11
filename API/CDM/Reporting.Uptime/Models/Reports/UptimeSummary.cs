
namespace Cashmere.API.CDM.Reporting.Uptime.Models.Reports
{
  public class UptimeSummary
  {
    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public double Uptime
    {
      get
      {
        TimeSpan timeSpan = ACTIVE + ADMIN + CIT;
        double totalSeconds1 = timeSpan.TotalSeconds;
        timeSpan = TOTAL;
        double totalSeconds2 = timeSpan.TotalSeconds;
        return totalSeconds1 / totalSeconds2;
      }
    }

    public TimeSpan ACTIVE { get; set; }

    public TimeSpan ADMIN { get; set; }

    public TimeSpan CIT { get; set; }

    public TimeSpan OUT_OF_ORDER { get; set; }

    public TimeSpan DEVICE_LOCKED { get; set; }

    public TimeSpan TOTAL => ACTIVE + ADMIN + OUT_OF_ORDER + CIT + DEVICE_LOCKED;
  }
}
