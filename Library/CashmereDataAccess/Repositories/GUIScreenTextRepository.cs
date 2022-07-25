using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenTextRepository : RepositoryBase<GUIScreenText>, IGuiScreenTextRepository
    {
        public GUIScreenTextRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}