using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextTranslationRepository : RepositoryBase<TextTranslation>, ITextTranslationRepository
    {
        public TextTranslationRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}