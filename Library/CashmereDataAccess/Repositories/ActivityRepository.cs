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

        public async Task<Activity> GetByName(string activity)
        {
            var result = depositorDBContext.Activities.Where(x => x.Name.Equals(activity, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return await Task.Run<Activity>(() => result);
        }
    }
}
