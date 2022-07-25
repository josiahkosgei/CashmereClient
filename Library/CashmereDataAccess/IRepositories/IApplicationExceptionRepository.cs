
using ApplicationException = Cashmere.Library.CashmereDataAccess.Entities.ApplicationException;
namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IApplicationExceptionRepository : IAsyncRepository<ApplicationException>
    {
    }
}