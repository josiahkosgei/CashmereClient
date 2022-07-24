using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class EscrowJamRepository : RepositoryBase<EscrowJam>, IEscrowJamRepository
    {
        public EscrowJamRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<EscrowJam> GetFirst()
        {
            return await DbContext.EscrowJams.OrderByDescending(x => x.DateDetected).FirstOrDefaultAsync();
        }
    }
}
