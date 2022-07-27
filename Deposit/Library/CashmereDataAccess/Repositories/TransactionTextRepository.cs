﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class TransactionTextRepository : RepositoryBase<TransactionText>, ITransactionTextRepository
    {
        public TransactionTextRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public TransactionText GetByTxItemIdAsync(int id)
        {
            return _depositorDBContext.TransactionTexts.Where(x=>x.TxItem ==id).FirstOrDefault();
        }
    }
}