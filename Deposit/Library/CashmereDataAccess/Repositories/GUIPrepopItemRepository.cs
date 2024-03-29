﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GUIPrepopItemRepository : RepositoryBase<GUIPrepopItem>, IGUIPrepopItemRepository
    {
        public GUIPrepopItemRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}