using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PrinterStatusRepository : RepositoryBase<PrinterStatus>, IPrinterStatusRepository
    {
        public PrinterStatusRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}