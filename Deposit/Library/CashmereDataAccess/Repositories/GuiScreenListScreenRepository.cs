using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GuiScreenListScreenRepository : RepositoryBase<GuiScreenListScreen>, IGuiScreenListScreenRepository
    {
        public GuiScreenListScreenRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public GuiScreenListScreen GetByGUIScreenId(int Id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.GuiScreenListScreens.Where(w => w.GUIScreenNavigation.Id == Id).FirstOrDefault();
                return result;

            }
        }
    }
}
