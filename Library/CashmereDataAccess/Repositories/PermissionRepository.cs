using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
    {
        public PermissionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Permission> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.Permissions.FirstOrDefault(x => x.Id == Id);
            return await Task.Run<Permission>(() => result);
        }

        public async Task<Permission> GetFirst(ApplicationUser user, Guid ActivityId, bool isAuthenticating = false)
        {
            var result = depositorDBContext.Permissions.FirstOrDefault(x => x.RoleId == user.RoleId && x.ActivityId == ActivityId && (isAuthenticating ? x.StandaloneCanAuthenticate : x.StandaloneAllowed));
            return await Task.Run<Permission>(() => result);
        }
    }
}
