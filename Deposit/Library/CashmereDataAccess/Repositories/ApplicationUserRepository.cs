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

        public ApplicationUser GetById(Guid Id)
        {

            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
                return result;

            }
        }

        public ApplicationUser GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.ApplicationUsers.FirstOrDefault();
                return result;

            }
        }
    }
}
