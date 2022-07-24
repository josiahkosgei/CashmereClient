using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GuiScreenListScreenRepository : RepositoryBase<GuiScreenListScreen>, IGuiScreenListScreenRepository
    {
        public GuiScreenListScreenRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<GuiScreenListScreen> GetByGUIScreenId(int Id)
        {
            return await DbContext.GuiScreenListScreens.Where(w=>w.GUIScreenNavigation.Id == Id).FirstOrDefaultAsync();
        }
    }
}
