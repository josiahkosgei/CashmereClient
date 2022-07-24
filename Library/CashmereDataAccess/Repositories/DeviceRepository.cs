using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Device> GetDevicedd(string machineName)
        {
            var tasks = await Task.Run(() =>
                      {
                          return DbContext.Devices
                                     .Where(x => x.MachineName == machineName)
                                     //.ToListAsync()
                                     //.Include(x => x.Branch)
                                     .FirstOrDefaultAsync();
                      });
            var device = Task.Run(() => tasks);
            return device.Result ?? null;
        }
        public async Task<Device> GetDeviceAndNavigations(string machineName)
        {
            return await DbContext.Devices
                .Where(x => x.MachineName == machineName)
                .Include(x => x.Branch)
                 .FirstOrDefaultAsync();
        }
        public async Task<IList<Device>> GetAllAsync()
        {
            return await DbContext.Devices.ToListAsync();
        }

        public async Task<Device> GetByIdAsync(Guid DeviceId)
        {
            return await DbContext.Devices
             .Where(x => x.Id == DeviceId)
             .Include(x => x.Branch)
              .FirstOrDefaultAsync();
        } 
        public async Task<Device> GetDevice(string machineName)
        {
            return await DbContext.Devices
             .Where(x => x.MachineName == machineName)
             .Include(x => x.Branch)
             .Include(x => x.ConfigGroupNavigation)
             .FirstOrDefaultAsync();
        }
    }
}
