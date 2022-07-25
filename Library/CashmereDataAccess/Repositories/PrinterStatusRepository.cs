using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PrinterStatusRepository : RepositoryBase<PrinterStatus>, IPrinterStatusRepository
    {
        public PrinterStatusRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}