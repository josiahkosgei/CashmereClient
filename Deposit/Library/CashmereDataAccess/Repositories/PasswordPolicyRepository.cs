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
            var result = _depositorDBContext.PasswordPolicies.FirstOrDefault(x => x.Id == Id);
            return result;
        }
        public PasswordPolicy GetFirst()
        {
            var result = _depositorDBContext.PasswordPolicies.FirstOrDefault();
            return result;
        }
    }
}
