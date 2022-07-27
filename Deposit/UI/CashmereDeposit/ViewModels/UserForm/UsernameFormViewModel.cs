
//sernameFormViewModel




using Caliburn.Micro;
using CashmereDeposit.Interfaces;

namespace CashmereDeposit.ViewModels.UserForm
{
    internal class UsernameFormViewModel : Conductor<Screen>
    {
        private string _textboxText;
        private string _errorMessageTextBlock;

        public UserControlViewModel UserControlViewModel { get; set; }

        public string DataEntryLabel { get; set; }

        public string DataEntryTextbox
        {
            get => _textboxText;
            set
            {
                _textboxText = value;
                NotifyOfPropertyChange("TextBoxText");
            }
        }

        public string ErrorMessageTextBlock
        {
            get => _errorMessageTextBlock;
            set
            {
                _errorMessageTextBlock = value;
                NotifyOfPropertyChange(nameof(ErrorMessageTextBlock));
            }
        }

        public UsernameFormViewModel(UserControlViewModel userControlViewModel)
        {
            UserControlViewModel = userControlViewModel;
        }

        public void Validate()
        {
            var str = UserControlViewModel.ValidateUsername(DataEntryTextbox);
            if (str == null)
                TryCloseAsync(new bool?(true));
            else
                ErrorMessageTextBlock = str;
        }

        public void Back()
        {
            TryCloseAsync(new bool?(false));
        }
    }
}
