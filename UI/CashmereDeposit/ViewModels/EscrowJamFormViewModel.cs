
// Type: CashmereDeposit.ViewModels.EscrowJamFormViewModel

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using Caliburn.Micro;
using Cashmere.Library.Standard.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  internal class EscrowJamFormViewModel : FormViewModelBase
  {
    private string _additionalInfo;
    private Decimal _retreived_amount;
    private string _RetreivedAmountString;

    protected string AdditionalInfo
    {
        get { return _additionalInfo; }
        set
      {
        _additionalInfo = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => AdditionalInfo));
      }
    }

    protected Decimal RetreivedAmount
    {
        get { return _retreived_amount; }
        set
      {
        _retreived_amount = value;
        NotifyOfPropertyChange((Expression<Func<Decimal>>) (() => RetreivedAmount));
      }
    }

    public string RetreivedAmountString
    {
        get { return _RetreivedAmountString; }
        set
      {
        _RetreivedAmountString = value;
        NotifyOfPropertyChange((Expression<Func<string>>) (() => RetreivedAmountString));
      }
    }

    private EscrowJam EscrowJam { get; set; }

    public EscrowJamFormViewModel(
      ApplicationViewModel applicationViewModel,
      Conductor<Screen> conductor,
      Screen callingObject,
      bool isNewEntry)
      : base(applicationViewModel, conductor, callingObject, isNewEntry)
    {
      if (ApplicationViewModel.EscrowJam == null)
      {
        ApplicationViewModel.Log.Error(nameof (EscrowJamFormViewModel), "Invalid EscrowJam", "ctor", "Currenct EscrowJam is null", Array.Empty<object>());
        ApplicationViewModel.AdminMode = false;
        ApplicationViewModel.ShowErrorDialog(new OutOfOrderScreenViewModel(ApplicationViewModel));
      }
      else
      {
        ApplicationViewModel.Log.Error(nameof (EscrowJamFormViewModel), "Processing", "ctor", "Creating EscrowJamForm for EscrowJam '{0}'", new object[1]
        {
          applicationViewModel.EscrowJam.Id
        });
        EscrowJam = depositorDbContext.EscrowJams.FirstOrDefault(x => x.Id == ApplicationViewModel.EscrowJam.Id);
        AdditionalInfo = EscrowJam?.AdditionalInfo;
        ScreenTitle = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor ScreenTitle", "sys_EscrowJamFormScreenTitle", "Clear Escrow Jam");
        NextCaption = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor NextCaption", "sys_EndEscrowJamRecoveryCommand_Caption", "Complete");
        Fields.Add(new FormListItem()
        {
          DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_EJAM_RetreivedAmountString_Caption", "sys_EJAM_RetreivedAmountString_Caption", "Retreived Amount"),
          Validate = new Func<string, string>(ValidateRetreivedAmountString),
          ValidatedText = RetreivedAmountString,
          DataTextBoxLabel = RetreivedAmountString,
          FormListItemType = FormListItemType.NUMERICTEXTBOX
        });
        Fields.Add(new FormListItem()
        {
          DataLabel = ApplicationViewModel.CashmereTranslationService.TranslateSystemText(GetType().Name + ".Constructor sys_EJAM_AdditionalInfo_Caption", "sys_EJAM_AdditionalInfo_Caption", "Additional Info"),
          Validate = new Func<string, string>(ValidateAdditionalInfo),
          ValidatedText = AdditionalInfo,
          DataTextBoxLabel = AdditionalInfo,
          FormListItemType = FormListItemType.ALPHATEXTBOX
        });
        ApplicationViewModel.DeviceManager.EscrowJamEndEvent += new EventHandler<EventArgs>(DeviceManager_EscrowJamEndEvent);
        ActivateItemAsync(new FormListViewModel(this));
      }
    }

    private void DeviceManager_EscrowJamEndEvent(object sender, EventArgs e)
    {
    }

    public string ValidateAdditionalInfo(string additionalInfo)
    {
      AdditionalInfo = additionalInfo;
      return null;
    }

    public string ValidateRetreivedAmountString(string RetreivedAmountStringString)
    {
      Decimal result;
      if (!Decimal.TryParse(RetreivedAmountStringString, out result))
        return "Invalid Retreived Amount. Not a number";
      if (result < 0M || result > 100000000M)
        return "Invalid Retreived Amount.";
      RetreivedAmount = result;
      return null;
    }

    public override async Task<string> SaveForm()
    {
      int num = FormValidation();
      if (num > 0)
        return string.Format("Form validation failed with {0:0} errors. Kindly correct them and try saving again", num);
      if (EscrowJam != null)
      {
        try
        {
          EscrowJam.RecoveryDate = new DateTime?(DateTime.Now);
          EscrowJam.RetreivedAmount = (long) (RetreivedAmount * 100M);
          EscrowJam.AdditionalInfo = AdditionalInfo;
          EscrowJam.AuthorisingUser = new Guid?(ApplicationViewModel.ValidatingUser.Id);
          EscrowJam.InitialisingUser = new Guid?(ApplicationViewModel.CurrentUser.Id);
          ApplicationViewModel.SaveToDatabase(depositorDbContext);
          ApplicationViewModel.EndEscrowJam();
        }
        catch (Exception ex)
        {
          ApplicationViewModel.Log.Error(GetType().Name, nameof (EscrowJamFormViewModel), nameof (SaveForm), "Error saving escrow jam to database: {0}", new object[1]
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
  }
}
