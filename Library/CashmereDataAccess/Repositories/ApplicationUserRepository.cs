using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            return await Task.Run<ApplicationUser>(() => result);
        }

        public async Task<ApplicationUser> GetFirst()
        {
            var result = depositorDBContext.ApplicationUsers.FirstOrDefault();
            return await Task.Run<ApplicationUser>(() => result);
        }
    }
}
