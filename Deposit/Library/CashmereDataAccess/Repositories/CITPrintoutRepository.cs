using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITPrintoutRepository : RepositoryBase<CITPrintout>, ICITPrintoutRepository
    {
        public CITPrintoutRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}