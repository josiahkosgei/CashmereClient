
//.CurrencyListScreenViewModel




using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Cashmere.Library.CashmereDataAccess.Entities;
using CashmereDeposit.Models;

namespace CashmereDeposit.ViewModels
{
  [Guid("392BC2B1-19AB-465F-B2D8-02E31D8E9392")]
  public class CurrencyListScreenViewModel : CustomerListScreenBaseViewModel
  {
    public CurrencyListScreenViewModel(
      string screenTitle,
      ApplicationViewModel applicationViewModel,
      bool required = false)
      : base(screenTitle, applicationViewModel, required)
    {
      ApplicationViewModel applicationViewModel1 = ApplicationViewModel;
      List<ATMSelectionItem<object>> atmSelectionItemList;
      if (applicationViewModel1 == null)
      {
        atmSelectionItemList = null;
      }
      else
      {
        List<Currency> currenciesAvailable = applicationViewModel1.CurrenciesAvailable;
        if (currenciesAvailable == null)
        {
          atmSelectionItemList = null;
        }
        else
        {
          IEnumerable<ATMSelectionItem<object>> source = currenciesAvailable.Select(x => new ATMSelectionItem<object>("{ResourceDir}/Resources\\Flags\\" + x.Flag + ".png", x.Name, x));
          atmSelectionItemList = source != null ? source.ToList() : null;
        }
      }
      FullList = atmSelectionItemList;
      GetFirstPage();
    }

    public void Cancel()
    {
        ApplicationViewModel.CancelSessionOnUserInput();
    }

    public override void PerformSelection()
    {
      ApplicationViewModel?.SetCurrency(SelectedFilteredList.Value as Currency);
      ApplicationViewModel.NavigateNextScreen();
    }
  }
}
