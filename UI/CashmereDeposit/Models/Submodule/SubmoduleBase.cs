
// Type: CashmereDeposit.Models.Submodule.SubmoduleBase




using System;
using CashmereDeposit.ViewModels;
using CashmereUtil.Licensing;

namespace CashmereDeposit.Models.Submodule
{
  public abstract class SubmoduleBase
  {
    public CDMLicense License { get; set; }

    public string ModuleName { get; set; }

    public Guid ModuleID { get; set; }

    protected ApplicationViewModel ApplicationViewModel { get; }

    public SubmoduleBase(
      ApplicationViewModel applicationViewModel,
      CDMLicense license,
      Guid subModuleID,
      string subModuleName)
    {
      ApplicationViewModel = applicationViewModel;
      License = license;
      ModuleID = subModuleID;
      ModuleName = subModuleName;
    }
  }
}
