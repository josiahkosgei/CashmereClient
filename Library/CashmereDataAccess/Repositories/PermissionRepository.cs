using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Permission> GetByIdAsync(Guid Id)
        {
            return await DbContext.Permissions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Permission> GetFirst(ApplicationUser user, Guid ActivityId, bool isAuthenticating = false)
        {
            return await DbContext.Permissions.FirstOrDefaultAsync(x => x.RoleId == user.RoleId && x.ActivityId == ActivityId && (isAuthenticating ? x.StandaloneCanAuthenticate : x.StandaloneAllowed));
        }
    }
}
