using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITDenominationRepository : RepositoryBase<CITDenomination>, ICITDenominationRepository
    {
        public CITDenominationRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}