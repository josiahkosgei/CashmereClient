using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Role> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.Roles.FirstOrDefault(x => x.Id == Id);
            return await Task.Run<Role>(() => result);
        }
        public async Task<Role> GetByNameAsync(string name)
        {
            var result = depositorDBContext.Roles.FirstOrDefault(x => x.Name == name);
            return await Task.Run<Role>(() => result);
        }

        public async Task<Role> GetFirst()
        {
            var result = depositorDBContext.Roles.FirstOrDefault();
            return await Task.Run<Role>(() => result);
        }
    }
}
