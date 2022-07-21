using Caliburn.Micro;

namespace CashmereDeposit.ViewModels.RearScreen
{
    public interface ICashmereWindowConductor
    {
        void CloseDialog(bool generateScreen = true);

        void ShowDialog(Screen screen);

        void ShowDialogBox(Screen screen);
    }
}
