
// Type: CashmereDeposit.Models.UptimeMonitor

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;

using CashmereDeposit.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.Models
{
  internal class UptimeMonitor
  {
    private static UptimeMonitor _uptimeMonitor = new UptimeMonitor();

    public static UptimeModeType CurrentUptimeMode { get; private set; }

    public static CashmereDeviceState CurrentUptimeComponentState { get; private set; }

    private UptimeMonitor()
    {
        using DepositorDBContext DBContext = new DepositorDBContext();
        DateTime now = DateTime.Now;
        DbSet<UptimeMode> uptimeModes = DBContext.UptimeModes;
        Expression<Func<UptimeMode, bool>> predicate1 = x => !x.EndDate.HasValue;
        foreach (UptimeMode uptimeMode in uptimeModes.Where(predicate1))
            uptimeMode.EndDate = new DateTime?(now);
        DbSet<UptimeComponentState> uptimeComponentStates = DBContext.UptimeComponentStates;
        Expression<Func<UptimeComponentState, bool>> predicate2 = x => !x.EndDate.HasValue;
        foreach (UptimeComponentState uptimeComponentState in uptimeComponentStates.Where(predicate2))
            uptimeComponentState.EndDate = new DateTime?(now);
        ApplicationViewModel.SaveToDatabase(DBContext);
    }

    public UptimeMonitor GetInstance()
    {
      if (_uptimeMonitor == null)
        _uptimeMonitor = new UptimeMonitor();
      return _uptimeMonitor;
    }

    public static void SetCurrentUptimeMode(UptimeModeType state)
    {
        using DepositorDBContext DBContext = new DepositorDBContext();
        try
        {
            if (state == CurrentUptimeMode)
                return;
            DateTime now = DateTime.Now;
            Device device = ApplicationViewModel.GetDevice(DBContext);
            CurrentUptimeMode = state;
            UptimeMode uptimeMode = DBContext.UptimeModes.Where(x => x.Device == device.Id).OrderByDescending(x => x.Created).FirstOrDefault();
            if (uptimeMode != null)
                uptimeMode.EndDate = new DateTime?(now);
            DBContext.UptimeModes.Add(new UptimeMode()
            {
                Id = GuidExt.UuidCreateSequential(),
                Device = device.Id,
                Created = now,
                StartDate = now,
                DeviceMode = (int) state
            });
            ApplicationViewModel.SaveToDatabase(DBContext);
        }
        catch (Exception ex)
        {
            ApplicationViewModel.Log.Error(nameof (UptimeMonitor), "Error", nameof (SetCurrentUptimeMode), ex.MessageString(), Array.Empty<object>());
        }
    }

    public static void SetCurrentUptimeComponentState(CashmereDeviceState state)
    {
        using DepositorDBContext DBContext = new DepositorDBContext();
        try
        {
            if (CurrentUptimeComponentState.HasFlag(state))
                return;
            DateTime now = DateTime.Now;
            Device device = ApplicationViewModel.GetDevice(DBContext);
            CurrentUptimeComponentState = state;
            if (DBContext.UptimeComponentStates.Where(x => x.Device == device.Id && x.ComponentState == (int) state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault() == null)
                DBContext.UptimeComponentStates.Add(new UptimeComponentState()
                {
                    Id = GuidExt.UuidCreateSequential(),
                    Device = device.Id,
                    Created = now,
                    StartDate = now,
                    ComponentState = (int) state
                });
            ApplicationViewModel.SaveToDatabase(DBContext);
        }
        catch (Exception ex)
        {
            ApplicationViewModel.Log.Error(nameof (UptimeMonitor), "Error", nameof (SetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
        }
    }

    public static void UnSetCurrentUptimeComponentState(CashmereDeviceState state)
    {
        using DepositorDBContext DBContext = new DepositorDBContext();
        try
        {
            if (!CurrentUptimeComponentState.HasFlag(state))
                return;
            DateTime now = DateTime.Now;
            Device device = ApplicationViewModel.GetDevice(DBContext);
            CurrentUptimeComponentState = state;
            UptimeComponentState entity = DBContext.UptimeComponentStates.Where(x => x.Device == device.Id && x.ComponentState == (int) state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault();
            if (entity != null)
            {
                if (now - entity.StartDate < TimeSpan.FromMinutes(1.0))
                    DBContext.UptimeComponentStates.Remove(entity);
                else
                    entity.EndDate = new DateTime?(now);
            }
            ApplicationViewModel.SaveToDatabase(DBContext);
        }
        catch (Exception ex)
        {
            ApplicationViewModel.Log.Error(nameof (UptimeMonitor), "Error", nameof (UnSetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
        }
    }
  }
}
