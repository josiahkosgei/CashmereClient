using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationLogRepository : RepositoryBase<ApplicationLog>, IApplicationLogRepository
    {
        public ApplicationLogRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
