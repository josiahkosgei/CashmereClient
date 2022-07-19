
using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  public class WaitForProcessScreenViewModel : Conductor<Screen>, IShell
  {
    public ApplicationViewModel ApplicationViewModel { get; }

    public string ProcessingTitleText
    {
      get
      {
        try
        {
          return ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof (ProcessingTitleText), "sys_ProcessingTitleText", "Processing. Please wait...", ApplicationViewModel.CurrentLanguage) ?? throw new NullReferenceException("Invalid translation: TranslateSystemText is null");
        }
        catch (Exception ex)
        {
          ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof (ProcessingTitleText), "TranslationError", "Error translating: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
          return "[Translation Error]";
        }
      }
    }

    public string ProcessingDescriptionText
    {
      get
      {
        try
        {
          return ApplicationViewModel.CashmereTranslationService.TranslateSystemText(nameof (ProcessingDescriptionText), "sys_ProcessingDescriptionText", "The requested operation is being carried out and will conclude shortly.", ApplicationViewModel.CurrentLanguage) ?? throw new NullReferenceException("Invalid translation: TranslateSystemText is null");
        }
        catch (Exception ex)
        {
          ApplicationViewModel.Log.WarningFormat(GetType().Name, nameof (ProcessingDescriptionText), "TranslationError", "Error translating: {0}>>{1}", ex.Message, ex?.InnerException?.Message);
          return "[Translation Error]";
        }
      }
    }

    public WaitForProcessScreenViewModel(ApplicationViewModel applicationViewModel)
    {
      ApplicationViewModel = applicationViewModel;
      NotifyOfPropertyChange(() => ProcessingTitleText);
      NotifyOfPropertyChange(() => ProcessingDescriptionText);
    }
  }
}
