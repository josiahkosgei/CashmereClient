using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Activity> GetByName(string activity)
        {
            return await DbContext.Activities.Where(x => x.Name.Equals(activity, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefaultAsync();
        }
    }
}
