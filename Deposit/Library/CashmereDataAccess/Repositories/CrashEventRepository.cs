using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CrashEventRepository : RepositoryBase<CrashEvent>, ICrashEventRepository
    {
        public CrashEventRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}