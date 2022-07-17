
//.IATMScreen




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
