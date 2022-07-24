using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPermissionRepository : IAsyncRepository<Permission>
    {
        public Task<Permission> GetFirst(ApplicationUser user,Guid ActivityId,  bool isAuthenticating = false);
        public Task<Permission> GetByIdAsync(Guid id);
    }

}
