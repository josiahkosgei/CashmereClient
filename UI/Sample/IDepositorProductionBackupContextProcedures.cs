﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sample;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    public partial interface IDepositorProductionBackupContextProcedures
    {
        Task<List<GetCITDenominationByDatesResult>> GetCITDenominationByDatesAsync(DateTime? startDate, DateTime? endDate, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<GetDestinationEmailsByAlertTypeResult>> GetDestinationEmailsByAlertTypeAsync(int? alertMessageTypeID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<GetDeviceConfigByUserGroupResult>> GetDeviceConfigByUserGroupAsync(int? ConfigGroup, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<GetDeviceUsersByDeviceResult>> GetDeviceUsersByDeviceAsync(int? Device_UserGroup, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<HashTransactionResult>> HashTransactionAsync(string TxString, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}