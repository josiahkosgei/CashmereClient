using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenListRepository : RepositoryBase<GUIScreenList>, IGuiScreenListRepository
    {
        public GUIScreenListRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}