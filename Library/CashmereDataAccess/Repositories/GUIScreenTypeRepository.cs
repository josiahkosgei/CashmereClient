using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenTypeRepository : RepositoryBase<GUIScreenType>, IGuiScreenTypeRepository
    {
        public GUIScreenTypeRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}