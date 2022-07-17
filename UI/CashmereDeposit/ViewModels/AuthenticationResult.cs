
//.AuthenticationResult




using System;
using Cashmere.Library.CashmereDataAccess.Entities;

namespace CashmereDeposit.ViewModels
{
  public class AuthenticationResult
  {
    public ApplicationUser InitiatingUser { get; set; }

    public ApplicationUser AuthorisingUser { get; set; }

    public string ActivityName { get; set; }

    public DateTime InitiateDate { get; set; }

    public DateTime AuthorisingDate { get; set; }

    public bool MustBeInitiated { get; set; }

    public bool MustBeAuthorised { get; set; }

    public LoginAuthResponse LoginAuthResponse { get; set; }

    public bool IsAuthenticated { get; set; }

    public string ServerErrorCode { get; set; }

    public string ServerErrorMessage { get; set; }

    public string PublicErrorCode { get; set; }

    public string PublicErrorMessage { get; set; }

    public bool IsSuccess { get; set; }
  }
}
