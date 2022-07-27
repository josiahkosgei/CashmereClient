using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PasswordPolicyRepository : RepositoryBase<PasswordPolicy>, IPasswordPolicyRepository
    {
        public PasswordPolicyRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public PasswordPolicy GetById(Guid Id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.PasswordPolicies.FirstOrDefault(x => x.Id == Id);
                return result;

            }
        }
        public PasswordPolicy GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.PasswordPolicies.FirstOrDefault();
                return result;

            }
        }
    }
}
