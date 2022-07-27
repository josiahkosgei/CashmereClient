using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IGUIScreenRepository : IAsyncRepository<GUIScreen>
    {
        public GUIScreen GetCurrentGUIScreen(int Id);
        public GUIScreen GetGUIScreenByCode(Guid code);
    }

}
