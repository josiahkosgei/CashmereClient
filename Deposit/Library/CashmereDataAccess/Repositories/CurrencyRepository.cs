using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;


namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  Currency GetByCode(string code)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Currencies.Where(x => x.Code == code).FirstOrDefault();
            return result;

            }
        }
    }
}