using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IDepositorSessionRepository : IAsyncRepository<DepositorSession>
    {
        public Task<DepositorSession> GetFirst();
         public Task<IList<DepositorSession>> GetCompleted();
    }

}
