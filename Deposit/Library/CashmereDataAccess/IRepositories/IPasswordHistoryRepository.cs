using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPasswordHistoryRepository : IAsyncRepository<PasswordHistory>
    {
        public PasswordHistory GetFirst();
        public PasswordHistory GetByIdAsync(Guid id);
        public IList<PasswordHistory> GetByUserId(Guid id, int historySize);
    }

}
