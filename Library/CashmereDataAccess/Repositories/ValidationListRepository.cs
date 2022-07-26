using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationListRepository : RepositoryBase<ValidationList>, IValidationListRepository
    {
        public ValidationListRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}