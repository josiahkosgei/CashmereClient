using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PasswordHistoryRepository : RepositoryBase<PasswordHistory>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<PasswordHistory> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.PasswordHistories.FirstOrDefault(x => x.Id == Id);
            return await Task.Run<PasswordHistory>(() => result);
        }

        public async Task<IList<PasswordHistory>> GetByUserId(Guid UserId, int historySize)
        {
            var result = depositorDBContext.PasswordHistories.Where(x => x.User == UserId).OrderByDescending(x => x.LogDate).Take(historySize).ToList();
            return await Task.Run<IList<PasswordHistory>>(() => result);
        }

        public async Task<PasswordHistory> GetFirst()
        {
            var result = depositorDBContext.PasswordHistories.FirstOrDefault();
            return await Task.Run<PasswordHistory>(() => result);
        }
    }
}
