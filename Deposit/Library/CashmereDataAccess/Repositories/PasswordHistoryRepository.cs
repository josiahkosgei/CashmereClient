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

        public PasswordHistory GetById(Guid Id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.PasswordHistories.FirstOrDefault(x => x.Id == Id);
                return result;

            }
        }

        public IList<PasswordHistory> GetByUserId(Guid UserId, int historySize)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.PasswordHistories.Where(x => x.User == UserId).OrderByDescending(x => x.LogDate).Take(historySize).ToList();
                return result;

            }
        }

        public PasswordHistory GetFirst()
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.PasswordHistories.FirstOrDefault();
                return result;

            }
        }
    }
}
