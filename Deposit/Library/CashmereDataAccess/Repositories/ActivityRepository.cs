using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Activity GetByName(string activity)
        {
            var db = _dbContextFactory.CreateDbContext(null);
            using (var dbContext = db)
            {
            var result = dbContext.Activities.Where(x => x.Name.Equals(activity, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return result;

            }
        }
    }
}
