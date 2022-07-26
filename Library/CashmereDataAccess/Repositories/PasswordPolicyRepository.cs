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

        public async Task<PasswordPolicy> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.PasswordPolicies.FirstOrDefault(x => x.Id == Id);
            return await Task.Run<PasswordPolicy>(() => result);
        }
        public async Task<PasswordPolicy> GetFirst()
        {
            var result = depositorDBContext.PasswordPolicies.FirstOrDefault();
            return await Task.Run<PasswordPolicy>(() => result);
        }
    }
}
