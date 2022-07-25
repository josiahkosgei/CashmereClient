using Cashmere.Library.CashmereDataAccess.IRepositories;
using ApplicationException = Cashmere.Library.CashmereDataAccess.Entities.ApplicationException;
namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ApplicationExceptionRepository : RepositoryBase<ApplicationException>, IApplicationExceptionRepository
    {
        public ApplicationExceptionRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}