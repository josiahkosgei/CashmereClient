using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface ICurrencyRepository : IAsyncRepository<Currency>
    {
        public Task<Currency> GetByCode(string code);
    }
}