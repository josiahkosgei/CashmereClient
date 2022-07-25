using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PasswordPolicyRepository : RepositoryBase<PasswordPolicy>, IPasswordPolicyRepository
    {
        public PasswordPolicyRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PasswordPolicy> GetByIdAsync(Guid Id)
        {
            return await DbContext.PasswordPolicies.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<PasswordPolicy> GetFirst()
        {
            return await DbContext.PasswordPolicies.FirstOrDefaultAsync();
        }
    }
}
