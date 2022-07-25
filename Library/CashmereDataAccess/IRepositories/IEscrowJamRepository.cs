using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IEscrowJamRepository : IAsyncRepository<EscrowJam>
    {
        public Task<EscrowJam> GetFirst();
    }
}
