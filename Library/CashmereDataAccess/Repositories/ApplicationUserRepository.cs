using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid Id)
        {
            return await DbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ApplicationUser> GetFirst()
        {
            return await DbContext.ApplicationUsers.FirstOrDefaultAsync();
        }
    }
}
