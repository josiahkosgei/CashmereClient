
// Type: CashmereDeposit.Utils.CashmereDeviceStatus




using System;
using System.Linq.Expressions;
using Caliburn.Micro;
using Cashmere.Library.Standard.Statuses;

namespace CashmereDeposit.Utils
{
  public class CashmereDeviceStatus : PropertyChangedBase
  {
    private CashmereDeviceState _cashmereDeviceState;
    private ControllerStatus _controllerStatus;
    private CoreBankingStatus _coreBankingStatus;

    public CashmereDeviceState CashmereDeviceState
    {
      get => _cashmereDeviceState & ~CashmereDeviceState.COUNTING_DEVICE & ~CashmereDeviceState.DATABASE & ~CashmereDeviceState.PRINTER;
      set
      {
        if (_cashmereDeviceState == value)
          return;
        _cashmereDeviceState = value;
        NotifyOfPropertyChange((Expression<Func<CashmereDeviceState>>) (() => CashmereDeviceState));
      }
    }

    public ControllerStatus ControllerStatus
    {
      get => _controllerStatus;
      set
      {
        _controllerStatus = value;
        NotifyOfPropertyChange((Expression<Func<ControllerStatus>>) (() => ControllerStatus));
      }
    }

    public CoreBankingStatus CoreBankingStatus
    {
      get => _coreBankingStatus;
      set
      {
        _coreBankingStatus = value;
        NotifyOfPropertyChange((Expression<Func<CoreBankingStatus>>) (() => CoreBankingStatus));
      }
    }
  }
}
