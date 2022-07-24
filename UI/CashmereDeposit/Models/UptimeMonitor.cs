using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using CashmereDeposit.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.Models
{
    internal class UptimeMonitor
    {
        private static UptimeMonitor _uptimeMonitor = new UptimeMonitor();

        public static UptimeModeType CurrentUptimeMode { get; private set; }

        public static CashmereDeviceState CurrentUptimeComponentState { get; private set; }

        private static IDeviceRepository _iDeviceRepository { get; set; }
       //  private static DepositorDBContext _depositorDBContext { get; set; }

        private UptimeMonitor()
        {

            _iDeviceRepository = IoC.Get<IDeviceRepository>();
            _depositorDBContext = IoC.Get<DepositorDBContext>();
            DateTime now = DateTime.Now;
            DbSet<UptimeMode> uptimeModes = _depositorDBContext.UptimeModes;
            Expression<Func<UptimeMode, bool>> predicate1 = x => !x.EndDate.HasValue;
            foreach (UptimeMode uptimeMode in uptimeModes.Where(predicate1))
                uptimeMode.EndDate = new DateTime?(now);
            DbSet<UptimeComponentState> uptimeComponentStates = _depositorDBContext.UptimeComponentStates;
            Expression<Func<UptimeComponentState, bool>> predicate2 = x => !x.EndDate.HasValue;
            foreach (UptimeComponentState uptimeComponentState in uptimeComponentStates.Where(predicate2))
                uptimeComponentState.EndDate = new DateTime?(now);
            _depositorDBContext.SaveChangesAsync().Wait();


        }

        public UptimeMonitor GetInstance()
        {
            if (_uptimeMonitor == null)
                _uptimeMonitor = new UptimeMonitor();
            return _uptimeMonitor;
        }

        public static void SetCurrentUptimeMode(UptimeModeType state)
        {
            try
            {
                if (state == CurrentUptimeMode)
                    return;
                DateTime now = DateTime.Now;
                Device device = ApplicationViewModel.GetDeviceAsync();
                CurrentUptimeMode = state;
                UptimeMode uptimeMode = _depositorDBContext.UptimeModes.Where(x => x.Device == device.Id).OrderByDescending(x => x.Created).FirstOrDefault();
                if (uptimeMode != null)
                    uptimeMode.EndDate = new DateTime?(now);
                _depositorDBContext.UptimeModes.Add(new UptimeMode()
                {
                    Id = GuidExt.UuidCreateSequential(),
                    Device = device.Id,
                    Created = now,
                    StartDate = now,
                    DeviceMode = (int)state
                });
                _depositorDBContext.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeMode), ex.MessageString(), Array.Empty<object>());
            }

        }

        public static void SetCurrentUptimeComponentState(CashmereDeviceState state)
        {
            try
            {
                if (CurrentUptimeComponentState.HasFlag(state))
                    return;
                DateTime now = DateTime.Now;
                Device device = _depositorDBContext.Devices.FirstOrDefault(f => f.MachineName == Environment.MachineName);
                CurrentUptimeComponentState = state;
                if (_depositorDBContext.UptimeComponentStates.Where(x => x.Device == device.Id && x.ComponentState == (int)state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault() == null)
                    _depositorDBContext.UptimeComponentStates.Add(new UptimeComponentState()
                    {
                        Id = GuidExt.UuidCreateSequential(),
                        Device = device.Id,
                        Created = now,
                        StartDate = now,
                        ComponentState = (int)state
                    });
                _depositorDBContext.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
            }

        }

        public static void UnSetCurrentUptimeComponentState(CashmereDeviceState state)
        {
            try
            {
                if (!CurrentUptimeComponentState.HasFlag(state))
                    return;
                DateTime now = DateTime.Now;
                Device device = ApplicationViewModel.GetDeviceAsync();
                CurrentUptimeComponentState = state;
                UptimeComponentState entity = _depositorDBContext.UptimeComponentStates.Where(x => x.Device == device.Id && x.ComponentState == (int)state && !x.EndDate.HasValue).OrderByDescending(x => x.Created).FirstOrDefault();
                if (entity != null)
                {
                    if (now - entity.StartDate < TimeSpan.FromMinutes(1.0))
                        _depositorDBContext.UptimeComponentStates.Remove(entity);
                    else
                        entity.EndDate = new DateTime?(now);
                }
                _depositorDBContext.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(UnSetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
            }

        }
    }
}
