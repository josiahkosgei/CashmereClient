using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PrintoutRepository : RepositoryBase<Printout>, IPrintoutRepository
    {
        public PrintoutRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}