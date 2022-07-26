﻿using Cashmere.Library.CashmereDataAccess.IRepositories;
using Cashmere.Library.CashmereDataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class GuiScreenListScreenRepository : RepositoryBase<GuiScreenListScreen>, IGuiScreenListScreenRepository
    {
        public GuiScreenListScreenRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<GuiScreenListScreen> GetByGUIScreenId(int Id)
        {
            var result = depositorDBContext.GuiScreenListScreens.Where(w => w.GUIScreenNavigation.Id == Id).FirstOrDefault();
            return await Task.Run<GuiScreenListScreen>(() => result);
        }
    }
}
