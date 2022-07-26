using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class EscrowJamRepository : RepositoryBase<EscrowJam>, IEscrowJamRepository
    {
        public EscrowJamRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<EscrowJam> GetFirst()
        {
            var result = depositorDBContext.EscrowJams.OrderByDescending(x => x.DateDetected).FirstOrDefault();
            return await Task.Run<EscrowJam>(() => result);
        }
    }
}
