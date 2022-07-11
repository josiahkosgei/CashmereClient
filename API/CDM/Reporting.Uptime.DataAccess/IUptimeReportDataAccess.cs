
using Cashmere.API.CDM.Reporting.UptimeDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cashmere.API.CDM.Reporting.UptimeDataAccess
{
  public interface IUptimeReportDataAccess
  {
    string ConnectionString { get; set; }

    Task<Device> GetDevice(Guid id);

    Task<List<UptimeMode>> GetUptimeModesByRange(
      Guid device,
      DateTime fromDate,
      DateTime toDate);

    Task<List<UptimeComponentState>> GetDeviceComponentsByRange(
      Guid device,
      DateTime fromDate,
      DateTime toDate);
  }
}
