
//.TermsAndConditionsViewModel




using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using CashmereDeposit.Models.Submodule;
using CashmereDeposit.Utils;

namespace CashmereDeposit.ViewModels
{
  [Guid("C382EA93-1EE2-491C-9AD1-B4FEED09C3C7")]
  public class TermsAndConditionsViewModel : DepositorCustomerScreenBaseViewModel
  {
    private string _Dialog_OK_Caption;

    public string TermsAndConditionsText { get; set; }

    public string Dialog_OK_Caption
    {
        get { return _Dialog_OK_Caption; }
        set
      {
        _Dialog_OK_Caption = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => Dialog_OK_Caption));
      }
    }

    public TermsAndConditionsViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
        InitialiseScreen();
    }

    private void InitialiseScreen()
    {
      Dialog_OK_Caption = ApplicationViewModel.CashmereTranslationService?.TranslateSystemText(GetType().Name + ".InitialiseScreen Dialog_OK_Caption", "sys_Dialog_OK_Caption", "OK");
      CashmereTranslationService translationService = ApplicationViewModel.CashmereTranslationService;
      string str;
      if (translationService == null)
      {
        str = null;
      }
      else
      {
        string s = translationService.TranslateUserText(GetType().Name + ".InitialiseScreen TermsAndConditionsText", ApplicationViewModel?.CurrentTransaction?.TransactionText?.Terms, null);
        str = s != null ? s.CashmereReplace(ApplicationViewModel) : null;
      }
      TermsAndConditionsText = str;
    }

    public void AcceptTerms()
    {
      lock (ApplicationViewModel.NavigationLock)
      {
        if (!CanNext)
          return;
        CanNext = false;
        ApplicationViewModel.ShowDialog(new WaitForProcessScreenViewModel(ApplicationViewModel));
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        backgroundWorker.WorkerReportsProgress = false;
        backgroundWorker.DoWork += new DoWorkEventHandler(StatusWorker_DoWork);
        backgroundWorker.RunWorkerAsync();
      }
    }

    private void StatusWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        ApplicationViewModel.TermsAccepted();
    }

    public void RejectTerms()
    {
        ApplicationViewModel.TermsAccepted(false);
    }
  }
}
