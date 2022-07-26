using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIScreenTextRepository : RepositoryBase<GUIScreenText>, IGuiScreenTextRepository
    {
        public GUIScreenTextRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}