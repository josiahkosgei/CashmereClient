using Cashmere.Library.Standard.Logging;
using CashmereDeposit.Utils.AlertClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess.IRepositories;

namespace CashmereDeposit.Models
{
    public class AlertManager
    {
        private readonly IAlertMessageTypeRepository _alertMessageTypeRepository;
        private readonly IAlertEventRepository _alertEventRepository;
        public static ICashmereLogger Log;
        private static string commserv_uri;
        private static Guid appID;
        private static byte[] AppKey;
        private static string appName;

        private IList<AlertMessageType> AllowedMessages { get; set; }

        private DepositorCommunicationService DepositorCommunicationService { get; set; }

        public AlertManager(
          ICashmereLogger logger,
          string CommServURI,
          Guid AppID,
          byte[] appKey,
          string AppName)
        {
            _alertMessageTypeRepository = IoC.Get<IAlertMessageTypeRepository>();
            _alertEventRepository = IoC.Get<IAlertEventRepository>();
            Log = logger;
            appID = AppID;
            AppKey = appKey;
            appName = AppName;
            commserv_uri = CommServURI;
            InitialiseAlertManager();
        }

        public void SendAlert(AlertBase alert)
        {
            try
            {
                alert?.SendAlert();
            }
            catch (Exception ex)
            {
            }
        }

        public async void InitialiseAlertManager()
        {
            AllowedMessages =  _alertMessageTypeRepository.GetEnabled();
            DepositorCommunicationService = DepositorCommunicationService.NewDepositorCommunicationService(commserv_uri, appID, AppKey, appName);

        }
    }
}
