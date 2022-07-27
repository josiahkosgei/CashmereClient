using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenRepository : RepositoryBase<GUIScreen>, IGUIScreenRepository
    {
        public GUIScreenRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  GUIScreen GetCurrentGUIScreen(int Id)
        {
            var result = _depositorDBContext.GuiScreens.Where(w => w.Id == Id).Include(i => i.GuiTextNavigation.ScreenTitleNavigation).FirstOrDefault();
            return result;
        }

        public  GUIScreen GetGUIScreenByCode(Guid code)
        {
            var result = _depositorDBContext.GuiScreens.Where(w => w.GUIScreenType.Code == code).Include(i => i.GuiTextNavigation.ScreenTitleNavigation).OrderBy(y => y.Id).FirstOrDefault();
            return result;
        }
    }
}
