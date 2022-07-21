using Cashmere.Library.CashmereDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDeviceRepository: IAsyncRepository<Device>
    {
     public Task<Device> GetDevice(string machineName);
     public Task<IList<Device>> GetAllAsync();
    }
 
}
