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
        private static IDeviceRepository _deviceRepository { get; set; }

        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;

        private static IUptimeComponentStateRepository _uptimeComponentStateRepository { get; set; }
        private static IUptimeModeRepository _uptimeModeRepository { get; set; }


        private UptimeMonitor()
        {

            _deviceRepository = IoC.Get<IDeviceRepository>();
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            _uptimeComponentStateRepository = IoC.Get<IUptimeComponentStateRepository>();
            _uptimeModeRepository = IoC.Get<IUptimeModeRepository>();

            DateTime now = DateTime.Now;
            var uptimeModes = _uptimeModeRepository.GetEndDateHasValueAsync();
            foreach (UptimeMode uptimeMode in uptimeModes)
            {
                uptimeMode.EndDate = new DateTime?(now);
                _uptimeModeRepository.UpdateAsync(uptimeMode);
            }


            var uptimeComponentStates = _uptimeComponentStateRepository.GetEndDateHasValueAsync();
            foreach (UptimeComponentState uptimeComponentState in uptimeComponentStates)
            {
                uptimeComponentState.EndDate = new DateTime?(now);
                _uptimeComponentStateRepository.UpdateAsync(uptimeComponentState);

            }


        }

        public UptimeMonitor GetInstance()
        {
            if (_uptimeMonitor == null)
                _uptimeMonitor = new UptimeMonitor();
            return _uptimeMonitor;
        }

        public static async void SetCurrentUptimeMode(UptimeModeType state)
        {
            try
            {
                if (state == CurrentUptimeMode)
                    return;
                DateTime now = DateTime.Now;
                Device device = ApplicationViewModel.GetDeviceAsync();
                CurrentUptimeMode = state;
                UptimeMode uptimeMode =  _uptimeModeRepository.GetByDeviceIdAsync(device.Id);
                if (uptimeMode != null)
                    uptimeMode.EndDate = new DateTime?(now);
                _uptimeModeRepository.AddAsync(new UptimeMode()
                {
                    Id = GuidExt.UuidCreateSequential(),
                    Device = device.Id,
                    Created = now,
                    StartDate = now,
                    DeviceMode = (int)state
                });
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeMode), ex.MessageString(), Array.Empty<object>());
            }

        }

        public static async void SetCurrentUptimeComponentState(CashmereDeviceState state)
        {
            try
            {
                if (CurrentUptimeComponentState.HasFlag(state))
                    return;
                DateTime now = DateTime.Now;
                Device device =  _deviceRepository.GetDevice(Environment.MachineName);
                CurrentUptimeComponentState = state;
                var uptimeComponentState =  _uptimeComponentStateRepository.GetByDeviceIdAsync(device.Id, (int)CurrentUptimeComponentState);
                if (uptimeComponentState == null)
                     _uptimeComponentStateRepository.AddAsync(new UptimeComponentState()
                    {
                        Id = GuidExt.UuidCreateSequential(),
                        Device = device.Id,
                        Created = now,
                        StartDate = now,
                        ComponentState = (int)state
                    });
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(SetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
            }

        }

        public static async void UnSetCurrentUptimeComponentState(CashmereDeviceState state)
        {
            try
            {
                if (!CurrentUptimeComponentState.HasFlag(state))
                    return;
                DateTime now = DateTime.Now;
                Device device = ApplicationViewModel.GetDeviceAsync();
                CurrentUptimeComponentState = state;
                UptimeComponentState entity =  _uptimeComponentStateRepository.GetByDeviceIdAsync(device.Id, (int)CurrentUptimeComponentState);
                if (entity != null)
                {
                    if (now - entity.StartDate < TimeSpan.FromMinutes(1.0))
                         _uptimeComponentStateRepository.DeleteAsync(entity);
                    else
                        entity.EndDate = new DateTime?(now);
                }
                _uptimeComponentStateRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UptimeMonitor), "Error", nameof(UnSetCurrentUptimeComponentState), ex.MessageString(), Array.Empty<object>());
            }

        }
    }
}
