﻿using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceConfigRepository : RepositoryBase<DeviceConfig>, IDeviceConfigRepository
    {
        public DeviceConfigRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  IList<DeviceConfig> ExecuteStoredProc(int config_group)
        {
            var result = _depositorDBContext.DeviceConfigs.FromSqlRaw("EXECUTE  dbo.GetDeviceConfigByUserGroup @ConfigGroup = {0}", config_group).ToList();
            return result;
        }
    }
}