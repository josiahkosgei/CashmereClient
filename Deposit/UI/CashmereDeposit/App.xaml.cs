
// Type: CashmereDeposit.App


using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace CashmereDeposit
{
    public partial class App : Application
    {
        public App()
        {
            try
            {

                string appSetting1 = ConfigurationManager.AppSettings["Culture"];
                string appSetting2 = ConfigurationManager.AppSettings["UICulture"];
                if (string.IsNullOrEmpty(appSetting1))
                    return;
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(appSetting1);
                }
                catch (Exception ex)
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(appSetting1);
                }
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(appSetting2);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
