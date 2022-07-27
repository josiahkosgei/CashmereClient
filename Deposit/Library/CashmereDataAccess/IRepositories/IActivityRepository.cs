using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IActivityRepository : IAsyncRepository<Activity>
    {
        public Activity GetByName(string Name);
    }

}
