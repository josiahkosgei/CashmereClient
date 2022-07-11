﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.Extensions;
using Cashmere.Library.CashmereDataAccess.StoredProcs.Models;

namespace Cashmere.Library.CashmereDataAccess.StoredProcs
{
    public partial interface IDepositorContextProcedures
    {
        Task<List<GetCITDenominationByDatesResult>> GetCITDenominationByDatesAsync(DateTime? startDate, DateTime? endDate, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<GetDestinationEmailsByAlertTypeResult>> GetDestinationEmailsByAlertTypeAsync(int? alertMessageTypeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<GetDeviceConfigByUserGroupResult>> GetDeviceConfigByUserGroupAsync(int? ConfigGroup, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<ApplicationUser>> GetDeviceUsersByDeviceAsync(int? Device_UserGroup, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<HashTransactionResult>> HashTransactionAsync(string TxString, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
