using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITTransactionRepository : RepositoryBase<CITTransaction>, ICITTransactionRepository
    {
        public CITTransactionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}