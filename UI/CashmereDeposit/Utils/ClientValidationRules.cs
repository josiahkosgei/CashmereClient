
// Type: CashmereDeposit.Utils.ClientValidationRules




using System.Text.RegularExpressions;

namespace CashmereDeposit.Utils
{
  public static class ClientValidationRules
  {
    public static bool RegexValidation(string input, string regularExpression) => Regex.Match(input, regularExpression, RegexOptions.IgnoreCase).Success;
  }
}
