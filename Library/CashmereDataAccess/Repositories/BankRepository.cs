using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class BankRepository : RepositoryBase<Bank>, IBankRepository
    {
        public BankRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}