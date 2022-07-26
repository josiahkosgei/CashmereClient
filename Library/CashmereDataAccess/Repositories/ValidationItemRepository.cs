using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationItemRepository : RepositoryBase<ValidationItem>, IValidationItemRepository
    {
        public ValidationItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}