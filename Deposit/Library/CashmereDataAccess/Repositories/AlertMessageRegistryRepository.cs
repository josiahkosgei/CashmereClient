using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class AlertMessageRegistryRepository : RepositoryBase<AlertMessageRegistry>, IAlertMessageRegistryRepository
    {
        public AlertMessageRegistryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}