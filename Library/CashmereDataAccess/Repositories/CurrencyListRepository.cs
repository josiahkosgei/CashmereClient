using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CurrencyListRepository : RepositoryBase<CurrencyList>, ICurrencyListRepository
    {
        public CurrencyListRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}