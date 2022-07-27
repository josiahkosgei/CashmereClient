using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IRoleRepository : IAsyncRepository<Role>
    {
        public Role GetFirst();
        public Role GetByNameAsync(string name);
        public Role GetByIdAsync(Guid id);
    }

}
