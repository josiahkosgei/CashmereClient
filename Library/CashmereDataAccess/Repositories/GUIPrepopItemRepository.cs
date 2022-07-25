using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIPrepopItemRepository : RepositoryBase<GUIPrepopItem>, IGUIPrepopItemRepository
    {
        public GUIPrepopItemRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}