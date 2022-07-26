using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ThisDeviceRepository : RepositoryBase<ThisDevice>, IThisDeviceRepository
    {
        public ThisDeviceRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}