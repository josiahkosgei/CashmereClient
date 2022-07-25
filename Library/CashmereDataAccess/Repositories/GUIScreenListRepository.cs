using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenListRepository : RepositoryBase<GUIScreenList>, IGuiScreenListRepository
    {
        public GUIScreenListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}