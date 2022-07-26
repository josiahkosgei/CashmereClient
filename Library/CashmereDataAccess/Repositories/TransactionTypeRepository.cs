﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTypeRepository : RepositoryBase<TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}