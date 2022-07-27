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

        public Permission GetById(Guid Id)
        {
            var result = _depositorDBContext.Permissions.FirstOrDefault(x => x.Id == Id);
            return result;
        }

        public Permission GetFirst(ApplicationUser user, Guid ActivityId, bool isAuthenticating = false)
        {
            var result = _depositorDBContext.Permissions.FirstOrDefault(x => x.RoleId == user.RoleId && x.ActivityId == ActivityId && (isAuthenticating ? x.StandaloneCanAuthenticate : x.StandaloneAllowed));
            return result;
        }
    }
}
