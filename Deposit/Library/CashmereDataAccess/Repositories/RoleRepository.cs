using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public  Role GetByIdAsync(Guid Id)
        {
            var result = _depositorDBContext.Roles.FirstOrDefault(x => x.Id == Id);
            return result;
        }
        public  Role GetByNameAsync(string name)
        {
            var result = _depositorDBContext.Roles.FirstOrDefault(x => x.Name == name);
            return result;
        }

        public  Role GetFirst()
        {
            var result = _depositorDBContext.Roles.FirstOrDefault();
            return result;
        }
    }
}
