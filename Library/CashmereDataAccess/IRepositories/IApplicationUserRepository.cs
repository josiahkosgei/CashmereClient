using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IApplicationUserRepository : IAsyncRepository<ApplicationUser>
    {
        public Task<ApplicationUser> GetFirst();
        public Task<ApplicationUser> GetByIdAsync(Guid id);
    } 

}
