using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class CITRepository : RepositoryBase<CIT>, ICITRepository
    {
        public CITRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<CIT> GetByIdAsync(Guid Id)
        {
            var result = depositorDBContext.CITs.Where(y => y.Id == Id).FirstOrDefault();
            return await Task.Run<CIT>(() => result);
        }
        public async Task<CIT> GetByDeviceId(Guid DeviceId)
        {
            var result = depositorDBContext.CITs.Where(y => y.DeviceId == DeviceId).OrderByDescending(x => x.CITDate).FirstOrDefault();
            return await Task.Run<CIT>(() => result);
        }

        public async Task<IList<CIT>> GetInCompleteByDeviceId(Guid lastCITId, Guid DeviceId)
        {
            var result = depositorDBContext.CITs.Where(y => y.DeviceId == DeviceId && y.Id == lastCITId && y.Complete == false).ToList();
            return await Task.Run<IList<CIT>>(() => result);
        }

        public async Task<CIT> LastCIT(Guid lastCITId)
        {
            var result = depositorDBContext.CITs.Where(x => x.Id == lastCITId).Include(y => y.StartUserNavigation).Include(z => z.AuthUserNavigation).FirstOrDefault();
            return await Task.Run<CIT>(() => result);
        }

        public async Task<bool> ValidateSealNumberAsync(string newSealNumber)
        {
            var result = depositorDBContext.CITs.Any(x => x.SealNumber == newSealNumber);
            return await Task.Run<bool>(() => result);
        }

        public IQueryable<CIT> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate)
        {
            return depositorDBContext.CITs.Where(t => t.FromDate >= txQueryStartDate && t.ToDate < txQueryEndDate).OrderByDescending(t => t.ToDate).AsQueryable();
        }
    }
}
