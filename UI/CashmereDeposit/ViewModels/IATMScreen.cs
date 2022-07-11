
// Type: CashmereDeposit.ViewModels.IATMScreen

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


namespace CashmereDeposit.ViewModels
{
  public interface IATMScreen
  {
    void GetFirstPage();

    void GetLastPage();

    void GetPreviousPage();

    void GetNextPage();

    void GetPage(int PageID);

    void NavigateBack();
  }
}
