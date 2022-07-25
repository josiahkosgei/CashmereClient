using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITRepository : RepositoryBase<CIT>, ICITRepository
    {
        public CITRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<CIT> GetByIdAsync(Guid Id)
        {
            return await DbContext.CITs.Where(y => y.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<CIT> GetByDeviceId(Guid DeviceId)
        {
            return await DbContext.CITs.Where(y => y.DeviceId == DeviceId).OrderByDescending(x => x.CITDate).FirstOrDefaultAsync();
        }

        public async Task<IList<CIT>> GetInCompleteByDeviceId(Guid lastCITId, Guid DeviceId)
        {
            return await DbContext.CITs.Where(y => y.DeviceId == DeviceId && y.Id == lastCITId && y.Complete == false).ToListAsync();
        }

        public async Task<CIT> LastCIT(Guid lastCITId)
        {
            return await DbContext.CITs.Where(x => x.Id == lastCITId).Include(y => y.StartUserNavigation).Include(z => z.AuthUserNavigation).FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateSealNumberAsync(string newSealNumber)
        {
            return await DbContext.CITs.AnyAsync(x => x.SealNumber == newSealNumber);
        }

        public IQueryable<CIT> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate)
        {
              return DbContext.CITs.Where(t => t.FromDate >= txQueryStartDate && t.ToDate < txQueryEndDate).OrderByDescending(t => t.ToDate).AsQueryable();
        }
    }
}
