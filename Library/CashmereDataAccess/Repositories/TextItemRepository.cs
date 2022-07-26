﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TextItemRepository : RepositoryBase<TextItem>, ITextItemRepository
    {
        public TextItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}