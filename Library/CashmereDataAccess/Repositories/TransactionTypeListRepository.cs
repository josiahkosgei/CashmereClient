using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListRepository : RepositoryBase<TransactionTypeList>, ITransactionTypeListRepository
    {
        public TransactionTypeListRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}