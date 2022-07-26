using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class PrintoutRepository : RepositoryBase<Printout>, IPrintoutRepository
    {
        public PrintoutRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}