using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CurrencyListCurrencyRepository : RepositoryBase<CurrencyListCurrency>, ICurrencyListCurrencyRepository
    {
        public CurrencyListCurrencyRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}