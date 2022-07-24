using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IGUIScreenRepository : IAsyncRepository<GUIScreen>
    {
        public Task<GUIScreen> GetCurrentGUIScreen(int Id);
        public Task<GUIScreen> GetGUIScreenByCode(Guid code);
    }

}
