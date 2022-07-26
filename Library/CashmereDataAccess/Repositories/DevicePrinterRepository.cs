﻿using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cashmere.Library.CashmereDataAccess.Repositories
{
    public class DevicePrinterRepository : RepositoryBase<DevicePrinter>, IDevicePrinterRepository
    {
        public DevicePrinterRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}