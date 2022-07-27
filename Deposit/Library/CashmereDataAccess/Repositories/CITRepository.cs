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

        public CIT GetByIdAsync(Guid Id)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.CITs.Where(y => y.Id == Id).FirstOrDefault();
                return result;

            }
        }
        public CIT GetByDeviceId(Guid DeviceId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.CITs.Where(y => y.DeviceId == DeviceId).OrderByDescending(x => x.CITDate).FirstOrDefault();
                return result;

            }
        }

        public IList<CIT> GetInCompleteByDeviceId(Guid lastCITId, Guid DeviceId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.CITs.Where(y => y.DeviceId == DeviceId && y.Id == lastCITId && y.Complete == false).ToList();
                return result;

            }
        }

        public CIT LastCIT(Guid lastCITId)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.CITs.Where(x => x.Id == lastCITId).Include(y => y.StartUserNavigation).Include(z => z.AuthUserNavigation).FirstOrDefault();
                return result;

            }
        }

        public bool ValidateSealNumberAsync(string newSealNumber)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                var result = dbContext.CITs.Any(x => x.SealNumber == newSealNumber);
                return result;

            }
        }

        public IQueryable<CIT> GetByDateRange(DateTime txQueryStartDate, DateTime txQueryEndDate)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
                return dbContext.CITs.Where(t => t.FromDate >= txQueryStartDate && t.ToDate < txQueryEndDate).OrderByDescending(t => t.ToDate).AsQueryable();

            }
        }
    }
}
