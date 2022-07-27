using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionExceptionRepository : RepositoryBase<TransactionException>, ITransactionExceptionRepository
    {
        public TransactionExceptionRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}