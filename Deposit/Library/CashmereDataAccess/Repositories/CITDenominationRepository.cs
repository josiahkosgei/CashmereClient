using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITDenominationRepository : RepositoryBase<CITDenomination>, ICITDenominationRepository
    {
        public CITDenominationRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}