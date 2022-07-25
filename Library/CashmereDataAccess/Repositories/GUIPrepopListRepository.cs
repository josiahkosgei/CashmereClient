using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIPrepopListRepository : RepositoryBase<GUIPrepopList>, IGUIPrepopListRepository
    {
        public GUIPrepopListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}