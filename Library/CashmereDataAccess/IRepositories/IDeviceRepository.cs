using Cashmere.Library.CashmereDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceRepository : IAsyncRepository<Device>
    {
        public Task<Device> GetByIdAsync(Guid DeviceId);
        public Task<List<ApplicationUser>> GetByUserGroupAsync(int? UserGroup);
        public Task<ApplicationUser> GetByUserGroupAsync(int? UserGroup, string Username);
        public Task<Device> GetDevice(string machineName);
        public Task<Device> GetDeviceWithNavigations(string machineName);
        public Task<IList<Device>> GetAllAsync();
        public Task<Device> GetDeviceScreenList(string machineName);
        public Task<Device> GetDeviceLanguageList(string machineName);
        public Task<Device> GetDeviceCurrencyList(string machineName);
        public Task<Device> GetDeviceTransactionTypeList(string machineName);
    }

}
