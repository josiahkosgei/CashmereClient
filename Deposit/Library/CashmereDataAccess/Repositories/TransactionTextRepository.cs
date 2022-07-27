using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTextRepository : RepositoryBase<TransactionText>, ITransactionTextRepository
    {
        public TransactionTextRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public TransactionText GetByTxItemIdAsync(int id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                return dbContext.TransactionTexts.Where(x => x.TxItem == id).FirstOrDefault();

            }
        }
    }
}