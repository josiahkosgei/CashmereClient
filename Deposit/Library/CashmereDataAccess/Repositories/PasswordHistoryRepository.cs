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
            var result = _depositorDBContext.PasswordHistories.FirstOrDefault(x => x.Id == Id);
            return result;
        }

        public IList<PasswordHistory> GetByUserId(Guid UserId, int historySize)
        {
            var result = _depositorDBContext.PasswordHistories.Where(x => x.User == UserId).OrderByDescending(x => x.LogDate).Take(historySize).ToList();
            return result;
        }

        public PasswordHistory GetFirst()
        {
            var result = _depositorDBContext.PasswordHistories.FirstOrDefault();
            return result;
        }
    }
}
