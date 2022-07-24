using Cashmere.Library.CashmereDataAccess.Entities;

namespace Cashmere.Library.CashmereDataAccess.IRepositories
{
    public interface IActivityRepository: IAsyncRepository<Activity>
    {
        public Task<Activity> GetByName(string Name);
    }
 
}
