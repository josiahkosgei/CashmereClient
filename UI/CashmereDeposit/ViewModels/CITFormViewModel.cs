
// Type: CashmereDeposit.ViewModels.CITFormViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


//using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Caliburn.Micro;
using Cashmere.Library.CashmereDataAccess.Entities;
using Cashmere.Library.CashmereDataAccess.StoredProcs;
using Microsoft.EntityFrameworkCore;

namespace CashmereDeposit.ViewModels
{
    internal class CITFormViewModel : FormViewModelBase
  {
    private string _bagNumber;
    private string _sealNumber;

    protected string NewBagNumber
    {
        get { return _bagNumber; }
        set
      {
        _bagNumber = value;
        NotifyOfPropertyChange("Bagnumber");
      }
    }

    protected string SealNumber
    {
        get { return _sealNumber; }
        set
      {
        _sealNumber = value;
        NotifyOfPropertyChange(nameof (SealNumber));
      }
    }

    private CIT CIT { get; set; }

    private CITDenomination CITDenominations { get; set; }

    private DateTime thisCITToDate { get; set; }

    private DateTime thisCITFromDate { get; set; }

    private Device Device { get; }

    public CITFormViewModel(
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject,
      bool isNewEntry)
      : base(applicationViewModel, conductor, callingObject, isNewEntry)
    {
      Device = applicationViewModel.ApplicationModel.GetDevice(depositorDbContext);
      ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_CITFormScreenTitle", "CIT Operation");
      NextCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_StartCITCommand_Caption", "Start CIT");
      Fields.Add(new FormListItem()
      {
        DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_NewBagNumber_Caption", "sys_CIT_NewBagNumber_Caption", "New Bag Number"),
        Validate = new Func<string, string>(ValidateBagNumber),
        ValidatedText = NewBagNumber,
        DataTextBoxLabel = NewBagNumber,
        FormListItemType = FormListItemType.ALPHATEXTBOX
      });
      Fields.Add(new FormListItem()
      {
        DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_CIT_SealNumber_Caption", "sys_CIT_SealNumber_Caption", "Seal Number"),
        Validate = new Func<string, string>(ValidateSealNumber),
        ValidatedText = SealNumber,
        DataTextBoxLabel = SealNumber,
        FormListItemType = FormListItemType.ALPHATEXTBOX
      });
      ActivateItemAsync(new FormListViewModel(this));
    }

    public string ValidateBagNumber(string newBagNumber)
    {
      if (string.IsNullOrWhiteSpace(newBagNumber))
        return "Please enter a Bag Number";
      NewBagNumber = newBagNumber;
      return null;
    }

    public string ValidateSealNumber(string newSealNumber)
    {
      if (depositorDbContext.CITs.Where(x => x.SealNumber == newSealNumber).ToList().Count > 0)
        return "Seal Number must be unique and unused.";
      SealNumber = newSealNumber;
      return null;
    }

    public override async Task<string> SaveForm()
    {
      int num = FormValidation();
      if (num > 0)
        return string.Format("Form validation failed with {0:0} errors. Kindly correct them and try saving again", num);
      CIT cit = createCIT();
      if (cit != null)
      {
        depositorDbContext.CITs.Add(cit);
        try
        {
          ApplicationViewModel.SaveToDatabase(depositorDbContext);
          ApplicationViewModel.StartCIT(SealNumber);
          ApplicationViewModel.AdminMode = false;
          ApplicationViewModel.ShowErrorDialog(new OutOfOrderScreenViewModel(ApplicationViewModel));
        }
        catch (Exception ex)
        {
          ApplicationViewModel.Log.Error(GetType().Name, nameof (CITFormViewModel), nameof (SaveForm), "Error saving CIT to database: {0}", new object[1]
          {
            ex.MessageString()
          });
          return "Error occurred. Contact administrator.";
        }
      }
      return null;
    }

    public override int FormValidation()
    {
      int num = 0;
      foreach (FormListItem field in Fields)
      {
        Func<string, string> validate = field.Validate;
        string str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
        if (str != null)
        {
          field.ErrorMessageTextBlock = str;
          ++num;
        }
      }
      return num;
    }

    private CIT createCIT()
    {
      try
      {
        ApplicationViewModel.HandleIncompleteSession();
      }
      catch (Exception ex)
      {
        ApplicationViewModel.Log.WarningFormat("CITFormViewModel.createCIT()", "Error closing incomplete sessions", "System", "{0}>>{1}>>{2}", ex?.Message, ex?.InnerException?.Message, ex?.InnerException?.InnerException?.Message);
      }
      CIT lastCIT = ApplicationViewModel.lastCIT;
      int num;
      if (lastCIT == null)
      {
        num = 1;
      }
      else
      {
        DateTime toDate = lastCIT.ToDate;
        num = 0;
      }
      thisCITFromDate = num != 0 ? DateTime.MinValue : lastCIT.ToDate;
      thisCITToDate = DateTime.Now;
      CIT = new CIT()
      {
        Id = GuidExt.UuidCreateSequential(),
        CITDate = DateTime.Now,
        StartUserId = ApplicationViewModel.CurrentUser.Id,
        AuthUserId = new Guid?(ApplicationViewModel.ValidatingUser.Id),
        FromDate = new DateTime?(thisCITFromDate),
        ToDate = thisCITToDate,
        DeviceId = Device.Id,
        OldBagNumber = lastCIT?.NewBagNumber,
        NewBagNumber = NewBagNumber,
        SealNumber = SealNumber,
        CITError = 0,
        Complete = false
      };
      DbSet<Transaction> transactions = depositorDbContext.Transactions;
      Expression<Func<Transaction, bool>> predicate = x => x.CITId == new Guid?() && x.DeviceId == Device.Id && x.TxStartDate >= CIT.FromDate && x.TxStartDate <= CIT.ToDate;
      foreach (Transaction transaction in transactions.Where(predicate).ToList())
        transaction.CIT = CIT;
      foreach (GetCITDenominationByDatesResult denominationByDatesResult in depositorContextProcedures.GetCITDenominationByDatesAsync(new DateTime?(thisCITFromDate), new DateTime?(thisCITToDate)).Result.OrderBy((Func<GetCITDenominationByDatesResult, int>) (x => x.Denom)).ToList())
        CIT.CITDenominations.Add(new CITDenomination()
        {
          Id = GuidExt.UuidCreateSequential(),
          CurrencyId = denominationByDatesResult.Txcurrency,
          Datetime = new DateTime?(thisCITToDate),
          Denom = denominationByDatesResult.Denom,
          Count = denominationByDatesResult.Count.Value,
          Subtotal = denominationByDatesResult.SubTotal.Value
        });
      CreateCITTransactions(CIT);
      return CIT;
    }

    private void CreateCITTransactions(CIT cit)
    {
      try
      {
        ApplicationViewModel.Log.DebugFormat("ApplicationViewModel", "Generate CITTransaction", nameof (CreateCITTransactions), "Generating CITTransactions for CIT id={0}", cit.Id);
        List<CITTransaction> citTransactionList = new List<CITTransaction>(5);
        foreach (IGrouping<string, CITDenomination> grouping in cit.CITDenominations.GroupBy(denom => denom.CurrencyId))
        {
          IGrouping<string, CITDenomination> currency = grouping;
          long num = currency.Sum(x => x.Subtotal);
          if (num > 0L)
            citTransactionList.Add(new CITTransaction()
            {
              Id = Guid.NewGuid(),
              CITId = cit.Id,
              AccountNumber = (depositorDbContext.DeviceCITSuspenseAccounts.FirstOrDefault(x => x.DeviceId == cit.DeviceId && x.CurrencyCode.Equals(currency.Key, StringComparison.OrdinalIgnoreCase) && x.Enabled == true) ?? throw new NullReferenceException(string.Format("No valid CITSuspenseAccount found for currency {0}", currency))).AccountNumber,
              SuspenseAccount = (depositorDbContext.DeviceSuspenseAccounts.FirstOrDefault(x => x.DeviceId == cit.DeviceId && x.CurrencyCode.Equals(currency.Key, StringComparison.OrdinalIgnoreCase) && x.Enabled == true) ?? throw new NullReferenceException(string.Format("No valid DeviceSuspenseAccount found for currency {0}", currency))).AccountNumber,
              Datetime = DateTime.Now,
              Amount = num,
              Currency = currency.Key,
              Narration = string.Format("CIT {0} on {1:yyyyMMddTHHmmss}", Device.DeviceNumber, cit.CITDate)
            });
        }
        depositorDbContext.CITTransactions.AddRange(citTransactionList);
        ApplicationViewModel.SaveToDatabase(depositorDbContext);
      }
      catch (Exception ex)
      {
        ApplicationViewModel.Log.ErrorFormat("ApplicationViewModel.CreateCITTransactions", 113, ApplicationErrorConst.ERROR_CIT_POST_FAILURE.ToString(), "Error posting CIT {0}: {1}", cit.Id, ex.MessageString());
        throw;
      }
    }
  }
}
