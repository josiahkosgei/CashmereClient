using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionLimitListItemRepository : RepositoryBase<TransactionLimitListItem>, ITransactionLimitListItemRepository
    {
        public TransactionLimitListItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}