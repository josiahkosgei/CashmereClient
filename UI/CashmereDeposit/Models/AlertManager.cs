using Cashmere.Library.Standard.Logging;
using CashmereDeposit.Utils.AlertClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cashmere.Library.CashmereDataAccess;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.Models
{
    public class AlertManager
    {
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

        public void InitialiseAlertManager()
        {
            using (DepositorDBContext depositorDbContext = new DepositorDBContext())
            {
                AllowedMessages = depositorDbContext.AlertMessageTypes.Where(x => x.Enabled == true).ToList();
                DepositorCommunicationService = DepositorCommunicationService.NewDepositorCommunicationService(commserv_uri, appID, AppKey, appName);
            }
        }
    }
}
