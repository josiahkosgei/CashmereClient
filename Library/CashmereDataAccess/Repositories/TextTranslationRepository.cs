﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextTranslationRepository : RepositoryBase<TextTranslation>, ITextTranslationRepository
    {
        public TextTranslationRepository(DepositorDBContext dbContext) : base(dbContext)
        {
        }
    }
}