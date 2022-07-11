
// Type: CashmereDeposit.Utils.ClientValidationRules

// MVID: F63D4D22-EE07-4205-A184-9ED72F588748


using System.Text.RegularExpressions;

namespace CashmereDeposit.Utils
{
  public static class ClientValidationRules
  {
    public static bool RegexValidation(string input, string regularExpression) => Regex.Match(input, regularExpression, RegexOptions.IgnoreCase).Success;
  }
}
