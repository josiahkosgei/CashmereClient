
// Type: CashmereDeposit.Models.Submodule.SubmoduleBase

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


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
