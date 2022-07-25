using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CurrencyListCurrencyRepository : RepositoryBase<CurrencyListCurrency>, ICurrencyListCurrencyRepository
    {
        public CurrencyListCurrencyRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}