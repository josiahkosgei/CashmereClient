using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPasswordPolicyRepository : IAsyncRepository<PasswordPolicy>
    {
        public Task<PasswordPolicy> GetFirst();
        public Task<PasswordPolicy> GetByIdAsync(Guid id);
    }

}
