using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeListTransactionTypeListItemRepository : RepositoryBase<TransactionTypeListTransactionTypeListItem>, ITransactionTypeListTransactionTypeListItemRepository
    {
        public TransactionTypeListTransactionTypeListItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}