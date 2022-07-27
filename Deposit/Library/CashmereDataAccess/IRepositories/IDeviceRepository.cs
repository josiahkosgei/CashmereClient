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
        public Device GetByIdAsync(Guid DeviceId);
        public List<ApplicationUser> GetByUserGroupAsync(int? UserGroup);
        public ApplicationUser GetByUserGroupAsync(int? UserGroup, string Username);
        public Device GetDevice(string machineName);
        public Device GetDeviceWithNavigations(string machineName);
        public IList<Device> GetAllAsync();
        public Device GetDeviceScreenList(string machineName);
        public Device GetDeviceLanguageList(string machineName);
        public Device GetDeviceCurrencyList(string machineName);
        public Device GetDeviceTransactionTypeList(string machineName);
    }

}
