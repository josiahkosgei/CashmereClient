using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPermissionRepository : IAsyncRepository<Permission>
    {
        public Permission GetFirst(ApplicationUser user, Guid ActivityId, bool isAuthenticating = false);
        public Permission GetByIdAsync(Guid id);
    }

}
