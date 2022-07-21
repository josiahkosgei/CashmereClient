
//.DepositorScreenViewModelBase




using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess;

using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels
{
    public class DepositorScreenViewModelBase : Conductor<Screen>
    {
        public ApplicationViewModel ApplicationViewModel;
        protected DepositorDBContext DBContext = new();

        protected string ScreenTitle { get; set; }

        protected Screen CallingObject { get; set; }

        public string CancelButton_Caption { get; set; }

        public string BackButton_Caption { get; set; }

        public string NextButton_Caption { get; set; }

        public string GetFirstPageButton_Caption { get; set; }

        public string GetPreviousPageButton_Caption { get; set; }

        public string GetNextPageButton_Caption { get; set; }

        public string GetLastPageButton_Caption { get; set; }

        public DepositorScreenViewModelBase(
          string screenTitle,
          ApplicationViewModel applicationViewModel,
          Screen callingObject)
        {
            ScreenTitle = screenTitle;
            ApplicationViewModel = applicationViewModel;
            CallingObject = callingObject;
            CancelButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.CancelButton_Caption", "sys_CancelButton_Caption", "Cancel");
            BackButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackButton_Caption", "sys_BackButton_Caption", "Back");
            NextButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.BackButton_Caption", "sys_NextButton_Caption", "Next");
            GetFirstPageButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetPreviousPageButton_Caption", "sys_GetFirstPageButton_Caption", "First");
            GetPreviousPageButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetPreviousPageButton_Caption", "sys_GetPreviousPageButton_Caption", "Prev");
            GetNextPageButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetNextPageButton_Caption", "sys_GetNextPageButton_Caption", "More");
            GetLastPageButton_Caption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText("ATMScreenViewModelBase.GetNextPageButton_Caption", "sys_GetLastPageButton_Caption", "Last");
        }

        public void Back()
        {
            ApplicationViewModel.ShowDialog(CallingObject);
        }

        public void Cancel()
        {
            ApplicationViewModel.CloseDialog();
        }
    }
}
