using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IRoleRepository : IAsyncRepository<Role>
    {
        public Task<Role> GetFirst();
        public Task<Role> GetByNameAsync(string name);
        public Task<Role> GetByIdAsync(Guid id);
    }

}
