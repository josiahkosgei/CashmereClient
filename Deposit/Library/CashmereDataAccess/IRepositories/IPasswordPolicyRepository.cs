using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IPasswordPolicyRepository : IAsyncRepository<PasswordPolicy>
    {
        public PasswordPolicy GetFirst();
        public PasswordPolicy GetByIdAsync(Guid id);
    }

}
