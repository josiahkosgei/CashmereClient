using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IUserLockRepository : IAsyncRepository<UserLock>
    {
        public Task<UserLock> GetFirst();
    } 
}
