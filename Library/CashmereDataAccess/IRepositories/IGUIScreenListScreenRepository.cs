using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IGuiScreenListScreenRepository : IAsyncRepository<GuiScreenListScreen>
    {
        public Task<GuiScreenListScreen> GetByGUIScreenId(int Id);
    }

}
