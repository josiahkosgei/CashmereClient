
// Type: CashmereDeposit.ViewModels.NoteJamScreenViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
  internal class NoteJamScreenViewModel : Conductor<Screen>, IShell
  {
    private string _NoteJamErrorTitleText;
    private string _NoteJamErrorDescriptionText;

    public ApplicationViewModel ApplicationViewModel { get; }

    public string NoteJamErrorTitleText
    {
        get { return _NoteJamErrorTitleText; }
        set
      {
        _NoteJamErrorTitleText = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => NoteJamErrorTitleText));
      }
    }

    public string NoteJamErrorDescriptionText
    {
        get { return _NoteJamErrorDescriptionText; }
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
