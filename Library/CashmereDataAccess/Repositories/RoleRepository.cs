using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Role> GetByIdAsync(Guid Id)
        {
            return await DbContext.Roles.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<Role> GetByNameAsync(string name)
        {
            return await DbContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Role> GetFirst()
        {
            return await DbContext.Roles.FirstOrDefaultAsync();
        }
    }
}
