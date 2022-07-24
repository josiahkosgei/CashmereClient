﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashmere.Library.CashmereDataAccess.Extensions
{
    public class ResilientTransaction
    {
        private DepositorDBContext _context;
        private ResilientTransaction(DepositorDBContext context) =>
            _context = context ?? throw new ArgumentNullException(nameof(context));

        public static ResilientTransaction New(DepositorDBContext context) =>
            new ResilientTransaction(context);

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                await action();
                await transaction.CommitAsync();
            });
        }
    }
}
