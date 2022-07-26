using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionLimitListRepository : RepositoryBase<TransactionLimitList>, ITransactionLimitListRepository
    {
        public TransactionLimitListRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}