using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ValidationItemValueRepository : RepositoryBase<ValidationItemValue>, IValidationItemValueRepository
    {
        public ValidationItemValueRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}