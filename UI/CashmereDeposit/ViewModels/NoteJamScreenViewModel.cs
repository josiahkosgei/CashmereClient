
//.NoteJamScreenViewModel




using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  internal class NoteJamScreenViewModel :  Conductor<Screen>
  {
    private string _NoteJamErrorTitleText;
    private string _NoteJamErrorDescriptionText;

    public ApplicationViewModel ApplicationViewModel { get; }

    public string NoteJamErrorTitleText
    {
        get => _NoteJamErrorTitleText;
        set
      {
        _NoteJamErrorTitleText = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => NoteJamErrorTitleText));
      }
    }

    public string NoteJamErrorDescriptionText
    {
        get => _NoteJamErrorDescriptionText;
        set
      {
        _NoteJamErrorDescriptionText = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => NoteJamErrorDescriptionText));
      }
    }

    public NoteJamScreenViewModel(ApplicationViewModel applicationViewModel)
    {
      ApplicationViewModel = applicationViewModel;
      InitialiseScreen();
    }

    private void InitialiseScreen()
    {
      NoteJamErrorDescriptionText = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("NoteJamErrorDescriptionText", "sys_NoteJamErrorDescriptionText", "Drop");
      NoteJamErrorTitleText = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText("NoteJamErrorTitleText", "sys_NoteJamErrorTitleText", "Reject");
    }
  }
}
