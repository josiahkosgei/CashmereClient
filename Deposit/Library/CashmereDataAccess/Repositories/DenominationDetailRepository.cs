using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DenominationDetailRepository : RepositoryBase<DenominationDetail>, IDenominationDetailRepository
    {
        public DenominationDetailRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}