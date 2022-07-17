
//.StartupImageViewModel




using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using DeviceManager;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public class StartupImageViewModel : Conductor<Screen>, IShell
  {
    public string CashmereGUIVersion
    {
        get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    }

    public string DeviceManagerVersion
    {
        get { return Assembly.GetAssembly(typeof(DeviceMessageBase)).GetName().Version.ToString(); }
    }

    public string CashmereUtilVersion
    {
        get { return Assembly.GetAssembly(typeof(XMLSerialization)).GetName().Version.ToString(); }
    }

    public string Copyright { get; }

    public string CompanyName { get; }

    public string Trademark { get; }

    public string Description { get; }

    public string ApplicationTitle { get; }

    public string Credits
    {
        get
        {
            return
                "\r\nlog4net by The Apache Software Foundation with license http://logging.apache.org/log4net/license.html \r\n\r\nNLog by Jarek Kowalski,Kim Christensen,Julian Verdurmen with license https://github.com/NLog/NLog/blob/master/LICENSE.txt\r\n\r\nEntityFramework by Microsoft with license https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm  \r\n\r\nCaliburn.Micro by Rob Eisenberg, Marco Amendola, Chin Bae, Ryan Cromwell, Nigel Sampson, Thomas Ibel, Matt Hidinger with license https://raw.githubusercontent.com/Caliburn-Micro/Caliburn.Micro/master/License.txt \r\n\r\nExtended.Wpf.Toolkit by Xceed with license https://github.com/xceedsoftware/wpftoolkit/blob/master/license.md \r\n\r\nIcons designed by www.flaticon.com";
        }
    }

    public StartupImageViewModel()
    {
      AssemblyDescriptionAttribute descriptionAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
      if (descriptionAttribute != null)
        Description = descriptionAttribute.Description;
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
      Trademark = versionInfo.LegalTrademarks;
      CompanyName = versionInfo.CompanyName;
      Copyright = $"Copyright © 2018 - {DateTime.Now:yyyy} Maniwa Technologies Ltd. All rights reserved.";
      ApplicationTitle = versionInfo.ProductName;
    }
  }
}
