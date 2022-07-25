using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIPrepopListItemRepository : RepositoryBase<GUIPrepopListItem>, IGUIPrepopListItemRepository
    {
        public GUIPrepopListItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}