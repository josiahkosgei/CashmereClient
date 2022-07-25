using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPasswordHistoryRepository : IAsyncRepository<PasswordHistory>
    {
        public Task<PasswordHistory> GetFirst();
        public Task<PasswordHistory> GetByIdAsync(Guid id);
        public Task<IList<PasswordHistory>> GetByUserId(Guid id, int historySize);
    }

}
