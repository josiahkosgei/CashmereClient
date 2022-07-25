using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CurrencyRepository : RepositoryBase<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Currency> GetByCode(string code)
        {
            return await DbContext.Currencies.Where(x => x.Code == code).FirstOrDefaultAsync();
        }
    }
}