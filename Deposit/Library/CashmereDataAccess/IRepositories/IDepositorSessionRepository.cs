using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDepositorSessionRepository : IAsyncRepository<DepositorSession>
    {
        public DepositorSession GetFirst();
        public IList<DepositorSession> GetCompleted();
    }

}
