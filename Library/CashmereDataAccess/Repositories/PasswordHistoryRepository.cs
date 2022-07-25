using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PasswordHistoryRepository : RepositoryBase<PasswordHistory>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<PasswordHistory> GetByIdAsync(Guid Id)
        {
            return await DbContext.PasswordHistories.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<IList<PasswordHistory>> GetByUserId(Guid UserId, int historySize)
        {
            return await DbContext.PasswordHistories.Where(x => x.User == UserId).OrderByDescending(x => x.LogDate).Take(historySize).ToListAsync();
        }

        public async Task<PasswordHistory> GetFirst()
        {
            return await DbContext.PasswordHistories.FirstOrDefaultAsync();
        }
    }
}
