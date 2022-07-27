using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IApplicationUserRepository : IAsyncRepository<ApplicationUser>
    {
        public ApplicationUser GetFirst();
        public ApplicationUser GetByIdAsync(Guid id);
    }
}
