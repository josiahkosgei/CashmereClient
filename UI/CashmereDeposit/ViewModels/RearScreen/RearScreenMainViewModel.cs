using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using CashmereDeposit.Models;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace CashmereDeposit.ViewModels.RearScreen
{
  public class RearScreenMainViewModel : Screen
  {
    private DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send, Application.Current.Dispatcher);
    private string adminButtonCaption;

    public RearScreenMainViewModel(
        Conductor<Screen> conductor,
      ApplicationViewModel applicationViewModel)
    {
      Conductor = conductor;
      ApplicationViewModel = applicationViewModel;
      ApplicationViewModel.DeviceStatusChangedEvent += new EventHandler<DeviceStatusChangedEventArgs>(ApplicationViewModel_DeviceStatusChangedEvent);
      Activated += new EventHandler<ActivationEventArgs>(RearScreenMainViewModel_Activated);
      DeviceManagerVersion = applicationViewModel.DeviceManager.DeviceManagerVersion.ToString();
      InitialiseDeviceReport(applicationViewModel);
      AdminButtonCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("RearScreenMainViewModel._ctor", "sys_RearScren_AdminButtonCaption", "Admin Login");
      dispTimer.Interval = TimeSpan.FromSeconds(1.0);
      dispTimer.Tick += new EventHandler(dispTimer_Tick);
      dispTimer.IsEnabled = true;
    }

    public string AdminButtonCaption
    {
      get => adminButtonCaption;
      set
      {
        adminButtonCaption = value;
        NotifyOfPropertyChange<string>((System.Linq.Expressions.Expression<Func<string>>) (() => AdminButtonCaption));
      }
    }

    public string MachineName => Environment.MachineName;

    public string CashmereGUIVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();

    public string DeviceManagerVersion { get; set; }

    public string CashmereUtilVersion => Assembly.GetAssembly(typeof (XMLSerialization)).GetName().Version.ToString();

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
      BagPercentFull = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.PercentFull.ToString()?.ToUpper();
      BagNoteLevel = applicationViewModel?.ApplicationStatus?.ControllerStatus?.Bag?.NoteLevel.ToString()?.ToUpper();
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

    private void RearScreenMainViewModel_Activated(object sender, ActivationEventArgs e)
    {
      if (!ApplicationViewModel.DeviceConfiguration.USE_REAR_SCREEN)
        return;
      ApplicationViewModel.AdminMode = false;
    }

    private void ApplicationViewModel_DeviceStatusChangedEvent(
      object sender,
      DeviceStatusChangedEventArgs e)
    {
      NotifyOfPropertyChange<bool>((System.Linq.Expressions.Expression<Func<bool>>) (() => CanAdminButton));
    }

    public Conductor<Screen> Conductor { get; }

    public ApplicationViewModel ApplicationViewModel { get; }

    public bool CanAdminButton
    {
      get
      {
        int num = 0;
        if (ApplicationViewModel.DeviceConfiguration.USE_REAR_SCREEN && !ApplicationViewModel.AdminMode)
        {
          ApplicationViewModel applicationViewModel = ApplicationViewModel;
          bool? nullable;
          if (applicationViewModel == null)
          {
            nullable = new bool?();
          }
          else
          {
            AppSession currentSession = applicationViewModel.CurrentSession;
            nullable = currentSession != null ? new bool?(!currentSession.CountingStarted) : new bool?();
          }
          //num = nullable.HasValue ? nullable.Value : 1;
        }
        else
          num = 0;
        return num != 0;
      }
    }

    public void AdminButton()
    {
      ApplicationViewModel.AdminMode = true;
      //MenuBackendATMViewModel backendAtmViewModel = new MenuBackendATMViewModel("Main Menu", this.ApplicationViewModel, Conductor, this);
    }
  }
}
