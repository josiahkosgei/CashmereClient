using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenRepository : RepositoryBase<GUIScreen>, IGUIScreenRepository
    {
        public GUIScreenRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<GUIScreen> GetCurrentGUIScreen(int Id)
        {
            return await DbContext.GuiScreens.Where(w => w.Id == Id).Include(i => i.GuiTextNavigation.ScreenTitleNavigation).FirstOrDefaultAsync();
        }

        public async Task<GUIScreen> GetGUIScreenByCode(Guid code)
        {
            return await DbContext.GuiScreens.Where(w => w.GUIScreenType.Code == code).Include(i => i.GuiTextNavigation.ScreenTitleNavigation).OrderBy(y => y.Id).FirstOrDefaultAsync();
        }
    }
}
