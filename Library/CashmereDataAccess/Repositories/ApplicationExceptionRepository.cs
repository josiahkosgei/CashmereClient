using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.Extensions.Configuration;
using ApplicationException = Cashmere.Library.CashmereDataAccess.Entities.ApplicationException;
namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationExceptionRepository : RepositoryBase<ApplicationException>, IApplicationExceptionRepository
    {
        public ApplicationExceptionRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}