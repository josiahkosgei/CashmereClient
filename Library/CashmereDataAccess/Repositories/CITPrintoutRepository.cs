using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITPrintoutRepository : RepositoryBase<CITPrintout>, ICITPrintoutRepository
    {
        public CITPrintoutRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}