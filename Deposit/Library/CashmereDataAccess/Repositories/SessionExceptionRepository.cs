﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class SessionExceptionRepository : RepositoryBase<SessionException>, ISessionExceptionRepository
    {
        public SessionExceptionRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}