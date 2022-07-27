using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationTypeRepository : RepositoryBase<ValidationType>, IValidationTypeRepository
    {
        public ValidationTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}