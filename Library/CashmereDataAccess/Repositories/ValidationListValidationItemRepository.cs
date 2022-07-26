using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationListValidationItemRepository : RepositoryBase<ValidationListValidationItem>, IValidationListValidationItemRepository
    {
        public ValidationListValidationItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}