using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace CashmereDeposit.ViewModels
{
  [Guid("257981B3-4CB4-4863-940D-EA742ADF16B3")]
  public class TimoutDialogueBoxViewModel : DialogueScreenBase
  {
      public TimoutDialogueBoxViewModel(ApplicationViewModel applicationViewModel, int timerDuration = 30)
      : base(applicationViewModel, timerDuration, 2)
    {
      if (!(Application.Current.FindResource("Dialog_ScreenIdleTimeout_TitleText") is string str))
        str = "Hello";
      ScreenTitle = str;
      DialogImage = "Resources/Icons/Main/clock.png";
      MessageBoxButton = MessageBoxButton.YesNo;
      if (!(Application.Current.FindResource("Dialog_ScreenIdleTimeout_DescriptionText") is string))
        str = "Would you like more time?";
      DialogBoxMessage = str;
    }

    public static MessageBoxResult ShowDialog(
      ApplicationViewModel applicationViewModel,
      int timerDuration = 30)
    {
        using var dialogueBoxViewModel = new TimoutDialogueBoxViewModel(applicationViewModel, timerDuration = 30);
        applicationViewModel.ShowDialogBox(dialogueBoxViewModel);
        while (!dialogueBoxViewModel.HasReturned)
        {
            Thread.CurrentThread.Join(10);
            Thread.Sleep(100);
        }
        return dialogueBoxViewModel.MessageBoxResult;
    }
  }
}
