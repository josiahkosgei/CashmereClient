
//.AdminDeviceSummaryViewModel




using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using System.Windows;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  internal class AdminDeviceSummaryViewModel : FormViewModelBase
  {
    private Device Device { get; }

    public AdminDeviceSummaryViewModel(
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject,
      bool isNewEntry)
      : base(applicationViewModel, conductor, callingObject, isNewEntry)
    {
      Device = applicationViewModel?.ApplicationModel?.GetDevice(depositorDbContext);
      if (Device == null)
        ApplicationViewModel.Log.Error(GetType().Name, 106, ApplicationErrorConst.ERROR_NULL_REFERENCE_EXCEPTION.ToString(), "Device is null when constructing AdminDeviceSummaryViewModel");
      if (!(Application.Current.FindResource("DeviceSummaryScreenTitle") is string str))
        str = "Device Summary";
      ScreenTitle = str;
      ActivateItemAsync(new FormListViewModel(this));
    }
  }
}
