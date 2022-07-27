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

        public  ApplicationUser GetById(Guid Id)
        {
            var result = _depositorDBContext.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            return result;
        }

        public ApplicationUser GetFirst()
        {
            var result = _depositorDBContext.ApplicationUsers.FirstOrDefault();
            return result;
        }
    }
}
