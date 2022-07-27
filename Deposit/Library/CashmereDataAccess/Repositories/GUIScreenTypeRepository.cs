using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenTypeRepository : RepositoryBase<GUIScreenType>, IGuiScreenTypeRepository
    {
        public GUIScreenTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}