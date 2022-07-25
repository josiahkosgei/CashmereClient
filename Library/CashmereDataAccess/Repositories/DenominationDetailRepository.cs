using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DenominationDetailRepository : RepositoryBase<DenominationDetail>, IDenominationDetailRepository
    {
        public DenominationDetailRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}