using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITTransactionRepository : RepositoryBase<CITTransaction>, ICITTransactionRepository
    {
        public CITTransactionRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}