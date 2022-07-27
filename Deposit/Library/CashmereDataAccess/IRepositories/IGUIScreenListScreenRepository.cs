using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IGuiScreenListScreenRepository : IAsyncRepository<GuiScreenListScreen>
    {
        public GuiScreenListScreen GetByGUIScreenId(int Id);
    }

}
