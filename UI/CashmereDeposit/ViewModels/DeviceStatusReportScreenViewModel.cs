
// Type: CashmereDeposit.ViewModels.DeviceStatusReportScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Cashmere.Library.Standard.Statuses;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using CashmereDeposit.Utils;

namespace CashmereDeposit.ViewModels
{
  public class DeviceStatusReportScreenViewModel : DepositorScreenViewModelBase
  {
    private DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send, Application.Current.Dispatcher);

    public string MachineName
    {
        get { return Environment.MachineName; }
    }

    public string CashmereGUIVersion
    {
        get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
    }

    public string DeviceManagerVersion { get; set; }

    public string CashmereUtilVersion
    {
        get { return Assembly.GetAssembly(typeof(XMLSerialization)).GetName().Version.ToString(); }
    }

    public string ControllerStatus { get; set; }

    public string TransactionStatus { get; set; }

    public string BAStatus { get; set; }

    public string BAType { get; set; }

    public string BagStatus { get; set; }

    public string BagNumber { get; set; }

    public string BagPercentFull { get; set; }

    public string BagNoteLevel { get; set; }

    public string BagNoteCapacity { get; set; }

    public string SensorBag { get; set; }

    public string SensorDoor { get; set; }

    public string EscrowType { get; set; }

    public string EscrowStatus { get; set; }

    public string EscrowPosition { get; set; }

    public string ApplicationStatus { get; set; }

    public string ApplicationState { get; set; }

    public string DeviceState { get; set; }

    public DeviceStatusReportScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      Screen callingObject)
      : base(screenTitle, applicationViewModel, callingObject)
    {
      DeviceManagerVersion = applicationViewModel.DeviceManager.DeviceManagerVersion.ToString();
      InitialiseDeviceReport(applicationViewModel);
      dispTimer.Interval = TimeSpan.FromSeconds(1.0);
      dispTimer.Tick += new EventHandler(dispTimer_Tick);
      dispTimer.IsEnabled = true;
    }

    private void dispTimer_Tick(object sender, EventArgs e)
    {
      InitialiseDeviceReport(ApplicationViewModel);
      NotifyOfPropertyChange("ControllerStatus");
      NotifyOfPropertyChange("TransactionStatus");
      NotifyOfPropertyChange("BAStatus");
      NotifyOfPropertyChange("BAType");
      NotifyOfPropertyChange("BagStatus");
      NotifyOfPropertyChange("BagNumber");
      NotifyOfPropertyChange("BagPercentFull");
      NotifyOfPropertyChange("BagNoteLevel");
      NotifyOfPropertyChange("BagNoteCapacity");
      NotifyOfPropertyChange("SensorBag");
      NotifyOfPropertyChange("SensorDoor");
      NotifyOfPropertyChange("EscrowType");
      NotifyOfPropertyChange("EscrowStatus");
      NotifyOfPropertyChange("EscrowPosition");
      NotifyOfPropertyChange("ApplicationStatus");
      NotifyOfPropertyChange("DeviceState");
    }

    private void InitialiseDeviceReport(ApplicationViewModel applicationViewModel)
    {
      ControllerStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.ControllerState.ToString()?.ToUpper();
      TransactionStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Transaction?.Status.ToString()?.ToUpper();
      BAStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.NoteAcceptor?.Status.ToString()?.ToUpper();
      BAType = applicationViewModel?.ApplicationStatus?.ControllerStatus?.NoteAcceptor?.Type.ToString()?.ToUpper();
      BagStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.BagState.ToString()?.ToUpper();
      BagNumber = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.BagNumber?.ToString()?.ToUpper();
      int num;
      string str1;
      if (applicationViewModel == null)
      {
        str1 = null;
      }
      else
      {
        CashmereDeviceStatus applicationStatus = applicationViewModel.ApplicationStatus;
        if (applicationStatus == null)
        {
          str1 = null;
        }
        else
        {
          ControllerStatus controllerStatus = applicationStatus.ControllerStatus;
          if (controllerStatus == null)
          {
            str1 = null;
          }
          else
          {
            DeviceBag bag = controllerStatus.Bag;
            if (bag == null)
            {
              str1 = null;
            }
            else
            {
              num = bag.PercentFull;
              str1 = num.ToString()?.ToUpper();
            }
          }
        }
      }
      BagPercentFull = str1;
      string str2;
      if (applicationViewModel == null)
      {
        str2 = null;
      }
      else
      {
        CashmereDeviceStatus applicationStatus = applicationViewModel.ApplicationStatus;
        if (applicationStatus == null)
        {
          str2 = null;
        }
        else
        {
          ControllerStatus controllerStatus = applicationStatus.ControllerStatus;
          if (controllerStatus == null)
          {
            str2 = null;
          }
          else
          {
            DeviceBag bag = controllerStatus.Bag;
            if (bag == null)
            {
              str2 = null;
            }
            else
            {
              num = bag.NoteLevel;
              str2 = num.ToString()?.ToUpper();
            }
          }
        }
      }
      BagNoteLevel = str2;
      BagNoteCapacity = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.NoteCapacity.ToString()?.ToUpper();
      SensorBag = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Sensor?.Bag.ToString()?.ToUpper();
      SensorDoor = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Sensor?.Door.ToString()?.ToUpper();
      EscrowType = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Type.ToString()?.ToUpper();
      EscrowStatus = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Status.ToString()?.ToUpper();
      EscrowPosition = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Escrow?.Position.ToString()?.ToUpper();
      ApplicationStatus = applicationViewModel?.CurrentApplicationState.ToString()?.ToUpper();
      ApplicationState = applicationViewModel?.ApplicationStatus?.CashmereDeviceState.ToString()?.ToUpper();
      DeviceState = applicationViewModel?.DeviceManager?.CurrentState.ToString()?.ToUpper();
    }
  }
}
